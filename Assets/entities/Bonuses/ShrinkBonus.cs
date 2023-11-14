
using System;
using UnityEngine;

public class ShrinkBonus: BonusBaseScript {
  protected new Color color = Color.red;
  protected new Color textColor = Color.white;
  protected new String text = BonusLabels.ShrinkBonusLabel;

  protected override void initializeBonus() {
    base.color = color;
    base.text = text;
    base.textColor = textColor;
    base.initializeBonus();
  }

  protected override void BonusActivate() {
    base.BonusActivate();

    if (playerObject.transform.localScale.x / 4f > (4f/9f)) {
      playerObject.transform.localScale = new Vector3(
        playerObject.transform.localScale.x / 1.5f,
        playerObject.transform.localScale.y,
        playerObject.transform.localScale.z);
    }
  }
}
