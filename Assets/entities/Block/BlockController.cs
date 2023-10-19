using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : AbstractBlock {
  private int hitPoints = 1;
  [SerializeField] private Sprite[] sprites;
  private SpriteRenderer spritRenderer;

  [SerializeField]
  public int HitPoints {
    get => hitPoints; set {
      hitPoints = value;
      spritRenderer.sprite = sprites[value % sprites.Length];
    }
  }

  private void Awake() {
    spritRenderer = GetComponent<SpriteRenderer>();
    spritRenderer.sprite = sprites[0];
  }

  public override void DealDamage() {
    HitPoints -= 1;
  }

  public override void SetHitPoints(int points) {
    HitPoints = points;
  }

  public override void SetSize((float horizontal, float vertical) size) {
    transform.localScale = new(size.horizontal, size.vertical);
  }
}
