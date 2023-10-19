using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoardController : AbstractBoardController {
  [SerializeField] private AbstractWall wall;
  [SerializeField] private AbstractBlock block;
  private GameObject boardHolder;
  private readonly float wallWidth = 0.1f;

  public override void InitBoard((int width, int height) size, Vector2 position) {
    if (boardHolder) Destroy(boardHolder);

    boardHolder = new("Board");
    boardHolder.transform.position = position;

    InitBounds(boardHolder.transform);
    InitBlocks(boardHolder.transform);
  }

  private void InitBlocks(Transform parent) {
    Config config = BoardConfig.GetConfig();

    Action<Vector2> getInit(int points) => (Vector2 vector) => {
      AbstractBlock blockInstance = Instantiate(block, vector, Quaternion.identity);
      blockInstance.transform.SetParent(parent);
      blockInstance.SetHitPoints(points);
    };

    config.blueBlocksPositions.ForEach(getInit(1));
    config.redBlocksPositions.ForEach(getInit(2));
    config.greenBlocksPositions.ForEach(getInit(3));
    config.yellowBlocksPositions.ForEach(getInit(4));
  }

  private void InitBounds(Transform parent) {
    Vector2 leftWallPos = new(-GameConfig.CameraWidthUnits - (wallWidth / 2) - Mathf.Epsilon, 0);
    AbstractWall leftWall = Instantiate(wall, leftWallPos, Quaternion.identity);
    leftWall.SetSize((wallWidth, GameConfig.CameraHeightUnits * 2));
    leftWall.transform.SetParent(parent);

    AbstractWall rightWall = Instantiate(wall, -leftWallPos, Quaternion.identity);
    rightWall.SetSize((wallWidth, GameConfig.CameraHeightUnits * 2));
    rightWall.transform.SetParent(parent);

    Vector2 topWallPos = new(0, -GameConfig.CameraHeightUnits - (wallWidth / 2) - Mathf.Epsilon);

    AbstractWall topWall = Instantiate(wall, -topWallPos, Quaternion.identity);
    topWall.SetSize(((GameConfig.CameraWidthUnits * 2) + 2 * wallWidth, wallWidth));
    topWall.transform.SetParent(parent);
  }
}
