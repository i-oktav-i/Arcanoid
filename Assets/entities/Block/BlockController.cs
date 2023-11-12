using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : AbstractBlock {
  [SerializeField] private Sprite[] sprites;
  private SpriteRenderer spritRenderer;
  public GameObject bonus;

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
    if (HitPoints == 1) { SpawnBonus(this.transform.position); }
    HitPoints -= 1;
  }

  public override void SetHitPoints(int points) {
    HitPoints = points;
  }

  public override void SetSize((float horizontal, float vertical) size) {
    transform.localScale = new(size.horizontal, size.vertical);
  }

  public override Action SubscribeDestroy(Action callback) {
    destroyCallbacks.Add(callback);

    return () => destroyCallbacks.Remove(callback);
  }
  public override void UnsubscribeDestroy(Action callback) {
    destroyCallbacks.Remove(callback);
  }
  public void SpawnBonus(Vector2 position) {
    if(Random.RandomRange(0,100)<=25){ var bonusadd = Instantiate(prefabmanger.instance.bonus, position, Quaternion.identity);
    bonusadd.AddComponent(typeof(BonusBaseScript));}
  }
}
