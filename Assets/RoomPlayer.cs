using System;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

public class RoomPlayer : NetworkBehaviour
{
    public static event Action OnPlayerChanged;
    
    [SyncVar(hook = nameof(UpdateUi))]
    public string Name = "Random";

    [SyncVar(hook = nameof(UpdateUi))] 
    public bool IsReady;
    
    [SyncVar]
    public bool IsHost;
    private LobbyUi lobbyUi;

    private GeneralNetworkManager networkManager => (GeneralNetworkManager)NetworkManager.singleton;
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        lobbyUi = FindObjectOfType<LobbyUi>();
        lobbyUi.Initialize(this);

        CmdChangeName("Name " + Random.Range(0, 12345));
        
        Debug.Log($"{Name} {(IsHost ? "is host" : "isn't host")}");
    }

    public override void OnStartClient()
    {
        networkManager.RoomPlayers.Add(this);
        
        OnPlayerChanged?.Invoke();
    }

    public void UpdateUi(string oldName, string newName) => OnPlayerChanged?.Invoke();
    public void UpdateUi(bool oldValue, bool newValue) => OnPlayerChanged?.Invoke();

    public override void OnStopClient()
    {
        networkManager.RoomPlayers.Remove(this);
        OnPlayerChanged?.Invoke();
    }

    [Command]
    public void CmdChangeName(string newName)
    {
        Name = newName;
    }

    [Command]
    public void CmdSetReadyState(bool isReady)
    {
        IsReady = isReady;
    }

    [Command]
    public void CmdStartGame()
    {
        if (!IsHost)
        {
            Debug.Log("Tried to execute start game without being the host");
            return;
        }

        networkManager.StartGame();
    }
}