using System;
using UnityEngine;

public abstract class AbstractBoardController : MonoBehaviour {
  abstract public void InitBoard(Vector2 position, int level);

  abstract public Action SubscribeBlocksEnd(Action callback);
  abstract public void UnsubscribeBlocksEnd(Action callback);
}
