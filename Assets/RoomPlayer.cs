using UnityEngine;
using Mirror;

public class RoomPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateUi))]
    public string Name = "Random";

    public bool IsHost;
    private LobbyUi lobbyUi;

    private GeneralNetworkManager networkManager => (GeneralNetworkManager)NetworkManager.singleton;
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        lobbyUi = FindObjectOfType<LobbyUi>();

        CmdChangeName("Name " + Random.Range(0, 12345));
        
        Debug.Log($"{Name} {(IsHost ? "is host" : "isn't host")}");
    }

    public override void OnStartClient()
    {
        lobbyUi = FindObjectOfType<LobbyUi>();

        networkManager.RoomPlayers.Add(this);
        
        lobbyUi.Test();
    }

    public void UpdateUi(string oldName, string newName)
    {
        lobbyUi.Test();
    }

    public override void OnStopClient()
    {
        networkManager.RoomPlayers.Remove(this);
    }

    [Command]
    public void CmdChangeName(string newName)
    {
        Name = newName;
    }
}