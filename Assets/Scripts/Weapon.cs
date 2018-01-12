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

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (reload > 0)
    {
      reload--;
    }
	}

  public void resetTransform(Transform t)
  {
    transform.rotation = t.rotation;
    transform.position = t.position + new Vector3(t.rotation.y > 0 ? -x : x, y, z);
  }

  public void flip(float z)
  {
    transform.position = new Vector3(transform.position.x, transform.position.y, z);
  }

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
          ammo -= 0.02f;
          break;
        case 1:
          bullet = Instantiate(GameManager.instance.bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
          bullet.lifetime = 1;
          bullet.speed = 0.4f;
          bullet.angle = transform.parent.rotation.y * 180;
          reload = 5;
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
          ammo -= 0.05f;
          break;
      }
      ammo = (Mathf.RoundToInt(ammo * 1000)) * 0.001f; 
    }
  }
}
