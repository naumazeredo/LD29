using UnityEngine;
using System.Collections;

public class HUDArrowScript : MonoBehaviour
{
  // Objective
  ObjectiveScript objective;

  void Update()
  {
    if (objective == null)
      objective = ObjectiveScript.instance;
    transform.localEulerAngles = Utils.RotateTo(objective.transform.position - transform.position);
  }
}
