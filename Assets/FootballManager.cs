using Mirror;
using UnityEngine;

public class FootballManager : NetworkBehaviour
{
    private GameObject ball;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
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
    public void ResetBall()
    {
        ball.transform.position = Vector3.zero;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}