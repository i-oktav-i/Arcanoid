using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : AbstractBoardController {
  [SerializeField] private AbstractWall wall;
  private GameObject boardHolder;
  private readonly float wallWidth = 0.1f;

  public override void InitBoard((int width, int height) size, Vector2 position) {
    if (boardHolder) Destroy(boardHolder);

    boardHolder = new("Board");
    boardHolder.transform.position = position;

    InitBounds(boardHolder.transform);
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
