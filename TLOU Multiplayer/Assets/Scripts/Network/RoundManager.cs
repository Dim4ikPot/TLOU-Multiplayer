using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoundManager : MonoBehaviourPunCallbacks
{
    public int myTeamNumber;
    public List<Player> firstTeam = new List<Player>();
    public List<Player> secondTeam = new List<Player>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /*void OnPlayerConnected( player)
    {
        if (firstTeam.Count >= secondTeam.Count)
            firstTeam.Add(player)
    }*/


    // Update is called once per frame
    void Update()
    {
        
    }
}
