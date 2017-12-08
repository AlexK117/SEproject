using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public Transform player;
  public GameObject[] bloodPrefabs;
  public GameObject bloodSpawner;

  public static GameManager instance;
  public static int difficulty = 0;

	// Use this for initialization
	void Start () {
    instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public static void splash (Vector3 position)
  {
    float scale = Random.Range(0.75f, 1.5f);
    var blood = Instantiate(instance.bloodPrefabs[Random.Range(0, instance.bloodPrefabs.Length)], new Vector3(position.x, position.y, -1f), Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward));
    blood.transform.localScale = new Vector3(scale, scale);
    Instantiate(instance.bloodSpawner, new Vector3(position.x, position.y, -0.2f), Quaternion.identity);
  }
}
