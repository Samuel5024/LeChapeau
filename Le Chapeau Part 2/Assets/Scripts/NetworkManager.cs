using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance; //instance

    private void Awake()
    {                               
        if (instance != null && instance != this) //if an instance already exists
            gameObject.SetActive(false);          ////and it's not this one- destroy us
        else
        {
            instance = this; //set the instance
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start()
    {
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
                
    [PunRPC] //changes scene using Photon's system
             //this is an RPC because when the host starts the game,
             //they will tell everyone else in the room to call this function

    public void ChangeScene (string sceneName)
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
