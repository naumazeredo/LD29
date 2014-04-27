using UnityEngine;
using System.Collections;

public static class Utils
{
  public static Vector3 RotateTo(float x, float y)
  {
    return new Vector3(0f, 0f, Mathf.Atan2(-x, y) * Mathf.Rad2Deg);
  }

  public static Vector3 RotateTo(Vector2 vec)
  {
    return RotateTo(vec.x, vec.y);
  }

  public static Vector2 V3toV2(Vector3 vec)
  {
    return new Vector2(vec.x, vec.y);
  }
}
