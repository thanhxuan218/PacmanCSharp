using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PacmanWindowForms.Scripts.Models;
using Microsoft.VisualBasic.Logging;

namespace PacmanWindowForms.Scripts.Models_refactor
{
    public abstract class EntityBase
    {

        private EntityParams entityParams;
        private EntityType entityType;
        private List<Point> points = new List<Point>();
        public EntityBase(EntityType type, EntityParams entityParams)
        {
            this.entityType = type;
            this.entityParams = entityParams;
        }

        protected void SetEntityParams(EntityParams entityParams)
        {
            this.entityParams = entityParams;
        }

        public abstract void LoadNew();
        public EntityType GetEntityType()
        {
            return entityType;
        }

        protected void SetWidth(int width)
        {
            this.entityParams.width = width;
            this.UpdatePoints();
        }

        protected void SetHeight(int height)
        {
            this.entityParams.height = height;
            this.UpdatePoints();
        }

        public Point GetPosition()
        {
            return entityParams.position;
        }

        public void SetPosition(Point position)
        {
            GameBoard.Instance.RemoveEntityAt(this.entityType, this.entityParams.position);
            this.entityParams.position = position;
            GameBoard.Instance.AddEntityAt(this.entityType, this.entityParams.position);
            this.UpdatePoints();
        }

        public List<Point> GetPoints()
        {
            return points;
        }

        public Direction GetDirection()
        {
            return entityParams.direction;
        }

        public void SetDirection(Direction direction)
        {
            this.entityParams.direction = direction;
        }

        public int GetSpeed()
        {
            return entityParams.speed;
        }

        protected void SetSpeed(int speed)
        {
            this.entityParams.speed = speed;
        }

        protected void UpdatePoints()
        {
            points.Clear();
            for (int i = entityParams.position.X; i < entityParams.position.X + entityParams.width; i++)
            {
                for (int j = entityParams.position.Y; j < entityParams.position.Y + entityParams.height; j++)
                {
                    points.Add(new Point(i, j));
                }
            }
        }

        public abstract void StartMoving();

        public void Print()
        {
            Logger.Log("Entity type: " + entityType + " position: " + entityParams.position + " direction: " + entityParams.direction + " speed: " + entityParams.speed);
        }
    }

    public class Pacman : EntityBase
    {
        private static Pacman pacman = null;

        public static Pacman Instance
        {
            get
            {
                if (pacman == null)
                {
                    EntityParams pacmanParams = GameBoard.Instance.GetPacmanParams();
                    pacman = new Pacman(pacmanParams);
                }
                return pacman;
            }
        }

        public override void LoadNew()
        {
            this.SetEntityParams(GameBoard.Instance.GetPacmanParams());
        }

        private bool isDead = false;
        private Task movingTask = null;
        private int lives = 4;
        private DynamicEntityState state = DynamicEntityState.Normal;

        private Pacman(EntityParams entityParams)
        : base(EntityType.Pacman, entityParams)
        {
        }
        public override void StartMoving()
        {
            this.UpdatePoints();
            // Moving task is running
            if (movingTask != null && !movingTask.IsCompleted)
            {
                return;
            }
            movingTask = Task.Run(() =>
            {
                while (true)
                {
                    if (this.GetDirection() == Direction.None)
                    {
                    }

                    // TODO: Add logic to check current game state
                    // If game state is not running, then stop moving
                    // if (GameController.Instance.GetGameState() != GameState.Running)
                    // {
                    //     continue;
                    // }
                    Point currentPosition = this.GetPosition();
                    Point nextPosition = GameBoard.Instance.NextLocation(currentPosition, this.GetDirection());
                    if (GameBoard.Instance.IsPossibleLocation(nextPosition))
                    {
                        this.SetPosition(nextPosition);
                    }
                    if (currentPosition != this.GetPosition())
                    {
                        this.NotifyPacmanMoving();
                    }
                    System.Threading.Thread.Sleep(this.GetSpeed());
                }
            });
        }

        public void SetLives(int lives)
        {
            this.lives = lives;
        }
        public int GetLives()
        {
            return this.lives;
        }

        public void SetState(DynamicEntityState state)
        {
            lock (this)
            {
                this.state = state;
            }
        }

        public DynamicEntityState GetState()
        {
            lock (this)
            {
                return this.state;
            }
        }

        private void NotifyPacmanDead()
        {
        }

        private void NotifyPacmanMoving()
        {
            // TODO: Notify to collision controller, so that it can handle collision
        }
    }

    public class Ghost : EntityBase
    {
        private Task movingTask = null;
        private GhostColor color;

        public Ghost(EntityParams entityParams, GhostColor color)
        : base(EntityType.Ghost, entityParams)
        {
            this.color = color;
        }

        public override void LoadNew()
        {
            this.SetEntityParams(GameBoard.Instance.GetGhostParams()[color]);
        }

