using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(SelfPoolScript))]
public class BulletScript : MonoBehaviour
{
  // Damage
  public float damage = 15f;

  // Movement
  public float speed = 4f;
  public float maxTravelDistance = 20f;
  float distTravelled = 0f;
  Vector2 travelDirection;

  public void Shoot(Vector2 direction)
  {
    travelDirection = direction;
    transform.localEulerAngles = Utils.RotateTo(direction);
    rigidbody2D.velocity = direction.normalized * speed;
    distTravelled = 0f;
  }

  void OnCollisionEnter2D(Collision2D coll)
  {
    if (coll.collider != null)
    {
      UnitScript unit = coll.collider.GetComponent<UnitScript>();
      if (unit != null)
      {
        unit.ReceiveDamage(damage);

        // Blood effect
        GameObject blood = ObjectPoolScript.instance.GetObjectForType("Blood", false);
        blood.transform.rotation = transform.rotation;

        Vector3 delta = unit.transform.position - transform.position;
        Vector3 dec = travelDirection.normalized * Vector2.Dot(Utils.V3toV2(delta), travelDirection) * delta.magnitude;
        blood.transform.position = transform.position + 2 * dec;
      }
    }

    GetComponent<SelfPoolScript>().PoolObject();
  }

  void Update()
  {
    // Avoids bullets going forever
    distTravelled += speed * Time.deltaTime;
    if (distTravelled >= maxTravelDistance)
      GetComponent<SelfPoolScript>().PoolObject();
  }
}
