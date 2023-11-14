
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
  enum ActiveLayout {
    MainMenu,
    Board,
    PauseMenu
  }
  private ActiveLayout activeLayout;

  public GameObject pauseCanvas;
  private PauseMenuController pauseMenuController;

  public GameObject mainCanvas;
  private MainMenuController mainMenuController;

  public GameState gameData;

  public bool IsMenuActive {
    get => activeLayout == ActiveLayout.MainMenu || activeLayout == ActiveLayout.PauseMenu;
  }

  void Start() {
    mainMenuController = mainCanvas.GetComponentInChildren<MainMenuController>();
    pauseMenuController = pauseCanvas.GetComponentInChildren<PauseMenuController>();
    mainMenuController.SetSoundSettings();
    Cursor.visible = true;
    mainMenuController.exitButton.gameObject.SetActive(false);
    pauseCanvas.SetActive(false);
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.M)) gameData.IsMusicOn = !gameData.IsMusicOn;
    if (Input.GetKeyDown(KeyCode.S)) gameData.IsSoundOn = !gameData.IsSoundOn;
    if (Input.GetButtonDown("Pause")) {
      if (activeLayout == ActiveLayout.PauseMenu) HidePauseMenu();
      if (activeLayout == ActiveLayout.Board) ShowPauseMenu();
    }
    if (Input.GetKeyDown(KeyCode.Escape)) OnClickExit();
  }

  public void OnClickExit() {
    Application.Quit();
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
  }

  public void ShowMainMenu(bool isEnd = false) {
    activeLayout = ActiveLayout.MainMenu;
    Cursor.visible = true;
    Time.timeScale = 0f;
    mainCanvas.SetActive(true);
    mainMenuController.SetSoundSettings();
    string playLabel = isEnd ? "NEW GAME" : "PLAY";
    mainMenuController.playButton.GetComponentInChildren<Text>().text = playLabel;
    mainMenuController.playButton.onClick.RemoveAllListeners();
    mainMenuController.playButton.onClick.AddListener(() => {
      if (isEnd) Camera.main.GetComponent<GameManager>().StartNewGame();
      else HideMainMenu();
    });
    mainMenuController.exitButton.gameObject.SetActive(isEnd);
  }

  public void HideMainMenu() {
    Cursor.visible = false;
    activeLayout = ActiveLayout.Board;
    mainCanvas.SetActive(false);
    Time.timeScale = 1f;
  }

  public void ShowPauseMenu() {
    activeLayout = ActiveLayout.PauseMenu;
    Time.timeScale = 0f;
    pauseCanvas.SetActive(true);
    Cursor.visible = true;
    pauseMenuController.SetSoundSettings();
  }

  public void HidePauseMenu() {
    activeLayout = ActiveLayout.Board;
    pauseCanvas.SetActive(false);
    Cursor.visible = false;
    Time.timeScale = 1f;
  }

  string OnOff(bool boolVal) => boolVal ? "on" : "off";

  void OnGUI() {
    if (IsMenuActive) return;
    GUI.Label(
      new Rect(5, 4, Screen.width - 10,
        100),
    string.Format(
          "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>"+
          " Score <b>{2}</b></size></color>",
          gameData.level, gameData.BallsCapacity, gameData.points
          )
    );
    GUIStyle style = new GUIStyle();
    style.alignment = TextAnchor.UpperRight;
    GUI.Label(new Rect(5, 14, Screen.width - 10, 100),
    string.Format("<color=yellow><size=20><color=white>Space</color>-pause {0}" +
    " <color=white>N</color>-new" +
    " <color=white>J</color>-jump" +
    " <color=white>M</color>-music {1}" +
    " <color=white>S</color>-sound {2}" +
    " <color=white>Esc</color>-exit</size></color>",
    OnOff(Time.timeScale > 0), OnOff(!gameData.music),
    OnOff(!gameData.sound)), style);
  }
}
