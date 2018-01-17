using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {


  [SerializeField]
  private float x, y, z;
  [SerializeField]
  private int type;

  private int reload = 0;
  public float ammo = 1f;
  
  void Start () {
		
	}
	
	void Update () {
		if (reload > 0)
    {
      reload--;
    }
	}

  //move transform position to the players hand after it has been equipped
  public void resetTransform(Transform t)
  {
    transform.rotation = t.rotation;
    transform.position = t.position + new Vector3(t.rotation.y > 0 ? -x : x, y, z);
  }

  //make sure the weapons z position always stays in front of the player even when he turns around and rotates by 180 degrees around the Y axis
  public void flip(float z)
  {
    transform.position = new Vector3(transform.position.x, transform.position.y, z);
  }

  //spawn bullets with different properties according to each weapon type
  public void shoot()
  {
    if (reload <= 0 && ammo > 0)
    {
      Bullet bullet;

      switch (type)
      {
        case 0:
          bullet = Instantiate(GameManager.instance.bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
          bullet.lifetime = 1;
          bullet.speed = 0.2f;
          bullet.angle = transform.parent.rotation.y * 180;
          reload = 20;
          AudioManager.Play("Pistol");
          ammo -= 0.02f;
          break;
        case 1:
          bullet = Instantiate(GameManager.instance.bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
          bullet.lifetime = 1;
          bullet.speed = 0.4f;
          bullet.angle = transform.parent.rotation.y * 180;
          reload = 5;
          AudioManager.Play("Maschinegun");
          ammo -= 0.005f;
          break;
        case 2:
          for (int i = -10; i <= 10; i += 10)
          {
            bullet = Instantiate(GameManager.instance.bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.lifetime = 0.125f;
            bullet.speed = 0.4f;
            bullet.angle = i + transform.parent.rotation.y * 180;
            reload = 35;
          }
          AudioManager.Play("Shotgun");
          ammo -= 0.05f;
          break;
      }
      ammo = (Mathf.RoundToInt(ammo * 1000)) * 0.001f; 
    }
  }
}
