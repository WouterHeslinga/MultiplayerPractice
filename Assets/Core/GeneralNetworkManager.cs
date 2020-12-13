using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GeneralNetworkManager : NetworkManager
{
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
        base.OnClientConnect(conn);
        
        Debug.Log("Client connected");
        OnClientConnected?.Invoke(conn);
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
        
        var player = Instantiate(playerPrefab).GetComponent<RoomPlayer>();
        player.IsHost = RoomPlayers.Count == 0;

        NetworkServer.AddPlayerForConnection(conn, player.gameObject);
        
        Debug.Log("Client readied");
        OnConnectionReadied?.Invoke(conn);
    }
    
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<RoomPlayer>();
            RoomPlayers.Remove(player);
        }

        base.OnServerDisconnect(conn);
    }
}
