using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LevelBackgroundController : AbstractBackgroundController {
  [SerializeField] private Sprite[] backgrounds;

  [SerializeField] private AbstractBackgroundRenderer backgroundRenderer;
  private AbstractBackgroundRenderer backgroundRendererInstance;

  private void Awake() {
    backgroundRendererInstance = Instantiate(backgroundRenderer);
  }

  // private readonly int initialBackgroundIndex = 0;
  // private void Start() {
  //   SetBackground(initialBackgroundIndex);
  // }

  override public void SetBackground(int index) {
    backgroundRendererInstance.SetBackgroundSprite(backgrounds[index]);
  }
}
