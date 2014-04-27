using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SelfPoolScript))]
public class ClawScript : MonoBehaviour
{
  public float damage = 10f;

  PlayerScript player;
  Animator anim;

  void Start()
  {
    player = PlayerScript.instance;
  }

  void OnEnable()
  {
    if (anim == null)
      anim = GetComponent<Animator>();

    anim.Play("ClawAnimation");
  }

  void Update()
  {
    AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
    if (stateInfo.IsName("Over"))
      GetComponent<SelfPoolScript>().PoolObject();

  }

  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll != null)
    {
      if (coll.tag == "Player")
        return;

      UnitScript unit = coll.GetComponent<UnitScript>();
      if (unit != null)
      {
        unit.ReceiveDamage(damage);

        // Blood effect
        GameObject blood = ObjectPoolScript.instance.GetObjectForType("Blood", false);
        blood.transform.rotation = transform.rotation;

        Vector3 delta = unit.transform.position - transform.position;
        Vector3 dec = player.transform.up.normalized * Vector2.Dot(Utils.V3toV2(delta), player.transform.up) * delta.magnitude;
        blood.transform.position = transform.position + 2 * dec;
      }
    }
  }
}
