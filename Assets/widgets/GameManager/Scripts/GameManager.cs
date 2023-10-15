using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  [SerializeField] private IBackgroundController backgroundController;

  private void Awake() {
    Instantiate(backgroundController);
  }
}
