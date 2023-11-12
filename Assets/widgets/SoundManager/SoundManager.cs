
using UnityEngine;

public class SoundManager : MonoBehaviour {
  AudioSource audioSrc;

  public GameState gameData;

  private void SetMusic() {
    Debug.Log("setting up music.");
    if (gameData.IsMusicOn) {
       audioSrc.Play();
      // audioSrc.volume = gameData.musicVolume;
    } else {
      // audioSrc.volume = 0f;
      audioSrc.Stop();
    }
  }

  private void Start() {
    audioSrc = Camera.main.GetComponent<AudioSource>();
    SetMusic();
    gameData.SubscribeMusicSwitch(SetMusic);
  }
}
