using UnityEngine;

public class Ball : AbstractBall {
  private new Rigidbody2D rigidbody;

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
}
