using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [System.Serializable]
  public class Enemy { public GameObject enemy; public int difficulty; }
  public Enemy[] enemies;

  void Start()
  {
    int rnd = (int)Random.Range(0, enemies.Length);
    if (enemies[rnd].difficulty <= GameManager.difficulty)
    {
      Instantiate(enemies[rnd].enemy, new Vector3(transform.position.x, transform.position.y, -0.1f), Quaternion.identity);
    }
    Destroy(gameObject);
  }
}
