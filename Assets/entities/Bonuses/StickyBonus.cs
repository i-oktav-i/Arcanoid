
using System;
using UnityEngine;

public class StickyBonus: BonusBaseScript {
  protected new Color color = new Color(0f, 0.4f, 0f);
  protected new Color textColor = Color.white;
  protected new String text = BonusLabels.StickyBonusLabel;

  protected override void initializeBonus() {
    base.color = color;
    base.text = text;
    base.textColor = textColor;
    base.initializeBonus();
  }

  protected override void BonusActivate() {
    base.BonusActivate();
    StickyBehaviour nall;
    if (!playerObject.TryGetComponent<StickyBehaviour>(out nall)) {
      playerObject.AddComponent(typeof(StickyBehaviour));
    }

  }
}
