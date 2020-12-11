using System;
using Mirror;

public class ScoreManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateScore))]
    public int RedScore;
    [SyncVar(hook = nameof(UpdateScore))]
    public int BlueScore;
    public event Action OnScoreChanged;

    public void AddScore(Team team)
    {
        switch (team)
        {
            case Team.Red:
                RedScore++;
                break;
            case Team.Blue:
                BlueScore++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(team), team, null);
        }
    }

    public void UpdateScore(int old, int n)
    {
        OnScoreChanged?.Invoke();
    }
}