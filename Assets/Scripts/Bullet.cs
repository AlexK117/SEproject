using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public float lifetime;
  public float speed;
  public float angle;

	// Use this for initialization
	void Start () {
    Destroy(gameObject, lifetime);
    this.transform.Rotate(new Vector3(0, 0, 1), Mathf.Deg2Rad * angle);
	}
	
	// Update is called once per frame
	void Update () {
    this.transform.position = new Vector3(this.transform.position.x + speed * Mathf.Cos(Mathf.Deg2Rad * angle), this.transform.position.y + speed * Mathf.Sin(Mathf.Deg2Rad * angle), this.transform.position.z);
  }

  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.tag == "Enemy")
    {
      ((Bunny)coll.gameObject.GetComponent<Bunny>()).die();
    }
  }
}
