using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  [SerializeField] private AbstractBackgroundController backgroundController;
  [SerializeField] private AbstractBoardController boardController;

  private void Start() {
    Instantiate(backgroundController);
    Instantiate(boardController).InitBoard((400, 300), new(0, 0));
  }
}
