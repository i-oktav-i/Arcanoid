using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject {

  [Range(1, LevelsConfig.MaxLevel)]
  public int level = InitialGameState.Level;

  public int balls = InitialGameState.BallsCnt;

  public int points = InitialGameState.Points;

  public bool resetOnStart;

  public void Reset()
  {
    level = InitialGameState.Level;
    balls = InitialGameState.BallsCnt;
    points = InitialGameState.Points;
  }

  private List<Action> levelChangeCallbacks = new();

  public Action SubscribeLevelChange(Action callback) {
    levelChangeCallbacks.Add(callback);
    return () => levelChangeCallbacks.Remove(callback);
  }

  public void UnsubscribeLevelChange(Action callback) {
    levelChangeCallbacks.Remove(callback);
  }
}
