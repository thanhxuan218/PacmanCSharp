using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PacmanWindowForms.Scripts.Models
{
    public enum EntityType { Pacman, Ghost, Wall, Dot, Border, Energy, None }
    public enum GameState { Init, Playing, Paused, GameOver, Win }
    public enum DynamicEntityState { Normal, Special, Dead, Respawn }
    public enum StaticEntityState { Normal, Eaten, None }
    public enum Direction { Up, Down, Left, Right, None }
    public enum GhostColor { Red, Blue, Yellow, Pink, None }

    // Base class for all entities
    public abstract class Entity
    {
        // List of points for drawing the entity and collision detection
        // For dynamic entities, this list contains points as perimeter of the entity
        // For static entities, this list contains points as every location of the entity
        // For example, a wall is a static entity, so the list contains points as every location of the wall
        public List<Point> points = new List<Point>();
        public EntityType entityType { get; set; }
        public Point position { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public bool isPointsChanged = true;

        public Entity(EntityType type, Point position, int width, int height)
        {
            this.entityType = type;
            this.position = position;
            this.width = width;
            this.height = height;
        }

        // Method for drawing the entity on the screen
        // This method is abstract because each entity has its own way of drawing
        public abstract void Draw();

        // Method for changing the entity's state
        // This method is abstract because each entity has its own way of changing state
        public abstract void ChangeState();

        // Method for checking collision between two entities
        // This method is abstract because each entity has its own way of checking collision
        public abstract bool CheckCollision(Entity entity);

        public List<Point> GetPoints()
        {
            return points;
        }

        public void SetPoints(List<Point> points)
        {
            this.points = points;
        }

        public void SetPoints(Point[] points)
        {
            this.points = points.ToList();
        }

        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        public void SetIsPointsChanged(bool isPointsChanged)
        {
            this.isPointsChanged = isPointsChanged;
        }
    }

    // Base class for all dynamic entities
    // Dynamic entities are entities that can move
    // Pacman and ghosts are dynamic entities
    public abstract class DynamicEntity : Entity
    {
        public Direction direction { get; set; }
        public int speed { get; set; }
        public int lives { get; set; }
        public DynamicEntityState state { get; set; }

        public DynamicEntity(EntityType type, Point position, int width, int height, Direction direction, int speed, int lives)
        : base(type, position, width, height)
        {
            this.direction = direction;
            this.speed = speed;
            this.lives = lives;
            this.state = DynamicEntityState.Normal;
            this.UpdatePoints();
        }

        // Method for moving the entity
        // This method is abstract because each entity has its own way of moving
        public abstract void Move();

        // Method for update all points of the entity
        // Position of the entity is the top left corner of the entity
        private void UpdatePoints()
        {
            this.points.Clear();
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    this.points.Add(new Point(this.position.X + i, this.position.Y + j));
                }
            }
        }
        public void SetDirection(Direction direction)
        {
            this.direction = direction;
        }

        public Direction GetDirection()
        {
            return this.direction;
        }

        public void SetSpeed(int speed)
        {
            this.speed = speed;
        }

        public int GetSpeed()
        {
            return this.speed;
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
            this.state = state;
        }

        public DynamicEntityState GetState()
        {
            return this.state;
        }

        public abstract void Reset();
        public void SetPosition(Point position)
        {
            this.position = position;
            this.UpdatePoints();
        }

        public Point GetPosition()
        {
            return this.position;
        }
    }

    // Base class for all static entities
    // Static entities are entities that cannot move
    // Walls, dots and bonuses are static entities
    public abstract class StaticEntity : Entity
    {
        // For static entities, 
        public StaticEntityState state { get; set; }
        public StaticEntity(EntityType type, int width, int height)
        : base(type, new Point(0, 0), width, height)
        {
            this.state = StaticEntityState.Normal;
        }
    }
}
