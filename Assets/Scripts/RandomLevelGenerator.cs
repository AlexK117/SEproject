using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomLevelGenerator : MonoBehaviour {

  [System.Serializable]
  public class levelType  {    public string name; public int count;  }

  public Transform player;
  public levelType[] levelTypes;

  float loadThreshhold = -1000;
  Vector3 nextLoadPosition = new Vector2(-8, 0);
  bool loadingScene = false;

  private int currentLevel = 0;
  private int counterUntilNextLevel = 0;

	// Use this for initialization
	void Start () {
    SceneManager.sceneLoaded += sceneLoaded;
    SceneManager.LoadSceneAsync("Scenes/Grass/Level0", LoadSceneMode.Additive);
    loadingScene = true;
  }
	
	// Update is called once per frame
	void Update () {
		if (!loadingScene && player.position.x > loadThreshhold)
    {
      SceneManager.sceneLoaded += sceneLoaded;
      SceneManager.LoadSceneAsync("Scenes/" + levelTypes[currentLevel].name + "/Level" + Random.Range(1, levelTypes[currentLevel].count + 1), LoadSceneMode.Additive);
      loadingScene = true;
    }
    Debug.DrawLine(new Vector3(loadThreshhold, -20, -5), new Vector3(loadThreshhold, 20, -5), Color.red);
    Debug.DrawLine(nextLoadPosition - Vector3.right - Vector3.up, nextLoadPosition + Vector3.right + Vector3.up, Color.green, 0, false);
    Debug.DrawLine(nextLoadPosition + Vector3.right - Vector3.up, nextLoadPosition - Vector3.right + Vector3.up, Color.green, 0, false);
  }

  void sceneLoaded(Scene scene, LoadSceneMode mode)
  {
    SceneManager.sceneLoaded -= sceneLoaded;
    StartCoroutine(InitializeAfterLoad(scene));
  }

  IEnumerator InitializeAfterLoad(Scene scene)
  {
    while (scene.isLoaded == false)
    {
      yield return new WaitForEndOfFrame();
    }

    Vector3 moveDistance = Vector3.zero;
    GameObject[] sceneObjects = scene.GetRootGameObjects();
    foreach(GameObject sceneObject in sceneObjects)
    {
      if (sceneObject.name == "Start")
      {
        moveDistance = nextLoadPosition - sceneObject.transform.position;
        Debug.Log(sceneObject.transform.position);
        Debug.Log(moveDistance);
        loadThreshhold = sceneObject.transform.position.x + moveDistance.x;
        Debug.Log(loadThreshhold);
      }
      if (sceneObject.name == "End")
      {
        nextLoadPosition = moveDistance + sceneObject.transform.position;
        Debug.Log(nextLoadPosition);
      }
    }

    foreach (GameObject sceneObject in sceneObjects)
    {
      sceneObject.transform.position += moveDistance;
    }
    counterUntilNextLevel++;
    if (counterUntilNextLevel > 10)
    {
      counterUntilNextLevel = 0;
      currentLevel = Random.Range(0, levelTypes.Length);
    }
    loadingScene = false;
  }
}
