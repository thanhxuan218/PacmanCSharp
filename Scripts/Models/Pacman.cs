using PacmanWindowForms.Scripts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWindowForms.Scripts.Models
{
    // Class for implementing Pacman
    public class Pacman : DynamicEntity
    {
        public Pacman(Point position, int width, int height, Direction direction, int speed, int lives)
        : base(EntityType.Pacman, position, width, height, direction, speed, lives)
        {
            this.state = DynamicEntityState.Normal;
        }

        public override void Draw()
        {
            // Draw the pacman on the screen using List<Point> points
            // For each point in points, draw a rectangle with width and height
            // The color of the rectangle is yellow
            // The pacman is drawn on the screen only when the game state differs from GameState.Init
            if (isPointsChanged)
            {
                Displayer.Instance.onRequestDisplay(new List<Point>() { position }, EntityType.Pacman);
            }
            isPointsChanged = false;
        }

        public override void ChangeState()
        {
        }

        public override bool CheckCollision(Entity entity)
        {
            return true;
        }

        public override void Reset()
        {
        }

        public override void Move()
        {
        }
    }
}
