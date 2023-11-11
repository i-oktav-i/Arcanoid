using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class BoardController : AbstractBoardController {
  [SerializeField] private AbstractWall wall;
  [SerializeField] private AbstractBlock block;
  [SerializeField] private AbstractPlayerController playerController;
  [SerializeField] private AbstractBallsDestroyer ballsDestroyer;

  private GameObject boardHolder;
  private readonly float wallWidth = 0.1f;

  private readonly int rows = 6;
  private readonly int columns = 10;

  private readonly int blocksCount = 30;
  private readonly int chanceCoefficient = 15;

  private List<Action> blocksEndCallbacks = new();

  private bool isDestroy = false;

  public GameState gameData;


  private int currentBlocksCount = 0;
  public int CurrentBlocksCount {
    get => currentBlocksCount;
    set {
      currentBlocksCount = value;
      if (value == 0) RunCallbacks(blocksEndCallbacks);
    }
  }

  private void SetMusic() {
    if (gameData.music)
      audioSrc.Play();
    else
      audioSrc.Stop();
  }

  AudioSource audioSrc;
  public AudioClip pointSound;
  private void Start() {
    audioSrc = Camera.main.GetComponent<AudioSource>();
    SetMusic();
  }

  public override void InitBoard(Vector2 position, int level) {
    if (boardHolder) Destroy(boardHolder);

    boardHolder = new("Board");
    boardHolder.transform.position = position;

    InitBounds(boardHolder.transform);
    InitBlocks(boardHolder.transform, level);
    InitPlayer(boardHolder.transform);
  }

  // TODO: move to player input controller all such pieces maybe
  private void Update() {
    if (Input.GetKeyDown(KeyCode.M)) {
      gameData.music = !gameData.music;
      SetMusic();
    }
    if (Input.GetKeyDown(KeyCode.S)) gameData.sound = !gameData.sound;
  }

  private void InitBlocks(Transform parent, int level) {
    Random.InitState(level);
    List<Vector2> boardPositions = GetBoardPositions();
    List<int> blocksHits = GetBlocksHits(level, 4);

    currentBlocksCount = blocksCount;

    Enumerable
      .Range(0, blocksCount)
      .ToList()
      .ForEach(index => {
        int hits = blocksHits[index];
        Vector2 position = boardPositions[index];

        AbstractBlock blockInstance = Instantiate(block, position, Quaternion.identity);
        blockInstance.transform.SetParent(parent);
        blockInstance.SetHitPoints(hits);
        blockInstance.SubscribeDestroy(() => {
          gameData.points += InitialGameState.PointsPerBlockDestruction;
          CurrentBlocksCount -= 1;
          if (gameData.sound) audioSrc.PlayOneShot(pointSound);
        });
      });
  }

  private void InitBounds(Transform parent) {
    Vector2 leftWallPos = new(-GameConfig.CameraWidthUnits - (wallWidth / 2) - Mathf.Epsilon, 0);
    AbstractWall leftWall = Instantiate(wall, leftWallPos, Quaternion.identity);
    leftWall.SetSize((wallWidth, GameConfig.CameraHeightUnits * 2));
    leftWall.transform.SetParent(parent);

    AbstractWall rightWall = Instantiate(wall, -leftWallPos, Quaternion.identity);
    rightWall.SetSize((wallWidth, GameConfig.CameraHeightUnits * 2));
    rightWall.transform.SetParent(parent);

    Vector2 topWallPos = new(0, GameConfig.CameraHeightUnits + (wallWidth / 2) + Mathf.Epsilon);

    AbstractWall topWall = Instantiate(wall, topWallPos, Quaternion.identity);
    topWall.SetSize(((GameConfig.CameraWidthUnits * 2) + 2 * wallWidth, wallWidth));
    topWall.transform.SetParent(parent);

    AbstractBallsDestroyer bottomWall = Instantiate(ballsDestroyer, -topWallPos - new Vector2(0, 1), Quaternion.identity);
    bottomWall.SetSize(((GameConfig.CameraWidthUnits * 2) + 2 * wallWidth + 2, 1));
    bottomWall.transform.SetParent(parent);
  }

  private void InitPlayer(Transform parent) {
    AbstractPlayerController playerControllerInstance = Instantiate(playerController, new(-0.3f, -10), Quaternion.identity);
    playerControllerInstance.transform.SetParent(parent);
    // playerControllerInstance.SubscribeBallsEnd(() => RunCallbacks(levelLoseCallbacks));
  }


  private int GetBLockHits(int level = 0, int maxHits = 0, int currentHits = 0) {
    if (currentHits >= maxHits) return maxHits;

    return GetIsUpperThreshold(level) ? GetBLockHits(level, maxHits, currentHits + 1) : currentHits;
  }

  private bool GetIsUpperThreshold(int level) {
    int randomNumber = Random.Range(0, chanceCoefficient + level);

    return randomNumber >= chanceCoefficient;
  }

  private List<Vector2> GetBoardPositions() {
    Vector2 shift = new(0, 1.5f);

    return RandomSort(
      Enumerable
        .Range(0, columns)
        .Select(item => new Vector2(2 + (item - 5) * 2.5f, 0))
        .SelectMany(vector => Enumerable
          .Range(0, rows)
          .Select(shiftScaler => vector + shiftScaler * shift)
        )
        .ToList()
    );
  }

  private List<int> GetBlocksHits(int level, int maxHits) {
    return Enumerable
      .Range(0, blocksCount)
      .Select(index => GetBLockHits(level, maxHits, 1))
      .ToList();
  }

  private static List<T> RandomSort<T>(List<T> list) {
    List<T> newList = new(list);
    newList.Sort(delegate (T l, T r) { return Random.Range(0f, 1f) > 0.5 ? 1 : -1; });

    return newList;
  }

  public override Action SubscribeBlocksEnd(Action callback) {
    blocksEndCallbacks.Add(callback);
    return () => blocksEndCallbacks.Remove(callback);
  }

  public override void UnsubscribeBlocksEnd(Action callback) {
    blocksEndCallbacks.Remove(callback);
  }

  // public override Action SubscribeLevelLose(Action callback) {
  //   levelLoseCallbacks.Add(callback);
  //
  //   return () => levelLoseCallbacks.Remove(callback);
  // }
  //
  // public override void UnsubscribeLevelLose(Action callback) {
  //   levelLoseCallbacks.Remove(callback);
  // }

  private void RunCallbacks(List<Action> list) {
    if (isDestroy) return;

    list.ForEach(callback => callback());
  }

  private void OnDestroy() {
    Destroy(boardHolder);
  }
}
