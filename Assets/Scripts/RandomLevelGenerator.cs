﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomLevelGenerator : MonoBehaviour {

  public Transform player;
  public int levelCount;

  float loadThreshhold = -1000;
  Vector3 nextLoadPosition = new Vector2(-2, 0);
  bool loadingScene = false;

	// Use this for initialization
	void Start () {
  }
	
	// Update is called once per frame
	void Update () {
		if (!loadingScene && player.position.x > loadThreshhold)
    {
      SceneManager.sceneLoaded += sceneLoaded;
      SceneManager.LoadSceneAsync("Scenes/Grass/Level" + Random.Range(1, levelCount+1), LoadSceneMode.Additive);
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

    loadingScene = false;
  }
}
