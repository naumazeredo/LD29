using UnityEngine;
using System.Collections;

public class TerrainGenScript : MonoBehaviour
{
  PlayerScript player;

  // Initial position
  public Vector2 playerInitial;
  public Vector2 playerObjective;

  // Terrain size
  public int width = 3;
  public int height = 3;

  // Objects
  public Transform objective;
  public Transform[] buildings;
  public Transform[] wall;

  // People
  public Transform[] people;
  public Transform[] cops;
  public int peoplePerBuilding = 3;
  public int copsPerBuilding = 3;

  void Start()
  {
    player = PlayerScript.instance;

    player.transform.position = playerInitial;

    // Generate terrain
    // Generate objective
    Instantiate(objective, playerObjective, Quaternion.identity);

    // Generate walls
    int nWalls = wall.Length;
    if (nWalls == 8)
    {
      float x = (width - 1) * 5f;
      float y = (height - 1) * 5f;
      float x2 = x * 2 + 7f;
      float y2 = y * 2 + 7f;
      float sx = x + 7f;
      float sy = y + 7f;

      Transform t;
      float tx;
      float ty;

      // bl
      Instantiate(wall[0], new Vector3(-7f, -7f, 0), Quaternion.identity);
      // b
      t = Instantiate(wall[1], new Vector3(x, -7f, 0), Quaternion.identity) as Transform;
      tx = t.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
      t.localScale = new Vector3(sx / tx, 1f);

      // br
      Instantiate(wall[2], new Vector3(x2, -7f, 0), Quaternion.identity);

      // l
      t = Instantiate(wall[3], new Vector3(-7f, y, 0), Quaternion.identity) as Transform;
      ty = t.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
      t.localScale = new Vector3(1f, sy / ty);
      
      // r 
      t = Instantiate(wall[4], new Vector3(x2, y, 0), Quaternion.identity) as Transform;
      ty = t.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;
      t.localScale = new Vector3(1f, sy / ty);

      // tl
      Instantiate(wall[5], new Vector3(-7f, y2, 0), Quaternion.identity);

      // t 
      t = Instantiate(wall[6], new Vector3(x, y2, 0), Quaternion.identity) as Transform;
      tx = t.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
      t.localScale = new Vector3(sx / tx, 1f);

      // tr
      Instantiate(wall[7], new Vector3(x2, y2, 0), Quaternion.identity);
    }

    // Generate buildinds
    int nBuildings = buildings.Length;
    if (nBuildings > 0)
    {
      for (int i = 0; i < width; ++i)
      {
        for (int j = 0; j < height; ++j)
        {
          float x = 10f * i, y = 10f * j;
          int rand = Random.Range(0, nBuildings);
          Instantiate(buildings[rand], new Vector3(x, y, 0f), Quaternion.identity);

          // Generate people
          if (people.Length > 0)
          {
            for (int k = 0; k < peoplePerBuilding; ++k)
            {
              Vector2 pos;

              float size = 2.75f;
              float greater = Random.Range(-size, size);
              float minor = (Random.Range(0f, 1f) < 0.5f ? -1 : 1) * size;

              if (Random.Range(0f, 1f) < 0.5f)
                pos = new Vector2(x + greater, y + minor);
              else
                pos = new Vector2(x + minor, y + greater);

              rand = Random.Range(0, people.Length);
              Transform person = Instantiate(people[rand], pos, Quaternion.identity) as Transform;
              person.localEulerAngles = Utils.RotateTo(Random.insideUnitCircle);
            }
          }

          if (cops.Length > 0)
          {
            for (int k = 0; k < copsPerBuilding; ++k)
            {
              Vector2 pos;

              float size = 2.75f;
              float greater = Random.Range(-size, size);
              float minor = (Random.Range(0f, 1f) < 0.5f ? -1 : 1) * size;

              if (Random.Range(0f, 1f) < 0.5f)
                pos = new Vector2(x + greater, y + minor);
              else
                pos = new Vector2(x + minor, y + greater);

              rand = Random.Range(0, cops.Length);
              Transform cop = Instantiate(cops[rand], pos, Quaternion.identity) as Transform;
              cop.localEulerAngles = Utils.RotateTo(Random.insideUnitCircle);
            }
          }
        }
      }
    }
  }
}


