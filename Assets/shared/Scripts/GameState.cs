using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameState : ScriptableObject {

  // left these as public so theyre shown in the inspector
  [Range(1, LevelsConfig.MaxLevel)]
  public int level = InitialGameState.Level;

  public int balls = InitialGameState.BallsCapacity;

  public int points = InitialGameState.Points;

  public int pointsToBall = 0;

  public bool resetOnStart;
  public bool music = true;
  public bool sound = true;

  public float musicVolume = 1f;
  [FormerlySerializedAs("soundVolume")] public float sfxVolume = 10f;

  // TODO: separate audio source for sfx
  private const float fakeZero = 0.005f;
  public float MusicVolume {
    get => Math.Max(musicVolume, fakeZero);
    set {
      musicVolume = value;
    }
  }

  public float SfxVolume {
    get => sfxVolume * SoundConfig.SFX_VOLUME_MULTIPLIER;
  }

  public int requiredPointsToBall {
    get  => 400 + (level - 1) * 20;
  }
  public void Reset() {
    level = InitialGameState.Level;
    balls = InitialGameState.BallsCapacity;
    points = InitialGameState.Points;
    pointsToBall = InitialGameState.PointsToBall;
  }

  public void Save() {
    PlayerPrefs.SetInt("level", level);
    PlayerPrefs.SetInt("balls", balls);
    PlayerPrefs.SetInt("points", points);
    PlayerPrefs.SetInt("pointsToBall", pointsToBall);
    PlayerPrefs.SetInt("music", music ? 1 : 0);
    PlayerPrefs.SetInt("sound", sound ? 1 : 0);
    PlayerPrefs.SetFloat("musicVolume", musicVolume);
    PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
  }

  public void Load() {
    level = PlayerPrefs.GetInt("level", 1);
    balls = PlayerPrefs.GetInt("balls", 6);
    points = PlayerPrefs.GetInt("points", 0);
    pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
    music = PlayerPrefs.GetInt("music", 1) == 1;
    sound = PlayerPrefs.GetInt("sound", 1) == 1;
    musicVolume = PlayerPrefs.GetFloat("musicVolume");
    sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
  }

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
