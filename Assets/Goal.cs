using Mirror;
using UnityEngine;

public class Goal : NetworkBehaviour
{
    public Side Side;

    [Server]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.TryGetComponent<FootBall>(out var ball))
            return;
        
        FindObjectOfType<ScoreManager>().AddScore(Side);
    }
}