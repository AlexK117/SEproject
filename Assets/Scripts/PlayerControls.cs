using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

  public const float MoveSpeed = 6;
  public const float JumpHeight = 8;
  [SerializeField]
  private LayerMask whatIsGround;
  [SerializeField]
  private Collider2D attackHitBox;
  [SerializeField]
  private GameObject[] heartSprites;
  [SerializeField]
  private GameObject ammobarSprite;
  
  public GameObject currentWeapon = null;


  private Transform groundCheck;
  private bool isGrounded = false;
  private bool attacking;

  private int invincibilty = 0;
  private int health = 3;

  //For Player animations
  private Transform anim;
  private Animator myAnimator;

  void Start()
  {
    groundCheck = transform.Find("GroundCheck");

    //For Player animations
    anim = transform.Find("Animations");
    myAnimator = anim.GetComponent<Animator>();
  }

  //equips a new weapon: Destroys current weapon, sets the new weapon transforms parent to the player, removes the components that make it a pickup able item and adjusts its position to the players hand
  void equip(GameObject weapon)
  {
    if (currentWeapon != null)
    {
      Destroy(currentWeapon);
    }
    ((Weapon)weapon.GetComponent<Weapon>()).resetTransform(transform);
    weapon.transform.SetParent(transform);
    Destroy(weapon.GetComponent<Rigidbody2D>());
    Destroy(weapon.GetComponent<BoxCollider2D>());
    currentWeapon = weapon;
    ammobarSprite.transform.localScale = new Vector3(((Weapon)currentWeapon.GetComponent<Weapon>()).ammo, 1, 1);
  }
  
  void Update()
  {
    if (!GameManager.gameOver)    // Player is only controllable when not in Game-Over-Mode
    {
      //keep track of the distance the player has traveled to update the score with
      if ((int)transform.position.x > GameManager.distance)
      {
        GameManager.distance = (int)transform.position.x;
      }

      //play random coughing noises occasionally
      if (Random.value < 0.0002f)
      {
        AudioManager.Play("Cough", false);
      }
      if (Random.value < 0.0002f && !AudioManager.IsPlaying("Cough"))
      {
        AudioManager.Play("Cough2", false);
      }

      isGrounded = false;
      Vector2 vel = ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity;

      // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
      Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, whatIsGround);
      for (int i = 0; i < colliders.Length; i++)
      {
        if (colliders[i].gameObject != gameObject)
          isGrounded = true;
      }

      //left and right input, adjusts velocity on ground and in air (less), rotates the player around and adjusts the weapons z-position
      //to always be in front of the player even when the object is rotated by 180 degrees along the Y axis
      if (Input.GetKey(KeyCode.RightArrow))
      {
        vel.x += isGrounded ? MoveSpeed : MoveSpeed / 10;
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        if (currentWeapon != null)
        {
          ((Weapon)currentWeapon.GetComponent<Weapon>()).flip(-0.15f);
        }
      }
      if (Input.GetKey(KeyCode.LeftArrow))
      {
        vel.x -= isGrounded ? MoveSpeed : MoveSpeed / 10;
        this.transform.rotation = new Quaternion(0, 180, 0, 0);
        if (currentWeapon != null)
        {
          ((Weapon)currentWeapon.GetComponent<Weapon>()).flip(-0.15f);
        }
      }

      //jumping
      if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
      {
        vel.y += JumpHeight;
        AudioManager.Play("Jump");
      }

      //friction and walking sounds
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

      //attacking and shooting
      if (Input.GetKeyDown(KeyCode.A) && !attacking)
      {
        myAnimator.SetTrigger("Attack");
        attacking = true;
        AudioManager.Play("Hit");
      }
      if (Input.GetKey(KeyCode.S) && !attacking)
      {
        if ( currentWeapon != null)
        {
          ammobarSprite.transform.localScale = new Vector3(((Weapon)currentWeapon.GetComponent<Weapon>()).ammo, 1, 1);
          ((Weapon)currentWeapon.GetComponent<Weapon>()).shoot();
        }
      }
      //update animator
      myAnimator.SetFloat("speed", Mathf.Abs(vel.x));
      myAnimator.SetBool("Ground", isGrounded);

      attacking = myAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State");
      attackHitBox.offset = new Vector2(0.3f, attacking ? 0 : 1000);

      ((Rigidbody2D)this.GetComponent<Rigidbody2D>()).velocity = vel;

      // End the game if the player has fallen off the map
      if (this.transform.position.y < -50)
      {
        this.transform.position = new Vector3(-40, 0, -0.1f);
        GameManager.GameOver();
      }

      //make the player sprite flicker while he's invincible after taking a hit from an enemy
      if (invincibilty > 0)
      {
        invincibilty--;
        ((SpriteRenderer)this.GetComponentInChildren<SpriteRenderer>()).color = new Color(1, 1, 1, (invincibilty % 10) < 5 ? 1 : 0);
      }
    }
  }

  //managing health, blood and invicibility when the player takes damage
  private void takeDamage()
  {
    if (invincibilty == 0)
    {
      GameManager.splash(transform.position + Vector3.down * 0.1f);
      invincibilty = 50;
      health--;
      Destroy(heartSprites[health]);
    }
    if ( health <= 0)
    {
      this.transform.position = new Vector3(-40, 0, -0.1f);
      GameManager.GameOver();
    }
  }

  //kill enemy when they collide with the attack hitbox
  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.tag == "Enemy")
    {
      ((Bunny)coll.gameObject.GetComponent<Bunny>()).die();
    }
  }

  //manage collisions with enemies, weapon pickups and ammunition
  void OnCollisionEnter2D(Collision2D coll)
  {
    if (coll.gameObject.tag == "Enemy")
    {
      takeDamage();
    }
    if (coll.gameObject.tag == "Weapon")
    {
      equip(coll.gameObject);
    }
    if (coll.gameObject.tag == "Ammo")
    {
      ((Weapon)currentWeapon.GetComponent<Weapon>()).ammo = 1;
      ammobarSprite.transform.localScale = new Vector3(((Weapon)currentWeapon.GetComponent<Weapon>()).ammo, 1, 1);
      Destroy(coll.gameObject);
    }
  }
}
