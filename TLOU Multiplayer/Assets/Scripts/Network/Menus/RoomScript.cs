using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomScript : MonoBehaviourPunCallbacks
{
    public GameObject playerListObject;
    public GameObject bluePlayersListObject;
    public GameObject yellowPlayersListObject;
    public GameObject startButton;
    public int mapSceneIndex;

    PhotonView myPhotonView;

    TextMeshProUGUI playerListTitle;
    TextMeshProUGUI playerListNicknames;

    TextMeshProUGUI bluePlayerListTitle;
    TextMeshProUGUI bluePlayerListNicknames;

    TextMeshProUGUI yellowPlayerListTitle;
    TextMeshProUGUI yellowPlayerListNicknames;

    RoundManager roundManager;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();

        playerListTitle = playerListObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        playerListNicknames = playerListObject.transform.Find("Nicknames").GetComponent<TextMeshProUGUI>();

        bluePlayerListTitle = bluePlayersListObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        bluePlayerListNicknames = bluePlayersListObject.transform.Find("Nicknames").GetComponent<TextMeshProUGUI>();

        yellowPlayerListTitle = yellowPlayersListObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        yellowPlayerListNicknames = yellowPlayersListObject.transform.Find("Nicknames").GetComponent<TextMeshProUGUI>();

        roundManager = GameObject.Find("NetworkRoundManager").GetComponent<RoundManager>();

        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("CheckPlayers", RpcTarget.AllBuffered);
        else
            startButton.SetActive(false);
    }

    [PunRPC]
    void CheckPlayers()
    {
        playerListTitle.text = "Players : " + PhotonNetwork.PlayerList.Length + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        playerListNicknames.text = "";

        foreach (Player player in PhotonNetwork.PlayerList)
            if (player == PhotonNetwork.LocalPlayer)
                playerListNicknames.text += "<b><color=\"red\">" + player.NickName + "</color></b><br>";
            else
                playerListNicknames.text += player.NickName + "<br>";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //called whenever a new player joins the room
        print("connecting : " + newPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("CheckPlayers", RpcTarget.AllBuffered);
        //AddPlayer(firstTeamPlayers.Count < secondTeamPlayers.Count ? 1 : 2, newPlayer);
    }

    /*void AddPlayer(int teamNumber, Player newPlayer)
    {
        if (teamNumber == 1)
            firstTeamPlayers.Add(newPlayer);
        else
            secondTeamPlayers.Add(newPlayer);
        //PhotonNetwork.CurrentRoom.CustomProperties.Add(newPlayer.NickName, teamNumber);
    }*/

    /*void SetUI()
    {
        hunterPlayer.text = "";
        militaryPlayer.text = "";
        foreach (DictionaryEntry entry in PhotonNetwork.CurrentRoom.CustomProperties)
        {
            print(entry.Key + " : " + entry.Value);
            if (entry.Equals(1))
                hunterPlayer.text += (string)entry.Key + "<br>";
            else
                militaryPlayer.text += (string)entry.Key + "<br>";
        }
    }*/

    public void StartGame()
    {
        int i = 0;
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            i++;
            int teamNumber;
            if (i % 2 == 0)
                teamNumber = 2;
            else
                teamNumber = 1;
            myPhotonView.RPC("SetTeamsInfo", RpcTarget.AllBuffered, teamNumber, player);
        }

        myPhotonView.RPC("SetTeamsInfoUI", RpcTarget.AllBuffered);
        
        if (PhotonNetwork.IsMasterClient)
          PhotonNetwork.LoadLevel(mapSceneIndex);
    }

    [PunRPC]
    void SetTeamsInfo(int teamNumber, Player player)
    {
        if (teamNumber == 1)
            roundManager.firstTeam.Add(player);
        else
            roundManager.secondTeam.Add(player);
    }

    [PunRPC]
    void SetTeamsInfoUI()
    {
        playerListObject.SetActive(false);

        int maxTeamPlayerCnt = PhotonNetwork.CurrentRoom.MaxPlayers / 2;

        bluePlayerListTitle.text = "Blue : " + roundManager.firstTeam.Count + " / " + maxTeamPlayerCnt;
        yellowPlayerListTitle.text = "Yellow : " + roundManager.secondTeam.Count + " / " + maxTeamPlayerCnt;
        bluePlayerListNicknames.text = "";
        yellowPlayerListNicknames.text = "";

        foreach (Player player in roundManager.firstTeam)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                bluePlayerListNicknames.text += "<b><color=\"red\">" + player.NickName + "</color></b><br>";
                roundManager.myTeamNumber = 1;
            }
            else
            {
                bluePlayerListNicknames.text += player.NickName + "<br>";
            }
        }

        foreach (Player player in roundManager.secondTeam)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                yellowPlayerListNicknames.text += "<b><color=\"red\">" + player.NickName + "</color></b><br>";
                roundManager.myTeamNumber = 2;
            }
            else
            {
                yellowPlayerListNicknames.text += player.NickName + "<br>";
            }
        }
        
        bluePlayersListObject.SetActive(true);
        yellowPlayersListObject.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        print("connecting!");
    }
}
