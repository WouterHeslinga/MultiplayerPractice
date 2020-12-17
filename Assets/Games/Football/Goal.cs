using Mirror;
using UnityEngine;

public class Goal : NetworkBehaviour
{
    public Team Team;

    [Server]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.TryGetComponent<FootBall>(out var ball))
            return;
        
        Debug.Log($"{ball.LastPlayer.Owner.Name} scored {(ball.LastPlayer.Owner.Team == Team ? "an own goal!" : "a goal!")}");
        
        AddScoreForEnemyTeam();
    }

    [Server]
    private void AddScoreForEnemyTeam()
    {
        var otherTeam = Team == Team.Blue ? Team.Red : Team.Blue;
        
        FindObjectOfType<ScoreManager>().AddScore(otherTeam);
    }
}