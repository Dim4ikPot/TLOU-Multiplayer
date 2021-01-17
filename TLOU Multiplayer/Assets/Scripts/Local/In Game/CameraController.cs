using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook CMcam;
    public Transform camLookAt;
    public Transform allCamsPositions;
    PhotonView photonView;

    private void Awake()
    {
        CMcam = GameObject.Find("CinemachineFreeLook").GetComponent<CinemachineFreeLook>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        CMcam.m_Follow = camLookAt;
        CMcam.LookAt = camLookAt;
        camLookAt.localPosition = allCamsPositions.Find("Stand").localPosition;
    }

    public void SetCameraPosition(CharStatuses charStat, bool aiming)
    {
        string result;
        if (charStat == CharStatuses.Stand)
            result = "Stand";
        else //if (charStat == CharStatuses.Crouch)
            result = "Crouched";
        
        if (aiming)
            result += "_Aiming";

        float duration = 0.5f;
        if (result.IndexOf("Aiming") != 0)
            duration = 0.25f;

        camLookAt.DOLocalMove(allCamsPositions.Find(result).localPosition, duration);
    }

    public void SetCameraFoV(float zoom)
    {
        DOTween.To(() => CMcam.m_Lens.FieldOfView, x => CMcam.m_Lens.FieldOfView = x, zoom, 0.25f);
        //CMcam.m_Lens.FieldOfView = Mathf.Lerp(CMcam.m_Lens.FieldOfView, zoom, Time.deltaTime * 10f);
    }
}
