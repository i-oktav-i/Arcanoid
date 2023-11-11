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

  private bool blockInput = false;

  public GameState gameData;

  private bool isDestroy = false;
  private void OnDestroy() => isDestroy = true;

  private int AmmoCount {
    get => ballsMag.Count;
  }

  private int releasedBallsCnt = 0;

  public override void FillBallsMag() {
    if (isDestroy) return;
    var ballsToSpawn = Math.Min(InitialGameState.BallsMagCapacity, gameData.BallsCapacity);
    for (int i = 0; i < ballsToSpawn; i++) SpawnBall();
  }

  private void Awake() {
    player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
    player.transform.SetParent(transform);
    FillBallsMag();
  }

  private void SpawnBall() {
    var newBall = Instantiate(ballPrefab, player.transform.position + new Vector3(1.1f, 1.1f), Quaternion.identity);
    newBall.transform.SetParent(player.transform);

    ballsMag.Add(newBall);
    _ = newBall.SubscribeDestroy(() => {
      if (isDestroy) return;
      gameData.BallsCapacity--;
      releasedBallsCnt--;
      if (releasedBallsCnt <= 0) FillBallsMag();
    });
  }

  private void Update() {
    if (!blockInput && Input.GetKeyDown(KeyCode.Space) && AmmoCount > 0) {
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

  //  public override Action SubscribeBallsEnd(Action callback) {
  //   ballsEndCallbacks.Add(callback);
  //
  //   return () => ballsEndCallbacks.Remove(callback);
  // }
  // public override void UnsubscribeBallsEnd(Action callback) {
  //   ballsEndCallbacks.Remove(callback);
  // }
  // private void OnBallsEnd() {
  //   if (isDestroy) return;
  //
  //   ballsEndCallbacks.ForEach(callback => callback());
  // }
}
