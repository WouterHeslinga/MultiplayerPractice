using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeamManager : NetworkBehaviour
{
    private PlayerSpawnSystem spawnSystem;

    private Dictionary<Team, List<Player>> teams;

    public override void OnStartServer()
    {
        base.OnStartServer();

        teams = new Dictionary<Team, List<Player>>
        {
            {Team.Red, new List<Player>()},
            {Team.Blue, new List<Player>()}
        };
        
        spawnSystem = FindObjectOfType<PlayerSpawnSystem>();

        GeneralNetworkManager.OnConnectionReadied += AddPlayerToTeam;
        GeneralNetworkManager.OnClientDisconnected -= RemovePlayer;
    }

    [Server]
    private void RemovePlayer(NetworkConnection connection)
    {
        var leftPlayer = connection.identity.gameObject.GetComponent<Player>();

        teams[leftPlayer.Team].Remove(leftPlayer);
        
        NetworkServer.Destroy(leftPlayer.gameObject);
        teams[leftPlayer.Team].Remove(leftPlayer);
    }

    [Server]
    private void AddPlayerToTeam(NetworkConnection connection)
    {
        var minSizeTeam = teams.Min(t => t.Value.Count);
        var lowPlayerTeams = teams.Where(t => t.Value.Count == minSizeTeam).ToList();
        var chosenTeam = lowPlayerTeams[Random.Range(0, lowPlayerTeams.Count)].Key;

        var playerObj = spawnSystem.SpawnPlayer(connection, chosenTeam);
        var player = playerObj.GetComponent<Player>();
        player.SetTeam(chosenTeam, connection);
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