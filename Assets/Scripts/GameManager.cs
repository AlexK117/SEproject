using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public Transform player;
  public GameObject bulletPrefab;
  public GameObject[] bloodPrefabs;
  public GameObject bloodSpawner;
  public GameObject[] weaponPrefabs;
  public GameObject ammoPrefab;
  public TextMesh score;
  public static bool gameOver;
  public static bool restart;

  public static GameManager instance;
  public static int difficulty = 10;

  public static int distance = 0;
  public static int bonusScore = 0;

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
        distance = 0;
        bonusScore = 0;
      }
    }
    score.text = "Score: " + (distance + bonusScore);
	}

  public static void splash (Vector3 position)
  {
    float scale = Random.Range(0.75f, 1.5f);
    var blood = Instantiate(instance.bloodPrefabs[Random.Range(0, instance.bloodPrefabs.Length)], new Vector3(position.x, position.y, -1f), Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward));
    blood.transform.localScale = new Vector3(scale, scale);
    Instantiate(instance.bloodSpawner, new Vector3(position.x, position.y, -0.2f), Quaternion.identity);
  }
  public static void drop(Vector3 position)
  {
    GameObject drop = null;
    float rand = Random.value;
    if (rand < 0.3f)
    {
      if (((PlayerControls)instance.player.gameObject.GetComponent<PlayerControls>()).currentWeapon == null)
        drop = instance.weaponPrefabs[Random.Range(0, instance.weaponPrefabs.Length)];
      else
        drop = instance.ammoPrefab;
    }
    else if (rand < 0.4f)
    {
      drop = instance.weaponPrefabs[Random.Range(0, instance.weaponPrefabs.Length)];
    }

    if (drop != null)
    {
      Instantiate(drop, new Vector3(position.x, position.y, -0.2f), Quaternion.identity);
    }
  }

  // Moving Camera to Game-Over Screen on the map
  public static void GameOver()
  {
    gameOver = true;
    restart = true;

    AudioManager.Stop("Theme");
    AudioManager.Play("GameOver");
    Camera mainCam;
    mainCam = Camera.main;

    mainCam.transform.position = new Vector3(-30, 0, -10.1f);
  }
}
