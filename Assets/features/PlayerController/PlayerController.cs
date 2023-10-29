using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : AbstractPlayerController {
  [SerializeField] private AbstractPlayer playerPrefab;
  private AbstractPlayer player;
  [SerializeField] private AbstractBall ballPrefab;
  private AbstractBall spawnedBall;
  private int notLaunchedBalls = 1;
  private int ballsCount = 0;
  private bool blockInput = false;
  private List<Action> ballsEndCallbacks = new();

  public int BallsCount {
    get => ballsCount; set {
      ballsCount = value;

      if (value > 0 || notLaunchedBalls > 0) return;

      ballsEndCallbacks.ForEach(item => item());
    }
  }

  public override void AddBalls(int count) {
    notLaunchedBalls = count;

    SpawnBall();
  }

  private void Awake() {
    player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
    player.transform.SetParent(transform);

    SpawnBall();
  }

  private void SpawnBall() {
    if (spawnedBall != null || notLaunchedBalls <= 0) return;

    spawnedBall = Instantiate(ballPrefab, player.transform.position + new Vector3(1.1f, 1.1f), Quaternion.identity);
    spawnedBall.transform.SetParent(player.transform);

    BallsCount += 1;
    _ = spawnedBall.SubscribeDestroy(() => BallsCount -= 1);

    notLaunchedBalls -= 1;
  }

  private void Update() {
    if (!blockInput && Input.GetKeyDown(KeyCode.Space) && spawnedBall) {
      blockInput = true;

      spawnedBall.transform.SetParent(transform);
      spawnedBall.Launch(new(600, 600));
      spawnedBall = null;

      SpawnBall();
    }
    else {
      blockInput = false;
    }
  }

  override public Action SubscribeBallsEnd(Action callback) {
    ballsEndCallbacks.Add(callback);

    return () => ballsEndCallbacks.Remove(callback);
  }
  override public void UnsubscribeBallsEnd(Action callback) {
    ballsEndCallbacks.Remove(callback);
  }
}
