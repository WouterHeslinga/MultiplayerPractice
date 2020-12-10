using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GeneralNetworkManager : NetworkManager
{
    public static event Action<NetworkConnection> OnClientConnected;
    public static event Action<NetworkConnection> OnClientDisconnected;
    public static event Action<NetworkConnection> OnConnectionReadied;
    public static event Action OnServerStopped;

    public override void OnStopServer()
    {
        base.OnStopServer();
        
        OnServerStopped?.Invoke();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
        OnClientConnected?.Invoke(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        
        OnClientDisconnected?.Invoke(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        
        OnConnectionReadied?.Invoke(conn);
    }
}
