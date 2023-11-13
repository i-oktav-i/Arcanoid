using System;
using System.Collections.Generic;
using System.Linq;
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
  public UIManager uiManager;

  private bool isDestroy = false;
  private void OnDestroy() => isDestroy = true;

  private int AmmoCount {
    get => ballsMag.Count;
  }

  private int releasedBallsCnt = 0;

  private void Awake() {
    uiManager = Camera.main.GetComponent<UIManager>();
    player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
    player.transform.SetParent(transform);
    FillBallsMag();
  }

  public override void FillBallsMag() {
    if (isDestroy) return;
    var ballsToSpawn = Math.Min(InitialGameState.BallsMagCapacity, gameData.BallsCapacity);

    var newBalls = Enumerable.Range(0, ballsToSpawn)
      .Select((idx) => SpawnBall())
      .ToList();

    ballsMag.AddRange(newBalls);
  }

  private AbstractBall SpawnBall() {
    var newBall = Instantiate(ballPrefab, player.transform.position + new Vector3(1.1f, 1.1f), Quaternion.identity);
    newBall.transform.SetParent(player.transform);

    // TODO: move "no balls" case resolving to game manager
    newBall.SubscribeDestroy(() => {
      if (isDestroy) return;
      gameData.BallsCapacity--;
      releasedBallsCnt--;
      if (releasedBallsCnt <= 0) FillBallsMag();
    });

    return newBall;
  }

  private void Update() {
    if (uiManager.IsMenuActive) return;
    if (!blockInput && Input.GetButtonDown("Fire1") && AmmoCount > 0) {
      blockInput = true;

      // var ballsCount = ballsMag.Count;
      // for (int i = 0; i < ballsCount; i++) {
      //   ballsMag[i].transform.SetParent(transform);
      //   ballsMag[i].Launch(new(600, 600));
      //   releasedBallsCnt++;
      // }
      // ballsMag.Clear();

      var ball = ballsMag[0];
      ballsMag.RemoveAt(0);
      ball.transform.SetParent(transform);
      ball.Launch(new(600, 600));
      releasedBallsCnt++;
    }
    else {
      blockInput = false;
    }
  }
}
