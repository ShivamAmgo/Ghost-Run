using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class VictimMovement : MonoBehaviour
{
  [SerializeField] public float Speed = 10;
  [SerializeField] private float SpeedIncreaseFactor=1.25f;
  [SerializeField] private float JumpDuration = 0.5f;
  [SerializeField]private Vector3 Offset;
  [SerializeField] private Animator m_Animator;
  [SerializeField] private List<Rigidbody> RigRigidbodies;
  
  private bool IsMoving = false;

  public delegate void Fleeing();

  public static event Fleeing OnFlee;
  private LayerMask GroundMask;

private void Start()
{
  GroundMask=LayerMask.GetMask("Ground");
}

private void OnEnable()
  {
    GameManagerGhost.TransformGhostPlayer += IncreaseSpeed;
    Obstacle.OnJumpTriggered += Jump;
    MimicHandAttack.OnVictimCatched += Caught;
  }

  private void FixedUpdate()
  {
    if (!IsMoving)return;
    
    transform.position += (transform.forward*Time.deltaTime*Speed);
    
  }

  void IncreaseSpeed(bool status)
  {
    Speed+=SpeedIncreaseFactor;
  }
  private void Caught(Transform victim)
  {
    
      RagdollActive(true);
      Speed = 0;
      Debug.Log("Equillibrium");
  }

  void RagdollActive(bool activestatus)
  {
    m_Animator.enabled = !activestatus;
    foreach (Rigidbody rb in RigRigidbodies)
    {
      /*
      if (rb.name=="Head")
      {
        continue;
      }
      */
      rb.isKinematic = !activestatus;
      //rb.useGravity = !activestatus;
      //rb.AddForce(Vector3.forward*5,ForceMode.Impulse);
    }
    
  }

  public void SetGravity(bool GravityStatus)
  {
    foreach (Rigidbody rb in RigRigidbodies)
    {
      
      
      rb.useGravity = GravityStatus;
      //rb.AddForce(Vector3.forward*5,ForceMode.Impulse);
    }
  }

  
  public void Flee()
  {
    Debug.Log("FLEE");
    transform.rotation = Quaternion.identity;
    IsMoving = true;
    OnFlee?.Invoke();
    
  }

  void Jump(float JumpPower)
  {
    
    IsMoving = false;
    Vector3 jumpZ = transform.position + new Vector3(0, 0, 4);
    Vector3 jumpY = transform.position + new Vector3(0, JumpPower, 0);
    float duration = (Vector3.Distance(jumpY, transform.position)/9.8f); 
    //Debug.Log("Jump"+duration);
                   
    transform.DOJump(jumpZ,JumpPower,1,duration*2).
      SetEase(Ease.Linear).OnComplete(() =>
      {
        IsMoving = true;
      });
      
    
    
    /*
    float duration = Vector3.Distance(transform.position + new Vector3(0,JumpHeight,0),transform.position) / 9.8f;
    Debug.Log("time "+duration);
    transform.DOMoveY(transform.position.y + JumpHeight, 0.5f).SetLoops(1).SetEase(Ease.Linear).OnComplete(
      () =>
      {
        AlignToGround(Vector3.zero, true);
      });
      */

  }
  /*
  void AlignToGround(Vector3 OffsetFromground,bool Tween)
  {
      //float angle;
        RaycastHit raycastHit;
        Physics.Raycast(transform.position+Offset, -transform.up , out raycastHit, 50, GroundMask);
        Debug.DrawRay(transform.position+Offset,-transform.up,Color.blue,5);
        if (raycastHit.collider==null)
        {
            AlignToGround(OffsetFromground,Tween);
            return;
        }
        
        if (raycastHit.collider.transform.tag=="Ground")
        {
            //angle = Vector3.Angle(raycastHit.normal, Vector3.forward);
            float GroundDistance=Vector3.Distance(raycastHit.point,transform.position);
            //Vector3 AngleDiff = new Vector3(-1 * (angle-90), transform.eulerAngles.y, 0);
            //DOTween.KillAll();
            if (!Tween)
            {
                
                transform.position = raycastHit.point+OffsetFromground;
                //m_PlayerMovement.Speed = playerNormalSpeed;
                //Debug.Log("offest from grounf "+OffsetFromground);
            }
            
            else
            {
                
                transform.DOMoveY((raycastHit.point.y + OffsetFromground.y), GroundDistance/9.8f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                       
                    });
            }
            //transform.rotation = Quaternion.Euler(AngleDiff);
            
            return;
            
            //transform.position = new Vector3(transform.position.x, GroundOffset.y+transform.position.y, transform.position.z);
            return;
            ;
        }
            
        //m_PlayerRotation.enabled = true;
        //yield return new WaitForSeconds(0.15f);
       // m_PlayerRotation.enabled = false;
        //transform.position = new Vector3(transform.position.x, GroundOffset.y, transform.position.z);
        
    }
*/
  private void OnDisable()
  {
    GameManagerGhost.TransformGhostPlayer -= IncreaseSpeed;
    Obstacle.OnJumpTriggered -= Jump;
    MimicHandAttack.OnVictimCatched -= Caught;
  }
}
