
using System;
using UnityEngine;

public class ExpandBonus: BonusBaseScript {
  protected new Color color = Color.green;
  protected new Color textColor = Color.white;
  protected new String text = BonusLabels.ExpandBonusLabel;

  protected override void initializeBonus() {
    base.color = color;
    base.text = text;
    base.textColor = textColor;
    base.initializeBonus();
  }

  protected override void BonusActivate() {
    base.BonusActivate();

    if (playerObject.transform.localScale.x / 4f < 1.5 * 1.5) {
      playerObject.transform.localScale = new Vector3(
        1.5f * playerObject.transform.localScale.x,
        playerObject.transform.localScale.y,
        playerObject.transform.localScale.z);
    }
  }
}
