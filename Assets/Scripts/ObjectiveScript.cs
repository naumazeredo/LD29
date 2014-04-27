using UnityEngine;
using System.Collections;

public class ObjectiveScript : MonoBehaviour
{
  public static ObjectiveScript instance { get; private set; }

  void OnEnable()
  {
    instance = this;
  }

  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.tag == "Player")
      Debug.Log("WIN!");
  }
}
