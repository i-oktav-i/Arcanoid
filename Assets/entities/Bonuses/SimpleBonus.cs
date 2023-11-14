
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

    StickyBehaviour stickBeh;

    if (playerObject.TryGetComponent<StickyBehaviour>(out stickBeh)) {
      stickBeh.ReleaseBall();
      Destroy(stickBeh);
    }
  }
}
