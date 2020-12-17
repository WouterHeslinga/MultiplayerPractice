using System.Collections.Generic;
using System.Linq;
using Mirror;
using Random = UnityEngine.Random;

public class TeamManager : NetworkBehaviour
{
    private PlayerSpawnSystem spawnSystem;

    private Dictionary<Team, List<PlayerContainer>> teams;

    public override void OnStartServer()
    {
        base.OnStartServer();

        teams = new Dictionary<Team, List<PlayerContainer>>
        {
            {Team.Red, new List<PlayerContainer>()},
            {Team.Blue, new List<PlayerContainer>()}
        };
        
        spawnSystem = FindObjectOfType<PlayerSpawnSystem>();
    }

    [Server]
    public void RemovePlayer(NetworkConnection connection)
    {
        var leftPlayer = connection.identity.gameObject.GetComponent<GamePlayer>();

        teams[leftPlayer.Team].Remove(teams[leftPlayer.Team].First(cont => cont.GamePlayer == leftPlayer));
        
        NetworkServer.Destroy(leftPlayer.gameObject);
    }

    [Server]
    public void AddPlayerToTeam(NetworkConnection connection)
    {
        var minSizeTeam = teams.Min(t => t.Value.Count);
        var lowPlayerTeams = teams.Where(t => t.Value.Count == minSizeTeam).ToList();
        var chosenTeam = lowPlayerTeams[Random.Range(0, lowPlayerTeams.Count)].Key;

        var playerObj = spawnSystem.SpawnPlayer(connection, chosenTeam);
        var player = playerObj.GetComponent<FootballPlayer>();
        var owner = connection.identity.GetComponent<GamePlayer>();
        owner.SetTeam(chosenTeam);
        
        player.Initialize(owner);
        teams[chosenTeam].Add(new PlayerContainer(owner, player));
    }

    [Server]
    public void ResetPlayers()
    {
        foreach (var team in teams)
        {
            foreach (var player in team.Value)
            {
                player.FootballPlayer.RpcSetPlayerLocation(PlayerSpawnSystem.GetRandomSpawnLocation(player.GamePlayer.Team));
            }
        }
    }
}