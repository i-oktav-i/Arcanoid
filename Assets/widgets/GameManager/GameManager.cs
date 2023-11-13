using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  [SerializeField] private AbstractBackgroundController backgroundControllerPrefab;
  [SerializeField] private AbstractBoardController boardControllerPrefab;

  private GameObject boardControllerHolder;
  private AbstractBackgroundController backgroundController;

  private UIManager uiManager;
  public GameState gameData;

  static bool _gameStarted; // false by default

  private void InitGameState() {
    if (_gameStarted) return;
    _gameStarted = true;
    if (gameData.resetOnStart) gameData.Load();
  }

  private void Start() {
    uiManager = Camera.main.GetComponent<UIManager>();
    backgroundController = Instantiate(backgroundControllerPrefab);
    InitGameState();
    InitLevel(gameData.level);
    gameData.SubscribePointsToBall(OnPointsToBallChanged);
  }

  public void StartNewGame() {
    gameData.Reset();
    InitLevel(InitialGameState.Level);
    Cursor.visible = false;
    if (uiManager.IsMenuActive) {
      uiManager.HidePauseMenu();
      uiManager.HideMainMenu();
    }
  }

  private void Update() {
    if (uiManager.IsMenuActive) return;
    if (Input.GetKeyDown(KeyCode.N)) StartNewGame();
  }

  IEnumerator PlayBonusBallSound(AbstractBlock blockInstance) {
    for (int i = 0; i < 10; i++) {
      yield return new WaitForSeconds(0.2f);
      blockInstance.PlayOnDestroySound();
    }
  }

  public void OnPointsToBallChanged() {
    if (gameData.PointsToBall < gameData.requiredPointsToBall) return;
    gameData.BallsCapacity += 1;
    Debug.Log("Ball added to inventory");
    gameData.PointsToBall -= gameData.requiredPointsToBall;
  }

  public void OnBlockDestroyed(AbstractBlock blockInstance) {
    gameData.points += blockInstance.points;
    gameData.PointsToBall += blockInstance.points;
  }

  private void InitLevel(int level) {
    Debug.Log($"Initializing level {level}...");

    backgroundController.SetBackground(level - 1);

    Destroy(boardControllerHolder);
    boardControllerHolder = new("BoardController");

    AbstractBoardController boardController = Instantiate(boardControllerPrefab);
    boardController.InitBoard(new(0, 0), level, OnBlockDestroyed);
    boardController.SubscribeBlocksEnd(() => {
      Debug.Log($"Level {level} COMPLETE!");
      if (gameData.level < LevelsConfig.MaxLevel) gameData.level += 1;
      else uiManager.ShowMainMenu(true);
      InitLevel(gameData.level);
    });
    gameData.SubscribeBallsEnd(() => {
      Debug.Log($"Level {level} lose.");
      StartNewGame();
    });

    boardController.transform.SetParent(boardControllerHolder.transform);

    Debug.Log($"Level {level} initialization complete!");
  }

  private void OnDestroy() {
    Destroy(boardControllerHolder);
  }

  void OnApplicationQuit() {
    gameData.Save();
  }
}
