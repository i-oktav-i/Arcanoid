using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Hardware;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockController : AbstractBlock {
  private new Rigidbody2D rigidbody;
  private TextMeshProUGUI hpText;
  private bool isMoving = false;
  private Vector2 moveDirection = Vector2.right;
  [SerializeField] private Sprite[] sprites;
  public GameObject bonus;
  private SpriteRenderer spritRenderer;

  AudioSource audioSrc;
  public GameState gameData;

  private List<Action> destroyCallbacks = new();

  [SerializeField]
  public int HitPoints {
    get => hitPoints;
    set {
      hitPoints = value;

      if (value == 0) {
        destroyCallbacks.ForEach(item => item());
        Destroy(gameObject);
        return;
      }

      hpText.text = value.ToString();
    }
  }

  private void Awake() {
    spritRenderer = GetComponent<SpriteRenderer>();
    spritRenderer.sprite = sprites[0];
    hpText = GetComponentInChildren<TextMeshProUGUI>();
  }

  private void Start() {
    audioSrc = Camera.main.GetComponent<AudioSource>();
  }

  public override void DealDamage() {
    if (HitPoints == 1 && UnityEngine.Random.Range(0, 100) <= 100)
      SpawnBonus(this.transform.position);
    HitPoints -= 1;
  }

  public override void SetHitPoints(int points) {
    HitPoints = points;
  }

  public override void SetSize((float horizontal, float vertical) size) {
    transform.localScale = new(size.horizontal, size.vertical);
  }

  public override Action SubscribeDestroy(Action callback) {
    destroyCallbacks.Add(callback);

    return () => destroyCallbacks.Remove(callback);
  }

  public override void UnsubscribeDestroy(Action callback) {
    destroyCallbacks.Remove(callback);
  }

  public override void PlayOnDestroySound() {
    if (!audioSrc) return;
    audioSrc.PlayOneShot(SoundOnDestroy, gameData.SfxVolume);
  }
  public override void SetBlockType(int type) {
    spritRenderer.sprite = sprites[type % sprites.Length];
    SetHitPoints(GameConfig.blockHPs[type]);
    points = GameConfig.blockPoints[type];
    switch (type) {
      case 1:
        break;
      case 2:
        break;
      case 3:
        float movingRoll = UnityEngine.Random.Range(0f, 1f);
        if (movingRoll < GameConfig.yellowMovingProbability) {
          isMoving = true;
          moveDirection = UnityEngine.Random.value < 0.5f ? -moveDirection : moveDirection;
        }
        break;
      case 4:
        break;
    }
  }

  public void FixedUpdate() {
    if (isMoving) {
      transform.Translate(GameConfig.yellowMoveSpeed * Time.deltaTime * moveDirection);
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    moveDirection = -moveDirection;
  }

  public void SpawnBonus(Vector2 position) {
    Debug.Log("AAAAAAAAAAAAAAAAAAA");
    var bonusadd = Instantiate(prefabmanger.instance.bonus, position, Quaternion.identity);
    switch (Random.Range(0, 5)) {
      case 0:
        bonusadd.AddComponent(typeof(BonusBaseScript));
        break;
      case 1:
        bonusadd.AddComponent(typeof(SimpleBonus));
        break;
      case 2:
        bonusadd.AddComponent(typeof(ExpandBonus));
        break;
      case 3:
        bonusadd.AddComponent(typeof(ShrinkBonus));
        break;
      case 4:
        bonusadd.AddComponent(typeof(StickyBonus));
        break;
    }

  }
}
