using UnityEngine;

public abstract class AbstractBlock : MonoBehaviour {
  abstract public void SetSize((float horizontal, float vertical) size);
  abstract public void SetHitPoints(int points);
  abstract public void DealDamage();
}
