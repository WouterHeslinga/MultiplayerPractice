public class PlayerContainer
{
    public GamePlayer GamePlayer { get; private set; }
    public FootballPlayer FootballPlayer { get; private set; }

    public PlayerContainer(GamePlayer gamePlayer, FootballPlayer footballPlayer)
    {
        GamePlayer = gamePlayer;
        FootballPlayer = footballPlayer;
    }
}