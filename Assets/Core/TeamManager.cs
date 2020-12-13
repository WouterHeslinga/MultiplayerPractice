using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeamManager : NetworkBehaviour
{
    private PlayerSpawnSystem spawnSystem;

    private Dictionary<Team, List<GamePlayer>> teams;

    public override void OnStartServer()
    {
        base.OnStartServer();

        teams = new Dictionary<Team, List<GamePlayer>>
        {
            {Team.Red, new List<GamePlayer>()},
            {Team.Blue, new List<GamePlayer>()}
        };
        
        spawnSystem = FindObjectOfType<PlayerSpawnSystem>();
    }

    [Server]
    public void RemovePlayer(NetworkConnection connection)
    {
        var leftPlayer = connection.identity.gameObject.GetComponent<GamePlayer>();

        teams[leftPlayer.Team].Remove(leftPlayer);
        
        NetworkServer.Destroy(leftPlayer.gameObject);
        teams[leftPlayer.Team].Remove(leftPlayer);
    }

    [Server]
    public void AddPlayerToTeam(NetworkConnection connection)
    {
        var minSizeTeam = teams.Min(t => t.Value.Count);
        var lowPlayerTeams = teams.Where(t => t.Value.Count == minSizeTeam).ToList();
        var chosenTeam = lowPlayerTeams[Random.Range(0, lowPlayerTeams.Count)].Key;

        var playerObj = spawnSystem.SpawnPlayer(connection, chosenTeam);
        var player = playerObj.GetComponent<GamePlayer>();
        player.SetTeam(chosenTeam);
        teams[chosenTeam].Add(player);
    }

    [Server]
    public void ResetPlayers()
    {
        foreach (var team in teams)
        {
            foreach (var player in team.Value)
            {
                player.RpcSetPlayerLocation(PlayerSpawnSystem.GetRandomSpawnLocation(player.Team));
            }
        }
    }
}