using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour {
  
  [SerializeField]
  private LayerMask whatIsGround;  //WhatIsGround?

  private Transform groundCheck;
  private Transform wallCheck;
  private Transform destinationCheck;
  private bool isGrounded = true;

  private enum State { IDLE, JUMP, ATTACK };

  //For Player animations
  private Transform anim;
  private Animator myAnimator;
  private State state = State.IDLE;
  private int facing = 1;
  private int waitTimer = 0;

  // Use this for initialization
  void Start()
  {
    groundCheck = transform.Find("GroundCheck");
    wallCheck = transform.Find("WallCheck");
    destinationCheck = transform.Find("DestinationCheck");

    //For Player animations
    anim = transform.Find("Animations");
    myAnimator = anim.GetComponent<Animator>();
  }

  void FixedUpdate()
  {
    if (waitTimer > 0) waitTimer--;
  }

  private bool isWall()
  {
    Collider2D [] colliders = Physics2D.OverlapCircleAll(wallCheck.position, 0.05f, whatIsGround);
    for (int i = 0; i<colliders.Length; i++)
    {
      if (colliders[i].gameObject != gameObject)
      {
      return true;
      }
    }
    return false;
  }

  private bool validDestination()
  {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(destinationCheck.position, 0.2f, whatIsGround);
    for (int i = 0; i < colliders.Length; i++)
    {
      if (colliders[i].gameObject != gameObject)
      {
        return true;
      }
    }
    return false;
  }

  // Update is called once per frame
  void Update()
  {
    Vector2 vel = ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity;

    if (state != State.IDLE && vel.y <= 0) {
      Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, whatIsGround);
      for (int i = 0; i < colliders.Length; i++)
      {
        if (colliders[i].gameObject != gameObject)
        {
          isGrounded = true;
          state = State.IDLE;
        }
      }
    }

    if (isGrounded)
    {
      vel.x *= 0.4f;
    }

    if (waitTimer == 0 && state == State.IDLE)
    {
      float rnd = Random.value;
      if (rnd > 0.75f)
      {
        waitTimer = 20;
      }
      else if (rnd > 0.5f)
      {
        facing = -facing;
        this.transform.rotation = new Quaternion(0, -90 * (facing - 1), 0, 0);
        waitTimer = 60;
      }
      else
      {
        if (validDestination() && !isWall())
        {
          vel.x = -3 * facing;
          vel.y = 3;
          isGrounded = false;
          state = State.JUMP;
        }
      }
    }
    
    myAnimator.SetBool("Ground", isGrounded);

    ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity = vel;

    if (this.transform.position.y < -10)
    {
      Destroy(gameObject);
    }


  }
}
