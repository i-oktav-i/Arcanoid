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

  public int pointsToBall = 0;

  public bool resetOnStart;
  public void Reset() {
    level = InitialGameState.Level;
    balls = InitialGameState.BallsCapacity;
    points = InitialGameState.Points;
    pointsToBall = InitialGameState.PointsToBall;
  }

  public int requiredPointsToBall {
    get  => 400 + (level - 1) * 20;
  }

  // TODO: better to make separate sound manager
  public bool music = true;
  public bool IsMusicOn {
    get => music;
    set {
      music = value;
      musicSwitchCallbacks.ForEach(callback => callback());
    }
  }
  private List<Action> musicSwitchCallbacks = new();
  public Action SubscribeMusicSwitch(Action callback) {
    musicSwitchCallbacks.Add(callback);
    return () => musicSwitchCallbacks.Remove(callback);
  }
  public void UnsubscribeMusicSwitch(Action callback) {
    musicSwitchCallbacks.Remove(callback);
  }

  public bool sound = true;
  public bool IsSoundOn {
    get => sound;
    set {
      sound = value;
      soundSwitchCallbacks.ForEach(callback => callback());
    }
  }
  private List<Action> soundSwitchCallbacks = new();
  public Action SubscribeSoundSwitch(Action callback) {
    soundSwitchCallbacks.Add(callback);
    return () => soundSwitchCallbacks.Remove(callback);
  }
  public void UnsubscribeSoundSwitch(Action callback) {
    soundSwitchCallbacks.Remove(callback);
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
