using System;
using Mirror;

public class ScoreManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateScore))]
    public int LeftScore;
    [SyncVar(hook = nameof(UpdateScore))]
    public int RightScore;
    public event Action OnScoreChanged;

    public void AddScore(Side side)
    {
        switch (side)
        {
            case Side.Left:
                RightScore++;
                break;
            case Side.Right:
                LeftScore++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(side), side, null);
        }

        FindObjectOfType<TeamManager>().ResetPlayers();
        FindObjectOfType<FootballManager>().ResetBall();
    }

    public void UpdateScore(int old, int n)
    {
        OnScoreChanged?.Invoke();
    }
}