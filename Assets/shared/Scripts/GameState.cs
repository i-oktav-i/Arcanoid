using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameState : ScriptableObject {

  // left these as public so theyre shown in the inspector
  [Range(1, LevelsConfig.MaxLevel)]
  public int level = InitialGameState.Level;

  public int balls = InitialGameState.BallsCapacity;

  public int points = InitialGameState.Points;

  public bool resetOnStart;

  public bool music = true;
  public bool sound = true;

  public void Reset()
  {
    level = InitialGameState.Level;
    balls = InitialGameState.BallsCapacity;
    points = InitialGameState.Points;
  }

  public int BallsCapacity {
    get => balls;
    set {
      balls = value;
      if (value > 0) return;
      OnBallsEnd();
    }
  }

  private List<Action> ballsEndCallbacks = new();
  public Action SubscribeBallsEnd(Action callback) {
    ballsEndCallbacks.Add(callback);
    return () => ballsEndCallbacks.Remove(callback);
  }
  public void UnsubscribeBallsEnd(Action callback) {
    ballsEndCallbacks.Remove(callback);
  }
  private void OnBallsEnd() {
    ballsEndCallbacks.ForEach(callback => callback());
  }
}