        private Direction FindNewPossibleDirection()
        {
            List<Direction> possibleDirections = new List<Direction>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction == Direction.None)
                {
                    continue;
                }
                Point nextPosition = GameBoard.Instance.NextLocation(this.GetPosition(), direction);
                if (GameBoard.Instance.IsPossibleLocation(nextPosition))
                {
                    possibleDirections.Add(direction);
                }
            }
            if (possibleDirections.Count == 0)
            {
                return Direction.None;
            }
            Random random = new Random();
            return possibleDirections[random.Next(possibleDirections.Count)];
        }

        public override void StartMoving()
        {
            this.UpdatePoints();
            // Moving task is running
            if (movingTask != null && !movingTask.IsCompleted)
            {
                return;
            }
            movingTask = Task.Run(() =>
            {
                while (true)
                {
                    if (this.GetDirection() == Direction.None)
                    {
                        continue;
                    }

                    // TODO: Add logic to check current game state
                    // If game state is not running, then stop moving
                    // if (GameController.Instance.GetGameState() != GameState.Running)
                    // {
                    //     continue;
                    // }
                    Point currentPosition = this.GetPosition();
                    Point nextPosition = GameBoard.Instance.NextLocation(currentPosition, this.GetDirection());
                    if (GameBoard.Instance.IsPossibleLocation(nextPosition))
                    {
                        this.SetPosition(nextPosition);
                        // Logger.Log("Ghost is moving to curr: " + this.GetPosition() + " next: " + nextPosition + "curr dir: " + this.GetDirection());
                    }
                    else
                    {
                        this.SetDirection(FindNewPossibleDirection());
                        continue;
                    }

                    if (currentPosition != this.GetPosition())
                    {
                        // Logger.Log("Ghost is moving to position: " + this.GetPosition());
                        this.NotifyGhostMoving();
                    }
                    System.Threading.Thread.Sleep(this.GetSpeed());
                }
            });
            Logger.Log("Ghost StartMoving");
        }

        public GhostColor GetColor()
        {
            return this.color;
        }



        private void NotifyGhostMoving()
        {
            // TODO: Notify to collision controller, so that it can handle collision

        }
    }

    public class GhostFactory
    {
        private static GhostFactory ghostFactory = null;
        private GhostFactory()
        {
        }

        public static GhostFactory Instance
        {
            get
            {
                if (ghostFactory == null)
                {
                    ghostFactory = new GhostFactory();
                    GhostFactory.Instance.LoadGhosts();
                }
                return ghostFactory;
            }
        }

        Dictionary<GhostColor, Ghost> ghosts = new Dictionary<GhostColor, Ghost>();

        public void LoadGhosts()
        {
            Dictionary<GhostColor, EntityParams> ghostParams = GameBoard.Instance.GetGhostParams();

            if (ghostParams == null || ghostParams.Count == 0)
            {
                return;
            }

            foreach (GhostColor color in ghostParams.Keys)
            {
                ghosts[color] = new Ghost(ghostParams[color], color);
            }
            Logger.Log($"GhostFactory LoadGhosts {ghosts.Count}");
        }

        public Direction GetDirection(GhostColor color)
        {
            if (ghosts.ContainsKey(color))
            {
                return ghosts[color].GetDirection();
            }
            return Direction.None;
        }

        public void Move(GhostColor color, Direction direction)
        {
            if (ghosts.ContainsKey(color))
            {
                ghosts[color].SetDirection(direction);
            }
        }

        public Ghost GetGhost(GhostColor color)
        {
            if (ghosts.ContainsKey(color))
            {
                return ghosts[color];
            }
            return null;
        }

        public void StartMoving()
        {
            foreach (GhostColor color in ghosts.Keys)
            {
                ghosts[color].StartMoving();
            }
        }

        public void SetDirection(GhostColor color, Direction direction)
        {
            if (ghosts.ContainsKey(color))
            {
                ghosts[color].SetDirection(direction);
            }
        }

        public List<Point> GetPoints(GhostColor color)
        {
            if (ghosts.ContainsKey(color))
            {
                return ghosts[color].GetPoints();
            }
            return null;
        }

        public List<Point> GetAllPoints()
        {
            List<Point> points = new List<Point>();
            foreach (GhostColor color in ghosts.Keys)
            {
                points.AddRange(ghosts[color].GetPoints());
                // Logger.Log(" GetAllPoints Ghost position: " + ghosts[color].GetPosition());
            }
            return points;
        }

        public void LoadNew()
        {
            foreach (GhostColor color in ghosts.Keys)
            {
                ghosts[color].LoadNew();
            }

            Logger.Log($"GhostFactory LoadNew {ghosts.Count}");
        }

        public Ghost GetGhostByPosition(Point position)
        {
            foreach (GhostColor color in ghosts.Keys)
            {
                if (ghosts[color].GetPosition() == position)
                {
                    return ghosts[color];
                }
            }
            return null;
        }

        public Ghost GetGhostByPoints(List<Point> points)
        {
            foreach (GhostColor color in ghosts.Keys)
            {
                if (ghosts[color].GetPoints().Intersect(points).ToList().Count > 0)
                {
                    return ghosts[color];
                }
            }
            return null;
        }

        public void Print()
        {
            foreach (GhostColor color in ghosts.Keys)
            {
                ghosts[color].Print();
            }
        }
    }

}

