using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class Launcher : MonoBehaviourPunCallbacks
{
    public int roomSize;

    void Start()
    {
        print("Connecting to Master : In Progress!");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() //Callback function for when the first connection is established successfully.
    {
        print("Connecting to Master : SUCCESS!");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 100);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
        //PhotonNetwork.AutomaticallySyncScene = true; //Makes it so whatever scene the master client has loaded is the scene all other clients will load
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public void DelayStart() //Paired to the Delay Start button
    {
        PhotonNetwork.JoinRandomRoom(); //First tries to join an existing room
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom(); // if it fails to join a room then it will try to create its own
    }

    public void CreateRoom()
    {
        int randomRoomNumber = Random.Range(0, 10000); //creating a random name for the room
        /*Hashtable roomOption = new Hashtable();
        for (int i = 0; i < roomSize; i++)
        {
            roomOption.Add(i, 1); //ник, команда
        }*/

        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            //CustomRoomProperties = roomOption,
            MaxPlayers = (byte)roomSize
        });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom(); //Retrying to create a new room with a different name.
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }
}
