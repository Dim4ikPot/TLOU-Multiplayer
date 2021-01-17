using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    RoundManager roundManager;
    public Transform firstTeamStartSpawnPoint;
    public Transform secondTeamStartSpawnPoint;

    void Awake()
    {
        roundManager = GameObject.Find("NetworkRoundManager").GetComponent<RoundManager>();
        if (PhotonNetwork.LocalPlayer.IsLocal)
            if (roundManager.myTeamNumber == 1)
                PhotonNetwork.Instantiate("Prefabs/Network/Player", firstTeamStartSpawnPoint.position, firstTeamStartSpawnPoint.rotation);
            else
                PhotonNetwork.Instantiate("Prefabs/Network/Player", secondTeamStartSpawnPoint.position, secondTeamStartSpawnPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
