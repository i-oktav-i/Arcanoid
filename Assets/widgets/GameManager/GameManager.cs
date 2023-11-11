using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  [SerializeField] private AbstractBackgroundController backgroundControllerPrefab;
  [SerializeField] private AbstractBoardController boardControllerPrefab;

  private GameObject boardControllerHolder;
  private AbstractBackgroundController backgroundController;

  public GameDataScript gameData;

  static bool _gameStarted; // false by default

  private void InitGameState() {
    if (_gameStarted) return;
    _gameStarted = true;
    if (gameData.resetOnStart) gameData.Reset();
  }

  private void Start() {
    Cursor.visible = false;
    backgroundController = Instantiate(backgroundControllerPrefab);
    InitGameState();
    InitLevel(gameData.level);
  }

  private void InitLevel(int level) {
    Debug.Log($"Initializing level {level}...");

    backgroundController.SetBackground(level - 1);

    Destroy(boardControllerHolder);
    boardControllerHolder = new("BoardController");

    AbstractBoardController boardController = Instantiate(boardControllerPrefab);
    boardController.InitBoard(new(0, 0), level);
    boardController.SubscribeLevelComplete(() => {
      if (gameData.level < LevelsConfig.MaxLevel) gameData.level += 1;
      SceneManager.LoadScene("Main");
    });
    boardController.SubscribeLevelLose(() => gameData.level += 0);

    boardController.transform.SetParent(boardControllerHolder.transform);

    Debug.Log($"Level {level} initialization complete!");
  }

  private void OnDestroy() {
    Destroy(boardControllerHolder);
  }

  // TODO: move to some view layer
  void OnGUI()
  {
    GUI.Label(new Rect(5, 4, Screen.width - 10, 100),
      string.Format(
        "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>"+
        " Score <b>{2}</b></size></color>",
        gameData.level, gameData.balls, gameData.points
        ));
  }
}
