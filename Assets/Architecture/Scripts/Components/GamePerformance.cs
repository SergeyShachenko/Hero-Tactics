namespace Components
{
    internal struct GamePerformance
    {
        public GameState State;
    }


    public enum GameState
    {
        Process, Win, GameOver
    }
}