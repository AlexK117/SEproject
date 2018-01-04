using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public Transform player;
  public GameObject[] bloodPrefabs;
  public GameObject bloodSpawner;
  public static bool gameOver;
  public static bool restart;

  public static GameManager instance;
  public static int difficulty = 0;

	// Use this for initialization
	void Start () {
      instance = this;
      gameOver = false;
      restart = false;
	}
	
	// Update is called once per frame
	void Update ()
  {
    // Reloading the level when in Game-Over-Mode and when Return is pressed
		if (restart)
    {
      if (Input.GetKeyDown(KeyCode.Return))
      {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        restart = false;
        gameOver = false;
      }
    }
	}

  public static void splash (Vector3 position)
  {
    float scale = Random.Range(0.75f, 1.5f);
    var blood = Instantiate(instance.bloodPrefabs[Random.Range(0, instance.bloodPrefabs.Length)], new Vector3(position.x, position.y, -1f), Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward));
    blood.transform.localScale = new Vector3(scale, scale);
    Instantiate(instance.bloodSpawner, new Vector3(position.x, position.y, -0.2f), Quaternion.identity);
  }

  // Moving Camera to Game-Over Screen on the map
  public static void GameOver()
  {
    gameOver = true;
    restart = true;

    Camera mainCam;
    mainCam = Camera.main;

    mainCam.transform.position = new Vector3(-30, 0, -10.1f);
  }
}
