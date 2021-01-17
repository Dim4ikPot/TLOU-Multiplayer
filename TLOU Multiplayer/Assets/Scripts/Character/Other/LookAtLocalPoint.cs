using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LookAtLocalPoint : MonoBehaviour
{
    Camera cam;
    PhotonView photonView;

    void Start()
    {
        cam = Camera.main;
        photonView = GetComponent<PhotonView>();
    }


    void Update()
    {
        if (photonView.IsMine)
            transform.position = cam.transform.position + cam.transform.forward.normalized * 7f;
    }
}
