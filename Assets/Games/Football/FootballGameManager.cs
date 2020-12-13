using System.Collections.Generic;
using Mirror;
using Telepathy;
using UnityEngine;
using Random = UnityEngine.Random;

public class FootballGameManager : NetworkBehaviour
{
    private List<GameObject> balls = new List<GameObject>();
    private ScoreManager scoreManager;
    private TeamManager teamManager;

    [SyncVar] public float nextBallTimer;
    private static readonly int BallTimer = 30;

    private void Update()
    {
        if(!isServer)
            return;
        
        nextBallTimer -= Time.deltaTime;
        
        if(nextBallTimer > 0)
            return;
        
        SpawnBall();
        nextBallTimer = BallTimer;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        scoreManager = FindObjectOfType<ScoreManager>();
        teamManager = FindObjectOfType<TeamManager>();

        GeneralNetworkManager.OnConnectionReadied += teamManager.AddPlayerToTeam;
        GeneralNetworkManager.OnClientDisconnected -= teamManager.RemovePlayer;
        

        scoreManager.OnScoreChanged += ResetGame;

        SpawnBall();
        nextBallTimer = BallTimer;
    }

    [Server]
    private void SpawnBall()
    {
        var ballPrefab = NetworkManager.singleton.spawnPrefabs.Find(prefab => prefab.name == "Football");
        var ballPosition = balls.Count == 0 ? Vector3.zero : new Vector3(0, Random.Range(-4f, 4f), 0);

        var newBall = Instantiate(ballPrefab, ballPosition, Quaternion.identity);
        balls.Add(newBall);
        NetworkServer.Spawn(newBall);
    }

    [Server]
    public void ResetGame()
    {
        foreach (var ball in balls)
            NetworkServer.Destroy(ball);
        
        balls = new List<GameObject>();
        
        nextBallTimer = BallTimer;
        SpawnBall();
        teamManager.ResetPlayers();
    }
}