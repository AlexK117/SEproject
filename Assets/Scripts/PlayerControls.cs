using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

  public const float MoveSpeed = 3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.D))
    { 
      ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity = new Vector2(MoveSpeed, ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity.y);
      this.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    if (Input.GetKey(KeyCode.A))
    {
      ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity = new Vector2(-MoveSpeed, ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity.y);
      this.transform.rotation = new Quaternion(0, 180, 0, 0);
    }

    if (this.transform.position.y < -10)
    {
      this.transform.position = new Vector3(0, 0, 0);
    }
  }
}
