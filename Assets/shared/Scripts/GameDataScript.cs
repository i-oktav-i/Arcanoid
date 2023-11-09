using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject {
  public int level = 1;
  public int balls = 6;
  public int points = 0;

  public readonly int pointsPerBlockDestruction = 1;
}
