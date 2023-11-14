
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StickyBehaviour: MonoBehaviour {
  private GameObject stickedBall = null;
  private Transform oldParent;
  private Rigidbody2D stickedRigidbody;
  public void OnCollisionEnter2D(Collision2D col) {
    if (stickedBall != null) {
      return;
    }

    if (col.gameObject.layer == LayerMask.NameToLayer("Balls")) {
      stickedBall = col.gameObject;
      stickedRigidbody = stickedBall.GetComponent<Rigidbody2D>();
      stickedRigidbody.bodyType = RigidbodyType2D.Static;
      oldParent = stickedBall.transform.parent;
      stickedBall.transform.parent = gameObject.transform;
    }
  }

  private void Update() {
    if (Input.GetButtonDown("Fire1"))
    {
      ReleaseBall();
    }

    if (stickedBall.IsDestroyed()) {
      stickedBall = null;
    }
  }

  public void ReleaseBall() {
    if (stickedBall == null) {
      return;
    }

    stickedBall.transform.SetParent(oldParent);
    stickedRigidbody.bodyType = RigidbodyType2D.Dynamic;
    stickedRigidbody.totalForce = calculateDirectionToPlayer();
    stickedBall = null;
  }

  Vector2 calculateDirectionToPlayer() {
    Vector2 dir = (stickedBall.transform.position - transform.position);
    return dir.normalized * (600f * Mathf.Sqrt(2));
  }
}
