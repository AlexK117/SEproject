using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bunny : MonoBehaviour {
  
  [SerializeField]
  private LayerMask whatIsGround;

  private Transform groundCheck;
  private Transform wallCheck;
  private Transform destinationCheck;
  private bool isGrounded = true;

  private enum State { IDLE, JUMP, ATTACK };

  //For animations
  private Transform anim;
  private Animator myAnimator;
  private State state = State.IDLE;
  private int facing = 1;
  private int waitTimer = 0;

  public AudioSource BunnyJump;
  
  void Start()
  {
    groundCheck = transform.Find("GroundCheck");
    wallCheck = transform.Find("WallCheck");
    destinationCheck = transform.Find("DestinationCheck");
    
    anim = transform.Find("Animations");
    myAnimator = anim.GetComponent<Animator>();

    BunnyJump = gameObject.AddComponent<AudioSource>();
    BunnyJump.clip = AudioManager.audioManager["JumpBunny"].clip;
    BunnyJump.pitch = 1.35f;
    BunnyJump.volume = 0.2f;
    BunnyJump.spatialBlend = 1f;
    BunnyJump.minDistance = 1f;
    BunnyJump.maxDistance = 7f;
    BunnyJump.rolloffMode = AudioRolloffMode.Linear;
  }

  void FixedUpdate()
  {
    if (waitTimer > 0) waitTimer--;
  }

  //Checks if there is a wall in front of the bunny
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

  //Cheks if there's ground a jump distance away from the bunny
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
  
  void Update()
  {
    Vector2 vel = ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity;

    //check if bunny is on the ground
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

    //friction
    if (isGrounded)
    {
      vel.x *= 0.4f;
    }

    //Bunny AI, determines wether the bunny waits, turns around or jumps whenever it's idle and the wait timer reaches 0
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
          BunnyJump.Play();
        }
      }
    }
    
    //update animator
    myAnimator.SetBool("Ground", isGrounded);
    //update new calculated velocity
    ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity = vel;

    //destroy bunny when it falls down the map
    if (this.transform.position.y < -50)
    {
      Destroy(gameObject);
    }


  }

  //handles sound, blood, drops and score if the bunny dies
  public void die()
  {
    AudioManager.Play("Blow");
    GameManager.splash(transform.position + Vector3.down * 0.1f);
    GameManager.drop(transform.position);
    Destroy(gameObject);
    GameManager.bonusScore += 100;
  }
}
