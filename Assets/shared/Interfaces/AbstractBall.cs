using System;
using UnityEngine;

public abstract class AbstractBall : MonoBehaviour {
  public AudioClip SoundOnLose;
  public AudioClip SoundOnHit;

  abstract public void Launch(Vector2 force);
  abstract public Action SubscribeDestroy(Action callback);
  abstract public void UnsubscribeDestroy(Action callback);
  abstract public void PlayOnHitSound();
  abstract public void PlayOnLoseSound();
}
