using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : AbstractBall {
  private new Rigidbody2D rigidbody;
  private List<Action> destroyCallbacks = new();

  public override void Launch(Vector2 force) {
    if (rigidbody.bodyType == RigidbodyType2D.Dynamic) return;

    rigidbody.bodyType = RigidbodyType2D.Dynamic;
    rigidbody.AddForce(force);
  }

  private void Awake() {
    rigidbody = GetComponent<Rigidbody2D>();
  }

  private void OnCollisionEnter2D(Collision2D other) {
    AbstractBlock wall = other.gameObject.GetComponent<AbstractBlock>();

    if (!wall) return;

    wall.DealDamage();
  }

  override public Action SubscribeDestroy(Action callback) {
    destroyCallbacks.Add(callback);

    return () => destroyCallbacks.Remove(callback);
  }
  override public void UnsubscribeDestroy(Action callback) {
    destroyCallbacks.Remove(callback);
  }

  private void OnDestroy() {
    destroyCallbacks.ForEach(item => item());
  }
}
