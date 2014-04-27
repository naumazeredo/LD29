using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UnitScript))]
public class PlayerScript : MonoBehaviour
{
  public static PlayerScript instance { get; private set; }

  // Control
  RebindData rebind;

  // Unit
  UnitScript unit;

  // Animator
  Animator anim;

  // Audio
  public AudioClip clawSound;

  void OnEnable()
  {
    instance = this;
  }

  void Start()
  {
    rebind = RebindData.GetRebindManager();
    unit = GetComponent<UnitScript>();
    anim = GetComponent<Animator>();

    //unit.OnDeath = OnDeath;
    unit.OnDeath = HUDScript.instance.OnPlayerDeath;
  }

  void Update()
  {
    // Controls
    int h = (rebind.GetKey("Right") ? 1 : 0) + (rebind.GetKey("Left") ? -1 : 0);
    int v = (rebind.GetKey("Up") ? 1 : 0) + (rebind.GetKey("Down") ? -1 : 0);

    if (h != 0 || v != 0)
      anim.SetBool("Walking", true);
    else
      anim.SetBool("Walking", false);

    rigidbody2D.velocity = new Vector2(h, v).normalized * unit.speed;

    if (h != 0 || v != 0)
      transform.localEulerAngles = Utils.RotateTo(h, v);

    if (rebind.GetKeyDown("Attack"))
      Attack();
  }

  void Attack()
  {
    // Audio
    if (clawSound != null)
      AudioSource.PlayClipAtPoint(clawSound, transform.position);

    GameObject claw = ObjectPoolScript.instance.GetObjectForType("Claw", false);
    claw.transform.position = transform.FindChild("claw").position;
  }
}
