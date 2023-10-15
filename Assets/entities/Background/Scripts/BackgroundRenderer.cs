using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundRenderer : IBackgroundRenderer {

  private Image spriteRenderer;

  void Awake() {
    spriteRenderer = GetComponent<Image>();
  }

  override public void SetBackgroundSprite(Sprite newSprite) {
    if (spriteRenderer == null) return;

    spriteRenderer.sprite = newSprite;
  }
}
