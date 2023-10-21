using UnityEngine;

public class Player : AbstractPlayer {
  void Update() {
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    transform.position = new(mousePosition.x, transform.position.y);
  }
}
