using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

  public const float MoveSpeed = 6;
  public const float JumpHeight = 8;
  [SerializeField]
  private LayerMask whatIsGround;  //WhatIsGround?


  private Transform groundCheck;
  private bool isGrounded = false;


  //For Player animations
  private Transform anim;
  private Animator myAnimator;

  // Use this for initialization
  void Start()
  {
    groundCheck = transform.Find("GroundCheck");

    //For Player animations
    anim = transform.Find("Animations");
    myAnimator = anim.GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!GameManager.gameOver)    // Player is only controllable when not in Game-Over-Mode
    {

      if (Random.value < 0.0005f)
      {
        AudioManager.Play("Cough", false);
      }

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
      if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
      {
        vel.y += JumpHeight;
        AudioManager.Play("Jump");
      }

      if (isGrounded)
      {
        vel.x *= 0.4f;
        if (Mathf.Abs(vel.x) > 0.2f)
        {
          AudioManager.Play("Walk", false);
        }
        else
        {
          AudioManager.Stop("Walk");
        }
      }
      else
      {
        vel.x *= 0.9f;
        AudioManager.Stop("Walk");
      }
      myAnimator.SetFloat("speed", Mathf.Abs(vel.x));
      myAnimator.SetBool("Ground", isGrounded);

      ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity = vel;

      // Moving player to above -10 if he has fallen off the map
      if (this.transform.position.y < -10)
      {
        this.transform.position = new Vector3(-40, 0, -0.1f);
        GameManager.GameOver();
      }
    }
  }

  void OnCollisionEnter2D(Collision2D coll)
  {

    if (coll.gameObject.tag == "Enemy")
    {
      AudioManager.Play("Blow");
      GameManager.splash(coll.gameObject.transform.position + Vector3.down * 0.1f);
      Destroy(coll.gameObject);
    }
  }
}
