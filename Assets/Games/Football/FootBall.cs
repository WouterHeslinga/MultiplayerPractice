using System;
using Mirror;
using UnityEngine;

public class FootBall : NetworkBehaviour
{
    [Server]
    public void AddForce(Vector3 direction)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * 500);
    }

    [Server]
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent<Player>(out var player))
            return;
    
        var direction = player.transform.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(-direction * 50);
    }
}