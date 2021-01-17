using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public enum CharStatuses
{
    Stand,
    Crouch,
    Crafting,
    Down,
    Death
}

public class Statuses : MonoBehaviourPunCallbacks, IPunObservable
{
    Animator anim;
    CharacterController cc;
    CharacterObjects charObjects;
    public CameraController camController;
    public PhotonView myPhotonView;
    private int _hp; 
    public int HP { get => _hp; set { if (value < 0) value = 0; } }
    public CharStatuses charStatuses;
    //public bool crouched;
    public bool aiming;
    public bool climbing;

    public delegate void AimingDelegate(bool flag);
    public AimingDelegate AimingCallback;

    public delegate void ChangeCameraFov(bool flag);
    public ChangeCameraFov ChangeCameraFovCallback;

    /*public delegate void ChangeCameraPosition(string status);
    public ChangeCameraPosition ChangeCameraPositionCallback;*/

    public delegate void ChangeCCGravitySetting(bool flag);
    public ChangeCCGravitySetting ChangeCCGravitySettingCallback;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        camController = GetComponent<CameraController>();
        charObjects = GetComponent<CharacterObjects>();
        myPhotonView = GetComponent<PhotonView>();
        if (PhotonNetwork.LocalPlayer.IsLocal)
            SetLocalObjects();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        //crouched = false;
        aiming = false;
        climbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myPhotonView.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (charStatuses == CharStatuses.Stand)
                ChangeCrouchStatus(true);
            else if (charStatuses == CharStatuses.Crouch)
                ChangeCrouchStatus(false);
        }
            

        if (Input.GetMouseButtonDown(1))
            ChangeAimingStatus(true);
        else if (Input.GetMouseButtonUp(1))
            ChangeAimingStatus(false);

    }

    public void ChangeCrouchStatus(bool flag)
    {
        //crouched = flag;
        if (flag)
            charStatuses = CharStatuses.Crouch;
        else
            charStatuses = CharStatuses.Stand;
        anim.SetBool("Crouched", flag);
        /*if (crouched)
        {
            cc.height = 1f;
            cc.center = Vector3.up * 0.9f;
        }
        else
        {
            cc.height = 1.7f;
            cc.center = Vector3.up * 0.9f;
        }*/
        //ChangeCameraPositionCallback.Invoke(CameraChangeTag());
        camController.SetCameraPosition(charStatuses, aiming);
    }

    public void ChangeAimingStatus(bool flag)
    {
        aiming = flag;
        anim.SetBool("Aiming", aiming);
        AimingCallback.Invoke(flag);
    }

    public void ChangeClimbingStatus(int i)
    {
        bool flag = i == 1;
        climbing = flag;
        ChangeCCGravitySettingCallback.Invoke(flag);
    }


    ////////////////////////////////////////////
    ///

    private void SetLocalObjects()
    {
        charObjects.weaponDefault = transform.Find("WeaponDefault").GetComponent<Rig>();
        charObjects.weaponAiming = transform.Find("WeaponAiming").GetComponent<Rig>();
        charObjects.bodyAiming = transform.Find("BodyAiming").GetComponent<Rig>();
        charObjects.handsIK = transform.Find("HandsIK").GetComponent<Rig>();
        charObjects.multiAimConstraint = transform.Find("WeaponAiming").GetChild(0).GetComponent<MultiAimConstraint>();
    }

    #region IPunObservable Implementaion
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(aiming);
            stream.SendNext(climbing);
            stream.SendNext(charStatuses);
        }
        else
        {
            // Network player, receive data
            this.aiming = (bool)stream.ReceiveNext();
            this.climbing = (bool)stream.ReceiveNext();
            this.charStatuses = (CharStatuses)stream.ReceiveNext();
        }
    }
    #endregion
}
