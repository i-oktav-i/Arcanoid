using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : AbstractPlayerController {
  [SerializeField] private AbstractPlayer playerPrefab;
  private AbstractPlayer player;
  [SerializeField] private AbstractBall ballPrefab;

  private List<AbstractBall> ballsMag = new();
  private int releasedBallsCnt = 0;

  private bool blockInput = false;
  private List<Action> ballsEndCallbacks = new();

  public GameDataScript gameData;

  private bool isDestroy = false;

  private int AmmoCnt {
    get => ballsMag.Count;
  }

  private int BallsCapacity {
    get => gameData.balls;
    set {
      gameData.balls = value;

      if (value > 0) return;

      OnBallsEnd();
    }
  }

  public override void AddBalls(int ballsRequested) {
    if (isDestroy) return;
    var ballsToSpawn = Math.Min(ballsRequested, InitialGameState.BallsCnt);
    for (int i = 0; i < ballsToSpawn; i++) {
      SpawnBall();
    }
  }

  private void Awake() {
    player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
    player.transform.SetParent(transform);
    AddBalls(InitialGameState.BallsMag);
  }

  private void SpawnBall() {
    var newBall = Instantiate(ballPrefab, player.transform.position + new Vector3(1.1f, 1.1f), Quaternion.identity);
    newBall.transform.SetParent(player.transform);

    ballsMag.Add(newBall);
    _ = newBall.SubscribeDestroy(() => {
      BallsCapacity--;
      releasedBallsCnt--;
      // TODO: add check if all blocks have been destroyed -
      if (BallsCapacity <= 0) {
        gameData.Reset();
        SceneManager.LoadScene("Main");
        return;
      }
      if (releasedBallsCnt <= 0) {
        AddBalls(InitialGameState.BallsMag);
      }
    });
  }

  private void Update() {
    if (!blockInput && Input.GetKeyDown(KeyCode.Space) && AmmoCnt > 0) {
      blockInput = true;

      var ball = ballsMag[0];
      ballsMag.RemoveAt(0); // does't really matter if it's 0 or len - 1, just need pop

      ball.transform.SetParent(transform);
      ball.Launch(new(600, 600));
      releasedBallsCnt++;
    }
    else {
      blockInput = false;
    }
  }

   public override Action SubscribeBallsEnd(Action callback) {
    ballsEndCallbacks.Add(callback);

    return () => ballsEndCallbacks.Remove(callback);
  }
  public override void UnsubscribeBallsEnd(Action callback) {
    ballsEndCallbacks.Remove(callback);
  }

  private void OnBallsEnd() {
    if (isDestroy) return;

    ballsEndCallbacks.ForEach(callback => callback());
  }

  private void OnDestroy() => isDestroy = true;
}
