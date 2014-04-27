using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour
{
  public static HUDScript instance { get; private set; }

  public Transform[] playing;
  public Transform[] dead;

  bool playerAlive;

  RebindData rebind;

  void OnEnable()
  {
    instance = this;
  }

  void Start()
  {
    rebind = RebindData.GetRebindManager();

    for (int i = 0; i < playing.Length; ++i)
      playing[i].gameObject.SetActive(true);

    for (int i = 0; i < dead.Length; ++i)
      dead[i].gameObject.SetActive(false);

    playerAlive = true;
  }

  public void OnPlayerDeath()
  {
    for (int i = 0; i < playing.Length; ++i)
      playing[i].gameObject.SetActive(false);

    for (int i = 0; i < dead.Length; ++i)
      dead[i].gameObject.SetActive(true);

    playerAlive = false;
  }

  void Update()
  {
    if (!playerAlive)
    {
      if (rebind.GetKeyDown("Restart"))
        Application.LoadLevel("MainScene");
    }
  }
}
