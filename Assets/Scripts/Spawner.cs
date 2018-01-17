using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [System.Serializable]
  public class EnemyType { public GameObject enemy; public int difficulty; }
  public EnemyType[] enemies;


  //Instantiate one of the given enemy types at the spawners position if they're below the global difficulty level (which increases over time)
  //This makes it possible to spawn stronger enemys instead of weaker ones the further the game progresses
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
