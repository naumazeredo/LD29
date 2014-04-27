using UnityEngine;
using System.Collections;

public class Timer
{
  float time = 0f;
  float maxTime = 0f;

  public void SetMaximumTime(float newMaxTime)
  {
    maxTime = newMaxTime;
    Reset();
  }

  public void SetRandomMaximumTime(float min, float max)
  {
    maxTime = Random.Range(min, max);
    Reset();
  }

  public bool PassedMaximum()
  {
    time += Time.deltaTime;
    if (time >= maxTime)
      return true;
    return false;
  }

  public void Reset()
  {
    time = 0f;
  }
}
