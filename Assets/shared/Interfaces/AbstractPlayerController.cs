using System;
using UnityEngine;

public abstract class AbstractPlayerController : MonoBehaviour {
  public abstract void AddBalls(int count);

  abstract public Action SubscribeBallsEnd(Action callback);
  abstract public void UnsubscribeBallsEnd(Action callback);
}
