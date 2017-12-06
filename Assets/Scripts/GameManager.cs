using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public Transform player;
  public GameObject[] bloodPrefabs;

  public static GameManager instance;

	// Use this for initialization
	void Start () {
    instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void splash (Vector3 position)
  {
    float scale = Random.Range(1.0f, 2.0f);
    var blood = Instantiate(bloodPrefabs[Random.Range(0, bloodPrefabs.Length)], new Vector3(position.x, position.y, 0), Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward));
    blood.transform.localScale = new Vector3(scale, scale);
    ((SpriteRenderer)blood.GetComponent<SpriteRenderer>()).flipX = Random.value < 0.5f;
  }
}
