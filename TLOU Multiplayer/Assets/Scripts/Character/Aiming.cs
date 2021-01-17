using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class Aiming : MonoBehaviour
{
    public float deaultZoom;
    public float aimingZoom;

    Statuses stats;
    CharacterObjects charObjects;

    void Start()
    {
        charObjects = GetComponent<CharacterObjects>();
        stats = GetComponent<Statuses>();
        if (/*stats.myPhotonView.IsMine*/PhotonNetwork.LocalPlayer.IsLocal)
        {
            stats.AimingCallback += AimingMethod;
        }
    }


    void Update()
    {

    }

    public void AimingMethod(bool flag)
    {
        stats.camController.SetCameraFoV(flag ? aimingZoom : deaultZoom);
        stats.camController.SetCameraPosition(stats.charStatuses, flag);
        stats.myPhotonView.RPC("ChangeAimingRig", RpcTarget.AllBuffered, flag);
    }

    [PunRPC]
    void ChangeAimingRig(bool flag)
    {
        DOTween.To(() => charObjects.weaponDefault.weight, x => charObjects.weaponDefault.weight = x, flag ? 0 : 1, 0.25f);
        DOTween.To(() => charObjects.weaponAiming.weight, x => charObjects.weaponAiming.weight = x, flag ? 1 : 0, 0.25f);
        DOTween.To(() => charObjects.rightHandIK.data.hintWeight, x => charObjects.rightHandIK.data.hintWeight = x, flag ? 0 : 1, 0.25f);
        DOTween.To(() => charObjects.bodyAiming.weight, x => charObjects.bodyAiming.weight = x, flag ? 1 : 0, 0.25f);
        //charObjects.bodyAiming.weight = Mathf.Clamp(Mathf.Lerp(charObjects.bodyAiming.weight, check ? 1 : 0, Time.deltaTime * 10f), 0, 1);
    }
}
