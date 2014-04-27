using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(SelfPoolScript))]
public class UnitScript : MonoBehaviour
{
  // Sprite
  public GameObject deadCorpse;
  public bool pollWhenDead = true;

  // Health
  bool alive = true;
  public float maxHealth = 10f;
  public float currentHealth = 10f;

  // Movement
  public float speed = 1f;

  public void ReceiveDamage(float damage)
  {
    currentHealth -= damage;
    if (currentHealth <= 0)
      Die();
  }

  void Die()
  {
    if (deadCorpse != null)
    {
      GameObject corpse = ObjectPoolScript.instance.GetObjectForType(deadCorpse.name, false);
      Vector3 rotation = transform.localEulerAngles;
      rotation.z += 180;
      corpse.transform.localEulerAngles = rotation;
      corpse.transform.position = transform.position;
    }

    if (OnDeath != null)
      OnDeath();

    if (pollWhenDead)
      GetComponent<SelfPoolScript>().PoolObject();
    else
      //Destroy(gameObject);
      gameObject.SetActive(false);
  }

  public bool IsDead()
  {
    return !alive;
  }

  public delegate void OnDeathDelegate();
  public OnDeathDelegate OnDeath;
}
