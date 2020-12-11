using System;
using Mirror;
using UnityEngine;

public class FootballGameManager : NetworkBehaviour
{
    private GameObject ball;
    private ScoreManager scoreManager;
    private TeamManager teamManager;
    
    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        teamManager = FindObjectOfType<TeamManager>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        GeneralNetworkManager.OnConnectionReadied += teamManager.AddPlayerToTeam;
        GeneralNetworkManager.OnClientDisconnected -= teamManager.RemovePlayer;
        scoreManager.OnScoreChanged += ResetGame;
        
        SpawnBall();
    }

    [Server]
    private void SpawnBall()
    {
        var ballPrefab = NetworkManager.singleton.spawnPrefabs.Find(prefab => prefab.name == "Football");
        ball = Instantiate(ballPrefab);
        NetworkServer.Spawn(ball);
    }

    [Server]
    public void ResetGame()
    {
        ball.transform.position = Vector3.zero;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        teamManager.ResetPlayers();
    }
}