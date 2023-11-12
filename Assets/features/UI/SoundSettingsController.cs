
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
    musicSlider.value = gameData.musicVolume;
    sfxSlider.value = gameData.sfxVolume;
  }

  private void Start() {
    audioSrc = Camera.main.GetComponent<AudioSource>();
  }

  public void OnChangeMusicVolume() {
    audioSrc.volume = musicSlider.value;
    gameData.musicVolume = musicSlider.value;
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
