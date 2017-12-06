using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

  public const float MoveSpeed = 6;
  public const float JumpHeight = 8;
  [SerializeField] private LayerMask whatIsGround;  //WhatIsGround?
  public AudioClip bla;

  private Transform groundCheck;
  private bool isGrounded = false;

  //For Player animations
  private GameObject anim;
  private Animator myAnimator;

  // Use this for initialization
  void Start ()
  {
    groundCheck = transform.Find("GroundCheck");

    //For Player animations
    anim = GameObject.Find("Animations");
    myAnimator = anim.GetComponent<Animator>();
  }
	
	// Update is called once per frame
	void Update () {
    isGrounded = false;
    Vector2 vel = ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity;

    // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
    // This can be done using layers instead but Sample Assets will not overwrite your project settings.
    Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, whatIsGround);
    for (int i = 0; i < colliders.Length; i++)
    {
      if (colliders[i].gameObject != gameObject)
      isGrounded = true;  
    }

    if (Input.GetKey(KeyCode.RightArrow))
    {
      vel.x += isGrounded ? MoveSpeed : MoveSpeed / 10;
      this.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    if (Input.GetKey(KeyCode.LeftArrow))
    {
      vel.x -= isGrounded ? MoveSpeed : MoveSpeed / 10;
      this.transform.rotation = new Quaternion(0, 180, 0, 0);
    }
    if(Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
    {
      vel.y += JumpHeight;
    }
    if(Input.GetKeyDown(KeyCode.Space))
    {
      GameManager.instance.splash(transform.position + Vector3.down*0.3f);
      AudioManager.play(bla);
    }

    if(isGrounded)
    {
      vel.x *= 0.4f;
      myAnimator.SetFloat("speed", Mathf.Abs(vel.x));
    }
    else
    {
      vel.x *= 0.9f;
      myAnimator.SetFloat("speed", 0);
    }

    ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity = vel;

    if (this.transform.position.y < -10)
    {
      this.transform.position = new Vector3(0, 0, -0.1f);
    }

    
  }
}
