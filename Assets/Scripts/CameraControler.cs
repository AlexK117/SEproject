using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{

  public Transform target;
  public float damping = 1;

  private Vector3 m_LastTargetPosition;
  private Vector3 m_CurrentVelocity;

  // Use this for initialization
  void Start()
  {
    m_LastTargetPosition = target.position;
    transform.parent = null;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (!GameManager.gameOver)              // Camera isn't moving when in Game-Over-Mode
    {
      Vector3 targetPos = target.position + Vector3.forward * -10;
      Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPos, ref m_CurrentVelocity, damping);

      transform.position = newPos;

      m_LastTargetPosition = target.position;
    }
  }

}
