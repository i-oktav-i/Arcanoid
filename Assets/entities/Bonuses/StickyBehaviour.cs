
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;

public class StickyBehaviour: MonoBehaviour {
  private GameObject stickedBall = null;
  //private Transform oldParent;
  private Rigidbody2D stickedRigidbody;
  private ParentConstraint parentConstraint;
  ConstraintSource constraintSource;
  public void OnCollisionEnter2D(Collision2D col) {
    if (stickedBall != null) {
      return;
    }

    if (col.rigidbody.bodyType != RigidbodyType2D.Dynamic) {
      return;
    }

    if (col.gameObject.layer == LayerMask.NameToLayer("Balls")) {
      stickedBall = col.gameObject;
      stickedRigidbody = stickedBall.GetComponent<Rigidbody2D>();
      stickedBall.transform.Translate(-calculateDirectionToPlayer() / 2000);
      stickedRigidbody.bodyType = RigidbodyType2D.Static;
      Vector3 constraintOffset = stickedBall.transform.position - gameObject.transform.position;
      parentConstraint = stickedBall.AddComponent(typeof(ParentConstraint)) as ParentConstraint;
      constraintSource.sourceTransform = gameObject.transform;
      constraintSource.weight = 1;
      int srcInd = parentConstraint.AddSource(constraintSource);
      parentConstraint.weight = 1;

      parentConstraint.constraintActive = true;
      parentConstraint.locked = false;
      parentConstraint.SetTranslationOffset(srcInd, constraintOffset);
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

    //stickedBall.transform.SetParent(oldParent);
    stickedRigidbody.bodyType = RigidbodyType2D.Dynamic;
    stickedRigidbody.totalForce = calculateDirectionToPlayer();
    Destroy(parentConstraint);
    stickedBall = null;
  }

  Vector2 calculateDirectionToPlayer() {
    Vector2 dir = (stickedBall.transform.position - transform.position);
    return dir.normalized * (600f * Mathf.Sqrt(2));
  }
}
