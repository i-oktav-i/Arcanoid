using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LevelBackgroundController : IBackgroundController {
  [SerializeField] private Sprite[] backgrounds;

  [SerializeField] private IBackgroundRenderer backgroundRenderer;
  private IBackgroundRenderer backgroundRendererInstance;


  private readonly int initialBackgroundIndex = 0;

  private void Awake() {
    backgroundRendererInstance = Instantiate(backgroundRenderer);
  }

  private void Start() {
    SetBackground(initialBackgroundIndex);
  }

  override public void SetBackground(int index) {
    backgroundRendererInstance.SetBackgroundSprite(backgrounds[index]);
  }
}
