namespace PacmanWindowForms.Scripts.Models
{
    public enum EntityType { Pacman, Ghost, Wall, Dot, Border, Energy, None }
    public enum GameState { Init, Running, Paused, GameOver, Win }
    public enum DynamicEntityState { Normal, Special, Dead, Respawn }
    public enum StaticEntityState { Normal, Eaten, None }
    public enum Direction { Up, Down, Left, Right, None }
    public enum GhostColor { Red, Blue, Yellow, Pink, None }
}