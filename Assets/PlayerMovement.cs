using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    NormalRun,
    ElevatedRun
}



public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> RigRigidbodies;
    [SerializeField] public float Speed=3;
    [SerializeField] private float StrafeSpeed = 3;
    [SerializeField] private float RotatePlayerLimitInDegrees = 10;
    [SerializeField] private float RotateSpeed = 10;
    [SerializeField] private float RotataeBackSpeed = 10;
    [SerializeField] private Rigidbody PlayerRb;
    [SerializeField] private Transform GroundCheck;
    
    [SerializeField]private Animator PlayerAnim;
    public bool isMoving = false;
    private Touch Xmove;
    private float posX = 0;
    private Vector3 RotY = Vector3.zero;
    private RaycastHit RayHit;
    private Ray ray;
    private Vector3 AngleDiff;
    private State PlayerState;
    
    private LayerMask GroundMask;
    private Vector3 Offset = new Vector3(0, 0.6f, 0);
    float angle;
    [SerializeField] private float PlayerClampXPos = 5.5f;
    Vector3 ClampPlayerPosX=Vector3.zero;

    public delegate void PlayerInfoRaise(Transform Player);

    public static event PlayerInfoRaise DeliverPlayerInfo;
    private void Start()
    {
        //PlayerAnim = GetComponent<Animator>();
        //RagdollActive(false);
        //PlayerAnim.SetTrigger("Run");
        PlayerState = State.NormalRun;
        DeliverPlayerInfo?.Invoke(transform);
        //GroundMask=LayerMask.GetMask("Ground");
    }

    private void Update()
    {
       
        if (Application.isEditor)
        {
            posX = Input.GetAxis("Mouse X");
           ClampPlayerPos();
        }
        else
        {
            Xmove = Input.GetTouch(0);
            ClampPlayerPos();
            if (Xmove.phase==TouchPhase.Moved)
            {
                posX = Input.GetAxis("Mouse X");
                
            }
            else if(Xmove.phase==TouchPhase.Ended)
            {
                posX = 0;
                
            }
        }
    }

  
    void RotatePlayer()
    {
        posX = Mathf.Clamp(posX, -1, 1);
        //transform.rotation=Quaternion.Euler(0,posX*RotateSpeed*Time.deltaTime,0);
        transform.rotation=Quaternion.AngleAxis(posX*RotateSpeed*Time.deltaTime,Vector3.up);
       
    }

    private void LateUpdate()
    {
        //ClampPlayerPos();
    }

    private void FixedUpdate()
    {
        if (!isMoving)return;
        if (PlayerState==State.NormalRun)
        {
            //Debug.Log("movex");
            Move(posX);
        }
        else if (PlayerState==State.ElevatedRun)
        {
            ElevatedRun();
        }
       
        RotatePlayer();
        //AlignPlayer();
    }

    void ClampPlayerPos()
    {
        ClampPlayerPosX = transform.position;
        ClampPlayerPosX.x = Mathf.Clamp(ClampPlayerPosX.x, -PlayerClampXPos, PlayerClampXPos);
        transform.position = ClampPlayerPosX;
    }
    void Move(float XAxismove)
    {
        
        Vector3 playerpos = transform.position;
        playerpos.x += XAxismove;
        //transform.position = playerpos;
        transform.position += (Vector3.right*XAxismove*Time.deltaTime*StrafeSpeed+Vector3.forward*Time.deltaTime*Speed);
       
    }
  
    void ElevatedRun()
    {
        Physics.Raycast(GroundCheck.position + Offset, -GroundCheck.up*10, out RayHit,20,GroundMask);
        Debug.DrawRay(GroundCheck.position+Offset,-GroundCheck.up,Color.blue,5);
        
        if (RayHit.collider==null)
        {
            return;
        }
        
        if (RayHit.collider.transform.tag=="Ground")
        {
         
            angle= Vector3.Angle(RayHit.normal, Vector3.forward);
       
            AngleDiff = new Vector3(-1 * (angle-90), 0, 0);
      
            transform.position = RayHit.point;
            transform.rotation = Quaternion.Euler(AngleDiff);
    
        }
    }
    public void RagdollActive(bool activestatus)
    {
        if (activestatus)
        {
            PlayerAnim.SetTrigger("Dead");
            return;
            //StartCoroutine(ragdollactivedelayed(activestatus));
            
        }
        ragdollactivedelayed(activestatus);

        
        
    }

    public  void ragdollactivedelayed(bool activestatus)
    {
        //yield return new WaitForSeconds(waitSeconds);
        PlayerAnim.enabled = !activestatus;
        foreach (Rigidbody rb in RigRigidbodies)
        {
            rb.isKinematic = !activestatus;
            //rb.AddForce(Vector3.forward*5,ForceMode.Impulse);
        }
    }


}
