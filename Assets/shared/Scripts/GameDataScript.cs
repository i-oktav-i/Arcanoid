using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject {

  public int level = InitialGameState.level;
  public int balls = InitialGameState.ballsCnt;
  public int points = InitialGameState.points;

  public bool resetOnStart;

  public void Reset()
  {
    level = InitialGameState.level;
    balls = InitialGameState.ballsCnt;
    points = InitialGameState.points;
  }
}
