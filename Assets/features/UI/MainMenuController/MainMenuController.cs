using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : SoundSettingsController
{
  public Button exitButton; // no exit button in initial game menu is bewildering.
  public Button playButton;

  public void OnClickPlay() {
     GameObject.Find("MainMenuCanvas").SetActive(false);
  }

  public void OnClickExit() {
    Application.Quit();
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
  }


}
