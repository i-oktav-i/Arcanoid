using UnityEngine;

public class Ball : AbstractBall {
  private new Rigidbody2D rigidbody;

  private void Awake() {
    rigidbody = GetComponent<Rigidbody2D>();

    rigidbody.AddForce(new(0, 600));
  }

  private void OnCollisionEnter2D(Collision2D other) {
    AbstractBlock wall = other.gameObject.GetComponent<AbstractBlock>();

    if (!wall) return;

    wall.DealDamage();
  }
}
