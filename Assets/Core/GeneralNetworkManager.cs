using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralNetworkManager : NetworkManager
{
    [SerializeField] private GameObject gamePlayerPrefab;
    
    public List<RoomPlayer> RoomPlayers = new List<RoomPlayer>();
    
    public static event Action<NetworkConnection> OnClientConnected;
    public static event Action<NetworkConnection> OnClientDisconnected;
    public static event Action<NetworkConnection> OnConnectionReadied;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started");
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
        
        base.OnStopServer();
    }

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
        Debug.Log("Server Change scene");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Client connected");
        OnClientConnected?.Invoke(conn);
        
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        
        Debug.Log("Client disconnected");
        OnClientDisconnected?.Invoke(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            var player = Instantiate(playerPrefab).GetComponent<RoomPlayer>();
            player.IsHost = RoomPlayers.Count == 0;
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        }
        
        Debug.Log("Client readied");
        OnConnectionReadied?.Invoke(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public void StartGame()
    {
        if(!IsReadyToStart)
            return;
        
        ServerChangeScene("MainScene");
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
        {
            var conn = RoomPlayers[i].connectionToClient;
            var gamePlayer = Instantiate(gamePlayerPrefab);
            
            //NetworkServer.Destroy(conn.identity.gameObject);
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
        }
    }

    private bool IsReadyToStart => RoomPlayers.TrueForAll(player => player.IsReady);
}
