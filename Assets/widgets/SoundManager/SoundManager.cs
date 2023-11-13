
using UnityEngine;

public class SoundManager : MonoBehaviour {
  AudioSource audioSrc;

  public GameState gameData;

  private void SetMusic() {
    audioSrc.volume = gameData.MusicVolume;
    if (gameData.IsMusicOn) audioSrc.Play();
    else audioSrc.Stop();
  }

  private void Start() {
    audioSrc = Camera.main.GetComponent<AudioSource>();
    SetMusic();
    gameData.SubscribeMusicSwitch(SetMusic);
  }
}
