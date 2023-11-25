using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacmanWindowForms.Scripts.Models;

namespace PacmanWindowForms.Scripts.Models
{
    // Class for implementing ghosts
    public class Ghost : DynamicEntity
    {
        public GhostColor ghostColor;
        public Ghost(GhostColor ghostColor, Point position, int width, int height)
        : base(EntityType.Ghost, position, width, height, Direction.None, 1, 0x9999)
        {
            this.ghostColor = ghostColor;
            this.state = DynamicEntityState.Normal;
        }

        public override void Draw(Graphics g)
        {
            // Draw the ghost on the screen using List<Point> points
            // For each point in points, draw a rectangle with width and height
            // The color of the rectangle is determined by the ghost's color
            // The ghost is drawn on the screen only when the game state differs from GameState.Init
        }

        public override void ChangeState()
        {
            // Change the state of the ghost
            // At the beginning of the game, the ghost is in Normal state
            // When the ghost is eaten by Pacman, the ghost is in Dead state
            // After a while (1s), the ghost is in Respawn state. In this state, 
            // After a 1s, the ghost is in Normal state again - In 1s the ghost is in Respawn state and position is reset to the initial position
            // The ghost only changes state when the game state is GameState.Playing, other request in other game states are ignored    
        }

        public override bool CheckCollision(Entity entity)
        {
            // Check collision between this ghost and another entity
            // If the entity is a dynamic entity, check collision between the entity's perimeter and this ghost
            // If the entity is a static entity, check collision between the entity's location and this ghost
            if (entity.entityType == EntityType.Pacman)
            {
                List<Point> otherPoints = entity.GetPoints();
                List<Point> samePoints = points.Intersect(otherPoints).ToList();
                if (samePoints.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void SetGhostColor(GhostColor ghostColor)
        {
            this.ghostColor = ghostColor;
        }

        public GhostColor GetGhostColor()
        {
            return ghostColor;
        }

        public override void Move()
        {
            // Move the ghost
            // The ghost moves in a random direction
            // The ghost only moves when the game state is GameState.Playing, other request in other game states are ignored
        }

        public override void Reset()
        {
            // Reset the ghost to the initial state
            // The ghost is in Normal state
            // The ghost is in the initial position
        }

    }
}
