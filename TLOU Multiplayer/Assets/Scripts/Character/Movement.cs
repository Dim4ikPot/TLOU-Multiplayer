using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Statuses))]
public class Movement : MonoBehaviour
{
    Animator anim;
    PhotonView myPhotonView;
    Statuses stats;
    CharacterController cc;
    CharacterObjects charObjects;

    Vector3 rootMotion;
    Vector3 velocity;
    bool isFalling;

    public float jumpHeight;
    public float gravity;
    public float stepDown;
    public float groundSpeed;

    private float _stepDown;

    void Start()
    {
        _stepDown = stepDown;
        myPhotonView = GetComponent<PhotonView>();
        charObjects = GetComponent<CharacterObjects>();
        anim = GetComponent<Animator>();
        stats = GetComponent<Statuses>();
        cc = GetComponent<CharacterController>();
        stats.ChangeCCGravitySettingCallback += ChangeStepDown;
    }



    void Update()
    {
        if (!myPhotonView.IsMine)
              return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        anim.SetFloat("Forward", v);
        anim.SetFloat("Right", h);

        //if (Input.GetKeyDown(KeyCode.Space))
            //Jump();
        
        float movementVelocity = Mathf.Abs(h) + Mathf.Abs(v);
        bool check = movementVelocity > 0.1 || stats.aiming;
        if (check)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
    }

    private void OnAnimatorMove()
    {
        rootMotion += anim.deltaPosition;
    }

    private void FixedUpdate()
    {
        if (!myPhotonView.IsMine)
            return;

        if (isFalling && !stats.climbing)
            UpdateInAir();
        else
            UpdateOnGround();
    }

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * _stepDown;

        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!cc.isGrounded)
            SetInAir(0);
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        cc.Move(displacement);
        isFalling = !cc.isGrounded;
        rootMotion = Vector3.zero;
    }

    void Jump()
    {
        if (!isFalling)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isFalling = true;
        velocity = anim.velocity * groundSpeed;
        velocity.y = jumpVelocity;
    }

    void ChangeStepDown(bool flag)
    {
        _stepDown = flag ? 0 : stepDown;
    }
}
