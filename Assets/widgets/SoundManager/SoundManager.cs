
using UnityEngine;

public class SoundManager {
  AudioSource audioSrc;

  public GameState gameData;

  private void SetMusic() {
    if (gameData.IsMusicOn) audioSrc.Play();
    else audioSrc.Stop();
  }

  private void Start() {
    audioSrc = Camera.main.GetComponent<AudioSource>();
    SetMusic();
    gameData.SubscribeMusicSwitch(SetMusic);
  }
}
