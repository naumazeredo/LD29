using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UnitScript))]
[RequireComponent (typeof(SelfPoolScript))]
public class PersonScript : MonoBehaviour
{
  PlayerScript player;
  UnitScript unit;

  // State
  enum PersonState
  {
    None,
    Idle,
    Walking,
    Running
  }

  PersonState state = PersonState.Idle,
    oldState = PersonState.None;

  // Timer
  Timer timer;

  // Runaway settings
  public float runRadius = 4f;
  public float runMinTime = 3f;
  public float runMaxTime = 4f;

  // Walking settings
  public float idleMaxTime = 0.5f;
  public float walkMaxTime = 1f;
  Vector2 walkDirection;

  // Audio
  public AudioClip screamAudio;

  void Start()
  {
    player = PlayerScript.instance;
    unit = transform.GetComponent<UnitScript>();
    timer = new Timer();
  }

  void Update()
  {
    if (HUDScript.endGame)
    {
      rigidbody2D.velocity = new Vector2();
      return;
    }

    Vector2 deltaPos = Utils.V3toV2(player.transform.position - transform.position);

    // Check if the person wants to run away
    if (deltaPos.sqrMagnitude <= runRadius * runRadius)
    {
      ChangeState(PersonState.Running);
    }

    // Running
    if (state == PersonState.Running)
    {
      if (oldState != state)
      {
        if (screamAudio != null)
          AudioSource.PlayClipAtPoint(screamAudio, transform.position);

        timer.SetRandomMaximumTime(runMinTime, runMaxTime);
        oldState = state;
      }

      // Change direction
      transform.localEulerAngles = Utils.RotateTo(-deltaPos);
      rigidbody2D.velocity = -deltaPos.normalized * unit.speed;

      if (timer.PassedMaximum())
      {
        ChangeState(PersonState.Idle);
      }
    }
    // Idle
    else if (state == PersonState.Idle)
    {
      if (oldState != state)
      {
        timer.SetRandomMaximumTime(0f, idleMaxTime);
        oldState = state;
      }

      rigidbody2D.velocity = new Vector2();

      if (timer.PassedMaximum())
      {
        ChangeState(PersonState.Walking);
      }
    }
    // Walking
    else if (state == PersonState.Walking)
    {
      if (oldState != state)
      {
        timer.SetRandomMaximumTime(0f, walkMaxTime);

        walkDirection = Random.insideUnitCircle.normalized;
        transform.localEulerAngles = Utils.RotateTo(walkDirection);
        rigidbody2D.velocity = walkDirection * unit.speed / 2f;

        oldState = state;
      }

      if (timer.PassedMaximum())
      {
        ChangeState(PersonState.Idle);
      }
    }
    // None
    else
    {
      ChangeState(PersonState.Idle);
    }
  }

  void ChangeState(PersonState newState)
  {
    oldState = state;
    state = newState;
  }
}
