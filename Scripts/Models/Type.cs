namespace PacmanWindowForms
{
    public enum GameState : byte { GameOver = 0, GameOn = 1, GamePaused = 2, GameWon = 3 }
    public enum Direction : byte { Up = 0, Down = 1, Left = 2, Right = 3, None = 4 }
    public enum GhostColor : byte { Red = 0, Pink = 1, Yellow = 2, Blue = 3, None = 4 }
    public enum GhostState : byte { Normal = 0, Bonus = 1, Eaten = 2, BonusEnd = 3 }
    public enum PacmanState : byte { Normal = 0, Bonus = 1, Dies = 2 }
}