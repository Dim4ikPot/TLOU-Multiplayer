using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Climbing : MonoBehaviour
{
    Statuses stats;
    Movement move;
    Animator anim;
    PhotonView photonView;
    void Start()
    {
        anim = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        stats = GetComponent<Statuses>();
        move = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Climb");
        }
    }
}
