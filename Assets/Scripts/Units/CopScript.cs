using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UnitScript))]
public class CopScript : MonoBehaviour
{
  PlayerScript player;
  UnitScript unit;

  // Cop States
  enum CopState
  {
    None,
    Idle,
    Walking,
    Chasing,
    Shooting
  }

  CopState state = CopState.None,
    oldState = CopState.None;

  // Timer
  Timer timer;

  // Chase settings
  public float chaseRadius = 5f;
  public float shootDistance = 3f;
  public float shootInterval = 1f;
  Timer shootTimer;
  bool canShoot = true;
  Vector2 chasePosition;

  // Walking settings
  public float idleMaxTime = 0.5f;
  public float walkMaxTime = 1f;
  Vector2 walkDirection;

  // Audio
  public AudioClip shootAudio;

  void Start()
  {
    player = PlayerScript.instance;
    unit = transform.GetComponent<UnitScript>();

    timer = new Timer();

    shootTimer = new Timer();
    shootTimer.SetMaximumTime(shootInterval);
  }

  void Update()
  {
    if (HUDScript.endGame)
    {
      rigidbody2D.velocity = new Vector2();
      return;
    }

    Vector2 deltaPos = Utils.V3toV2(player.transform.position - transform.position);

    // Check if the cops see the player
    float sqrDist = deltaPos.sqrMagnitude;
    if (sqrDist <= chaseRadius * chaseRadius)
    {
      chasePosition = player.transform.position;

      if (sqrDist > shootDistance * shootDistance)
        ChangeState(CopState.Chasing);
      else
        ChangeState(CopState.Shooting);
    }
    // Else, it goes idle
    else if (state == CopState.Chasing)
    {
      ChangeState(CopState.Idle);
    }

    // Shoot update
    if (!canShoot)
    {
      if (shootTimer.PassedMaximum())
      {
        canShoot = true;
        shootTimer.Reset();
      }
    }

    // Chasing
    if (state == CopState.Chasing)
    {
      transform.localEulerAngles = Utils.RotateTo(chasePosition - Utils.V3toV2(transform.position));
      rigidbody2D.velocity = deltaPos.normalized * unit.speed;
    }
    // Shooting
    else if (state == CopState.Shooting)
    {
      if (oldState != state)
      {
        oldState = state;
      }

      transform.localEulerAngles = Utils.RotateTo(chasePosition - Utils.V3toV2(transform.position));
      rigidbody2D.velocity = new Vector2();

      if (canShoot)
      {
        Shoot(deltaPos);
      }
    }
    // Idle
    else if (state == CopState.Idle)
    {
      if (oldState != state)
      {
        timer.SetRandomMaximumTime(0f, idleMaxTime);
        oldState = state;
      }

      rigidbody2D.velocity = new Vector2();

      if (timer.PassedMaximum())
      {
        ChangeState(CopState.Walking);
      }
    }
    // Walking
    else if (state == CopState.Walking)
    {
      if (oldState != state)
      {
        timer.SetRandomMaximumTime(0f, walkMaxTime);

        walkDirection = Random.insideUnitCircle.normalized;
        transform.localEulerAngles = Utils.RotateTo(walkDirection);

        oldState = state;
      }

      rigidbody2D.velocity = walkDirection * unit.speed / 2f;

      if (timer.PassedMaximum())
      {
        ChangeState(CopState.Idle);
      }
    }
    // None
    else
    {
      ChangeState(CopState.Idle);
    }
  }

  void Shoot(Vector2 direction)
  {
    // Audio
    if (shootAudio != null)
      AudioSource.PlayClipAtPoint(shootAudio, transform.position);

    BulletScript bullet = ObjectPoolScript.instance.GetObjectForType("Bullet", false).GetComponent<BulletScript>();
    bullet.transform.position = transform.FindChild("shoot").position;
    bullet.Shoot(direction);

    canShoot = false;
  }

  void ChangeState(CopState newState)
  {
    oldState = state;
    state = newState;
  }
}
