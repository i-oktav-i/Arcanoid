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
    if (gameData.resetOnStart) gameData.Load(); // gameData.Reset();
  }

  private void Start() {
    uiManager = Camera.main.GetComponent<UIManager>();
    backgroundController = Instantiate(backgroundControllerPrefab);
    InitGameState();
    InitLevel(gameData.level);
  }

  // TODO: move to input manager
  private void Update() {
    if (uiManager.IsMenuActive) return;
    if (Input.GetKeyDown(KeyCode.M)) gameData.IsMusicOn = !gameData.IsMusicOn;
    if (Input.GetKeyDown(KeyCode.S)) gameData.IsSoundOn = !gameData.IsSoundOn;
    if (Input.GetKeyDown(KeyCode.N)) {
      gameData.Reset();
      SceneManager.LoadScene("Main");
    }
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    if (Input.GetButtonDown("Pause")) uiManager.ShowPauseMenu();
  }

  public void OnBlockDestroyed(AbstractBlock blockInstance)
  {
    gameData.points += blockInstance.points;
    gameData.pointsToBall += blockInstance.points;
    if (gameData.pointsToBall >= gameData.requiredPointsToBall) {
      Debug.Log("Ball added to inventory");
      gameData.BallsCapacity++;
      gameData.pointsToBall -= gameData.requiredPointsToBall;
    }
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
      SceneManager.LoadScene("Main");
    });
    gameData.SubscribeBallsEnd(() => {
      Debug.Log($"Level {level} lose.");
      gameData.Reset();
      SceneManager.LoadScene("Main");
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
