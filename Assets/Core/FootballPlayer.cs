using Mirror;
using UnityEngine;

public class FootballPlayer : NetworkBehaviour
{
    public GamePlayer Owner;

    private PlayerColor playerColor;

    private void Awake()
    {
        playerColor = GetComponent<PlayerColor>();
    }

    public void Initialize(GamePlayer newOwner)
    {
        Owner = newOwner;
        
        ChangeColor(newOwner.Team);
    }

    public void ChangeColor(Team newTeam)
    {
        if(!hasAuthority) 
            return;
        
        playerColor.CmdSetupColor(Owner.Team == Team.Blue ? Color.blue : Color.red);
    }

    [ClientRpc]
    public void RpcSetPlayerLocation(Vector3 getRandomSpawnLocation)
    {
        transform.position = getRandomSpawnLocation;
    }
}