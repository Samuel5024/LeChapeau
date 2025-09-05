using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks //change inherited class
{
    //here are the variables
    [Header("Stats")]                 
    public bool gameEnded = false;      //has the game ended?
    public float timeToWin;             //time a player needs to hold the hat to win
    public float invincibleDuration;    //how llong afater a player gets the hat, are they invincible
    private float hatPickupTime;        //the time the hat was picked up by the current holder

    [Header("Players")]
    public string playerPrefabLocation; //path in resources folder to the Player prefab
    public Transform[] spawnPoints;
    public PlayerController[] players;   //array of al available spawn points
    public int playerWithHat;           //id of player with the hat
    private int playersInGame;          //number of players in the game

    //instance
    public static GameManager instance;

    void Awake()
    {
        //instance
        instance = this;
    }

    // Start is called before the first frame update
    void Start() //setup the players array
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.All);
    }

    //ImInGame gets called when a player joins the game
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;

        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    //SpawPlayer gets called when all players are in the game
    //spawns a player and initializes it
    void SpawnPlayer()
    {
        //instantiate the player across the network
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints 
            [Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        //get the player script
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();

        //initialize the player
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    //the following functions take in an id or game object, then returns the corresponding player

    public PlayerController GetPlayer(int playerId)
    {
        return players.First(x => x.id == playerId);
    }

    public PlayerController GetPlayer(GameObject playerObject)
    {
        return players.First(x => gameObject == playerObject);
    }

    //called when a player hits the hatted player - giving them the hat
    [PunRPC]
    public void GiveHat(int playerId, bool initialGive)
    {
        //remove the hat from the currently hatted player
        if (!initialGive)
            GetPlayer(playerWithHat).SetHat(false);

        //give the hat to the new player
        playerWithHat = playerId;
        GetPlayer(playerId).SetHat(true);
        hatPickupTime = Time.time;
    }

    //is the player able to take the hat at this current time?
    public bool CanGetHat()
    {
        if (Time.time > hatPickupTime + invincibleDuration)
            return true;
        else
            return false;
    }

    [PunRPC]
    void WinGame(int playerId)
    {
        gameEnded = true;
        PlayerController player = GetPlayer(playerId);
        //set the UI to show who's won

        Invoke("GoBackToMenu", 3.0f);
    }

    void GoBackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.ChangeScene("Menu");
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
