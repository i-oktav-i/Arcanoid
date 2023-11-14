
using System;
using UnityEngine;

public class SimpleBonus: BonusBaseScript {
  protected new Color color = Color.green;
  protected new Color textColor = Color.white;
  protected new String text = BonusLabels.SimpleBonusLabel;

  protected override void initializeBonus() {
    base.color = color;
    base.text = text;
    base.textColor = textColor;
    base.initializeBonus();
  }

  protected override void BonusActivate() {
    base.BonusActivate();

    if (playerObject.transform.localScale.x < 1.5 * 1.5) {
      playerObject.transform.localScale.Scale(new Vector3(1.5f, 1f, 1f));
    }
  }
}
