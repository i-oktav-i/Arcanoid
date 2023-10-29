using System;
using UnityEngine;

public abstract class AbstractBoardController : MonoBehaviour {
  abstract public void InitBoard(Vector2 position, int level);

  abstract public Action SubscribeLevelComplete(Action callback);
  abstract public void UnsubscribeLevelComplete(Action callback);

  abstract public Action SubscribeLevelLose(Action callback);
  abstract public void UnsubscribeLevelLose(Action callback);
}
