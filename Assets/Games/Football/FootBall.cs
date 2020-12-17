using System;
using Mirror;
using UnityEngine;

public class FootBall : NetworkBehaviour
{
    public FootballPlayer LastPlayer;
    
    [Server]
    public void AddForce(GameObject shooter)
    {
        if (!shooter.TryGetComponent<FootballPlayer>(out var player))
            return;

        LastPlayer = player;
        var dir = shooter.transform.position - transform.position;
        
        GetComponent<Rigidbody2D>().AddForce(-dir.normalized * 7500);
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent<FootballPlayer>(out var player))
            return;

        LastPlayer = player;
        
        var direction = player.transform.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(-direction.normalized * 40);
    }
}