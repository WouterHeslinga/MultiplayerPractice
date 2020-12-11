using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    
    private static Dictionary<Team, List<Transform>> spawnPoints = new Dictionary<Team, List<Transform>>();

    public static void AddSpawnPoint(Team team, Transform transform)
    {
        if(!spawnPoints.ContainsKey(team))
            spawnPoints.Add(team, new List<Transform>());
        
        spawnPoints[team].Add(transform);
    }

    public static void RemoveSpawnPoint(Team team, Transform transform)
    {
        spawnPoints[team].Remove(transform);
    }

    public static Vector3 GetRandomSpawnLocation(Team team) =>
        spawnPoints[team][Random.Range(0, spawnPoints[team].Count)].position;

    [Server]
    public GameObject SpawnPlayer(NetworkConnection conn, Team team)
    {
        var spawnPos = GetRandomSpawnLocation(team);

        var player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        return player;
    }
}