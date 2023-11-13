
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsController: MonoBehaviour {
  public Slider musicSlider;
  public Slider sfxSlider;
  public Toggle musicToggle;
  public Toggle sfxToggle;

  public GameState gameData;
  private AudioSource audioSrc;

  public void SetSoundSettings() {
    musicToggle.isOn = gameData.IsMusicOn;
    sfxToggle.isOn = gameData.IsSoundOn;
    musicSlider.value = gameData.MusicVolume;
    sfxSlider.value = gameData.sfxVolume;
  }

  private void Start() {
    audioSrc = Camera.main.GetComponent<AudioSource>();
  }

  public void OnChangeMusicVolume() {
    gameData.MusicVolume = musicSlider.value;
    audioSrc.volume = gameData.MusicVolume;
  }

  public void OnChangeSoundVolume() {
    gameData.sfxVolume = sfxSlider.value;
  }

  public void OnToggleMusic() {
    gameData.IsMusicOn = musicToggle.isOn;
    musicSlider.interactable = musicToggle.isOn;
  }

  public void OnToggleSfx() {
    gameData.IsSoundOn = sfxToggle.isOn;
    sfxSlider.interactable = sfxToggle.isOn;
  }
}
