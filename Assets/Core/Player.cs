using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private PlayerColor playerColor;
    public NetworkConnection NetworkConnection;

    [SyncVar(hook = nameof(ChangeColor))]
    public Team Team;
    
    private void Awake()
    {
        playerColor = GetComponent<PlayerColor>();
    }

    public void SetTeam(Team team, NetworkConnection networkConnection)
    {
        Team = team;
        NetworkConnection = networkConnection;
    }

    public void ChangeColor(Team oldT, Team newT)
    {
        playerColor.CmdSetupColor(Team == Team.Blue ? Color.blue : Color.red);
    }

    [ClientRpc]
    public void RpcSetPlayerLocation(Vector3 getRandomSpawnLocation)
    {
        transform.position = getRandomSpawnLocation;
    }
}