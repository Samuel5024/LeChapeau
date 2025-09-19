using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance; //instance

    private void Awake()
    {    
        //check if an instance already exists and its not THIS one
        if(instance != null && instance != this)
        {
            Destroy(gameObject); //kill duplicate
            return;             
        }

        instance = this;
<<<<<<< Updated upstream
        DontDestroyOnLoad(gameObject); //persists across scenes
=======
        DontDestroyOnLoad(gameObject); 
>>>>>>> Stashed changes
    }
    
    private void Start()
    {
        //only connect to Photon if not already connected
        if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();

    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    //changes scene using Photon's system
    //this is an RPC because when the host starts the game,
    //they will tell everyone else in the room to call this function
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server"); //CreateRoom("testroom");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }
}
