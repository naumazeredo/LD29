using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
public class CameraScript : MonoBehaviour
{
  public PlayerScript player;

  void Update()
  {
    if (player != null)
    {
      Vector3 pos = player.transform.position;
      pos.z = transform.position.z;
      transform.position = pos;
    }
  }
}
