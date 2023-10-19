using UnityEngine;

static class GameConfig {
  public static readonly float CameraHeightUnits = 10;

  /// <todo>
  /// public static float ViewportAspectRatio {
  ///   get {
  ///     if (ratioBuffer == -1 || Camera.current == prevCamera) {
  ///       prevCamera = Camera.current;
  ///       Rect rect = prevCamera.pixelRect;
  ///       float ratio = rect.width / rect.height;
  ///       ratioBuffer = ratio;
  ///     }
  ///     return ratioBuffer;
  ///   }
  /// }
  /// </todo>
  public static float ViewportAspectRatio = 1920f / 1080f;

  public static float CameraWidthUnits => ViewportAspectRatio * CameraHeightUnits;
}
