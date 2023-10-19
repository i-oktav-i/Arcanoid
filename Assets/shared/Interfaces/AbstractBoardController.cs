using UnityEngine;

public abstract class AbstractBoardController : MonoBehaviour {
  abstract public void InitBoard((int width, int height) size, Vector2 position);
}
