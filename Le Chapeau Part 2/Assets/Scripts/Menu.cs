using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks //"PunCallbacks" necessary to get Photon callbacks
{
    [Header("Screens")]
    public GameObject mainScreen; //reference each screen for variables
    public GameObject lobbyScreen;

    [Header("Main Screen")]
    public Button createRoomButton; //variables referencing thigns we
    public Button joinRoomButton;   //need on the main screen

    [Header("Lobby Screen")]
    public TextMeshProUGUI playerListText;
    public Button startGameButton;

    void Start()
    {
        createRoomButton.interactable = false; //deactive main screen buttons
        joinRoomButton.interactable = false; 
    }
    
    public override void OnConnectedToMaster() //called when connected to master server
    {
        createRoomButton.interactable = true; //enable the buttons
        joinRoomButton.interactable = true;
    }

    void SetScreen (GameObject screen)
    {
        mainScreen.SetActive(false); //deactivate all screens
        lobbyScreen.SetActive(false);

        screen.SetActive(true); //enable requested screen
    }

    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnJoinRoomButton (TMP_InputField roomNameInput)
    {
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text; //update player name
    }

    public override void OnJoinedRoom()
    {   SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
                         //we don't RPC it like when we join lobby
        UpdateLobbyUI(); //becuase OnJoinRoom is only called for the client that just joined
                         //OnPlayerLeftRoom gets called for all clients in the room (we dont need to RPC)
    }

    [PunRPC] //called whenever someone joins/leaves the lobby
    public void UpdateLobbyUI()
    {
        playerListText.text = "";
        foreach (Player player in PhotonNetwork.PlayerList) //display all players currently in lobby
        {
            playerListText.text += player.NickName + "\n";
        }
        if (PhotonNetwork.IsMasterClient)
            startGameButton.interactable = true; //only host can start the game
        else
            startGameButton.interactable = false;
    }


    public void OnLeaveLobbyButton ()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    public void OnStartGameButton ()
    {
        NetworkManager.instance.photonView.RPC("Change Scene", RpcTarget.All, "Game");
    }
}
