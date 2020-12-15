using Mirror;
using UnityEngine;

public class GamePlayer : NetworkBehaviour
{
    private PlayerColor playerColor;

    [SyncVar(hook = nameof(ChangeColor))]
    public Team Team;
    
    private void Awake()
    {
        playerColor = GetComponent<PlayerColor>();
    }

    public void SetTeam(Team team)
    {
        Team = team;
    }

    public void ChangeColor(Team oldT, Team newT)
    {
        if(!isLocalPlayer) return;
        
        playerColor.CmdSetupColor(Team == Team.Blue ? Color.blue : Color.red);
    }

    [ClientRpc]
    public void RpcSetPlayerLocation(Vector3 getRandomSpawnLocation)
    {
        transform.position = getRandomSpawnLocation;
    }
}