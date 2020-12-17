using System;
using Mirror;

public class GamePlayer : NetworkBehaviour
{
    [SyncVar]
    public string Name;
    [SyncVar]
    public Team Team;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Server]
    public void SetTeam(Team team)
    {
        Team = team;
    }
    
    [Server]
    public void SetDisplayName(string newName)
    {
        Name = newName;
    }
}