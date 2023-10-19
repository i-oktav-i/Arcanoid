using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public partial class BoardController {

  class Config {
    public List<Vector2> redBlocksPositions;
    public List<Vector2> blueBlocksPositions;
    public List<Vector2> greenBlocksPositions;
    public List<Vector2> yellowBlocksPositions;

    public Config(List<Vector2> redBlocksPositions, List<Vector2> blueBlocksPositions = null, List<Vector2> greenBlocksPositions = null, List<Vector2> yellowBlocksPositions = null) {
      this.redBlocksPositions = redBlocksPositions;
      this.blueBlocksPositions = blueBlocksPositions;
      this.greenBlocksPositions = greenBlocksPositions;
      this.yellowBlocksPositions = yellowBlocksPositions;
    }
  }
  private class BoardConfig {
    static public Config GetConfig() {
      List<Vector2> redPos = Enumerable
        .Range(0, 10)
        .Select(item => new Vector2(2 + (item - 5) * 2.5f, 0))
        .ToList();

      Vector2 shift = new(0, 1.5f);

      List<Vector2> bluePos = redPos.Select(vector => vector + shift).ToList();
      List<Vector2> yellowPos = bluePos.Select(vector => vector + shift).ToList();
      List<Vector2> greenPos = yellowPos.Select(vector => vector + shift).ToList();

      return new(redPos, bluePos, greenPos, yellowPos);
    }
  }
}
