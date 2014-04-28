using UnityEngine;
using System.Collections;

public class ObjectiveScript : MonoBehaviour
{
  public static ObjectiveScript instance { get; private set; }

  public delegate void OnHitObjectiveFunction();
  public OnHitObjectiveFunction OnHitObjective;

  void OnEnable()
  {
    instance = this;
  }

  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.tag == "Player")
      if (OnHitObjective != null)
        OnHitObjective();
  }
}
