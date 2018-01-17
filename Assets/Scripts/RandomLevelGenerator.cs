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
  Vector3 nextLoadPosition = new Vector3(-8, 0, 0);
  bool loadingScene = false;

  private int currentLevel = 0;
  private int counterUntilNextLevel = 0;
  
  //Instantly load the retirement home scene when the game starts
	void Start () {
    nextLoadPosition = new Vector3(-8, 0, 0);
    SceneManager.sceneLoaded += sceneLoaded;
    SceneManager.LoadSceneAsync("Scenes/Grass/Level0", LoadSceneMode.Additive);
    loadingScene = true;
  }
	
	//Load a new scene when there's none loading right now and the player passes the loading theshhold
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

  //remove the scene loading handler and initialize the scene after it finished loading
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

    //find the start and end point in the scene, calculate the distance to move each gameobject in the scene so it fits nicely at the end of the last scene
    //Also update the loading threshhold and the next load position
    Vector3 moveDistance = Vector3.zero;
    GameObject[] sceneObjects = scene.GetRootGameObjects();
    foreach(GameObject sceneObject in sceneObjects)
    {
      if (sceneObject.name == "Start")
      {
        moveDistance = nextLoadPosition - sceneObject.transform.position;
        loadThreshhold = sceneObject.transform.position.x + moveDistance.x;
      }
      if (sceneObject.name == "End")
      {
        nextLoadPosition = moveDistance + sceneObject.transform.position;
      }
    }

    //move all objects in the scene according to the previously calculated distance
    foreach (GameObject sceneObject in sceneObjects)
    {
      sceneObject.transform.position += moveDistance;
    }

    //increment the scene counter and change the level type to load after it passes 10 (level types are random and it might be the same as before)
    counterUntilNextLevel++;
    if (counterUntilNextLevel > 10)
    {
      counterUntilNextLevel = 0;
      currentLevel = Random.Range(0, levelTypes.Length);
    }
    loadingScene = false;
  }
}
