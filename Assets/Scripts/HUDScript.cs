using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour
{
  public static HUDScript instance { get; private set; }

  public Transform[] playing;
  public Transform[] dead;

  bool playerAlive;

  public static bool endGame { get; private set; }
  public Transform endGameBG;
  public Material endGameFont;
  public Transform endGameText;
  public float endGameTime = 1f;
  float endGameCurTime = 0f;

  RebindData rebind;

  void OnEnable()
  {
    instance = this;
    endGame = false;
  }

  void Start()
  {
    rebind = RebindData.GetRebindManager();

    for (int i = 0; i < playing.Length; ++i)
      playing[i].gameObject.SetActive(true);

    for (int i = 0; i < dead.Length; ++i)
      dead[i].gameObject.SetActive(false);

    playerAlive = true;

    ObjectiveScript.instance.OnHitObjective += EndGame;
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

    if (endGame)
    {
      endGameCurTime += Time.deltaTime;
      if (endGameCurTime > endGameTime)
        endGameCurTime = endGameTime;

      float ratio = endGameCurTime / endGameTime;

      Color c = endGameBG.GetComponent<SpriteRenderer>().color;
      endGameBG.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, ratio);

      c = endGameFont.color;
      endGameFont.color = new Color(c.r, c.g, c.b, ratio * ratio * ratio);
    }
  }

  void EndGame()
  {
    endGame = true;

    for (int i = 0; i < playing.Length; ++i)
      playing[i].gameObject.SetActive(false);

    for (int i = 0; i < dead.Length; ++i)
      dead[i].gameObject.SetActive(false);

    endGameBG.gameObject.SetActive(true);
    endGameText.gameObject.SetActive(true);
  }
}
