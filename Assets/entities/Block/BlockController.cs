using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : AbstractBlock {
  private int hitPoints = 1;
  [SerializeField] private Sprite[] sprites;
  private SpriteRenderer spritRenderer;

  private List<Action> destroyCallbacks = new();

  [SerializeField]
  public int HitPoints {
    get => hitPoints;
    set {
      hitPoints = value;

      if (value != 0) {
        spritRenderer.sprite = sprites[value % sprites.Length];
      }
      else {
        destroyCallbacks.ForEach(item => item());
        Destroy(gameObject);
      }
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

  override public Action SubscribeDestroy(Action callback) {
    destroyCallbacks.Add(callback);

    return () => destroyCallbacks.Remove(callback);
  }
  override public void UnsubscribeDestroy(Action callback) {
    destroyCallbacks.Remove(callback);
  }
}
