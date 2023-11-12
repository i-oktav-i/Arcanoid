using System;
using UnityEngine;

public abstract class AbstractBlock : MonoBehaviour {
  abstract public void SetSize((float horizontal, float vertical) size);
  abstract public void SetHitPoints(int points);
  abstract public void SetBlockType(int type);
  abstract public void DealDamage();

  abstract public Action SubscribeDestroy(Action callback);
  abstract public void UnsubscribeDestroy(Action callback);
}
