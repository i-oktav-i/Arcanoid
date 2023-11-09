using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  [SerializeField] private AbstractBackgroundController backgroundControllerPrefab;
  [SerializeField] private AbstractBoardController boardControllerPrefab;

  private GameObject boardControllerHolder;
  private AbstractBackgroundController backgroundController;
  private int level = 1;

  public int Level {
    get => level;
    set {
      level = value;
      InitLevel(value);
    }
  }

  public GameDataScript gameData;

  static bool _gameStarted; // false by default

  private void InitGameState() {
    if (_gameStarted) return;
    _gameStarted = true;
    if (gameData.resetOnStart) gameData.Reset();
  }

  private void Start() {
    backgroundController = Instantiate(backgroundControllerPrefab);
    InitGameState();
    InitLevel(level);
  }

  private void InitLevel(int level) {
    backgroundController.SetBackground(level - 1);

    Destroy(boardControllerHolder);
    boardControllerHolder = new("BoardController");

    AbstractBoardController boardController = Instantiate(boardControllerPrefab);
    boardController.InitBoard(new(0, 0), level);
    boardController.SubscribeLevelComplete(() => Level += 1);
    boardController.SubscribeLevelLose(() => Level += 0);

    boardController.transform.SetParent(boardControllerHolder.transform);
  }

  private void OnDestroy() {
    Destroy(boardControllerHolder);
  }
}
