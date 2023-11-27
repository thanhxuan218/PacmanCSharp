using PacmanWindowForms.Scripts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacmanWindowForms.Scripts.Views;

namespace PacmanWindowForms.Scripts.Models
{
    public class Border : StaticEntity
    {
        public Border(int width, int height) : base(EntityType.Border, width, height)
        {
        }
        public override void Draw()
        {
            // Draw the border on the screen using List<Point> points
            // For each point in points, draw a rectangle with width and height
            // The color of the rectangle is blue
            // The border is drawn on the screen only when the game state differs from GameState.Init
            isPointsChanged = false;

        }

        public override void onChangeGameState(GameState state)
        {
            // The border does not change state
            // Do nothing
        }
        public override bool CheckCollision(Entity entity)
        {
            // Check collision between this border and another entity
            // If the entity is a dynamic entity, check collision between the entity's perimeter and this border
            // If the entity is a static entity, check collision between the entity's location and this border
            if (entity.entityType == EntityType.Pacman || entity.entityType == EntityType.Ghost)
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
    }

    public class Wall : StaticEntity
    {
        public Wall(int width, int height) : base(EntityType.Wall, width, height)
        {
            isPointsChanged = true;
        }
        public override void Draw()
        {
            // Draw the wall on the screen using List<Point> points
            // For each point in points, draw a rectangle with width and height
            // The color of the rectangle is black
            // The wall is drawn on the screen only when the game state differs from GameState.Init
            Displayer.Instance.onRequestDisplay(points, EntityType.Wall);
        }

        public override void onChangeGameState(GameState state)
        {
            // The wall does not change state
            // Do nothing
            this.Draw();
        }
        public override bool CheckCollision(Entity entity)
        {
            // Check collision between this wall and another entity
            // If the entity is a dynamic entity, check collision between the entity's perimeter and this wall
            // If the entity is a static entity, check collision between the entity's location and this wall
            if (entity.entityType == EntityType.Pacman || entity.entityType == EntityType.Ghost)
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
    }

    public class Dot : StaticEntity
    {
        List<Point> eatenDots = new List<Point>();
        public Dot(int width, int height) : base(EntityType.Dot, width, height)
        {
        }
        public override void Draw()
        {
            // Draw the dot on the screen using List<Point> points
            // For each point in points, draw a rectangle with width and height
            // The color of the rectangle is yellow
            // The dot is drawn on the screen only when the game state differs from GameState.Init
            if (isPointsChanged)
                Displayer.Instance.onRequestDisplay(points, EntityType.Dot);

            isPointsChanged = false;

        }

        public override void onChangeGameState(GameState state)
        {
            // if (gameState == GameState.Playing)
            // {
            //     // The dot is eaten by Pacman
            //     // Remove the dot from the list of dots
            //     // Add 10 points to the score
            //     if (eatenDots.Count > 0)
            //     {
            //         foreach (Point point in eatenDots)
            //         {
            //             points.Remove(point);

            //         }
            //         eatenDots.Clear();
            //         // TODO: add 10 points to the score here
            //     }
            // }
        }
        public override bool CheckCollision(Entity entity)
        {
            // Check collision between this dot and another entity
            // If the entity is a dynamic entity, check collision between the entity's perimeter and this dot
            // If the entity is a static entity, check collision between the entity's location and this dot
            if (entity.entityType == EntityType.Pacman)
            {
                List<Point> otherPoints = entity.GetPoints();
                List<Point> samePoints = points.Intersect(otherPoints).ToList();
                if (samePoints.Count > 0)
                {
                    // The dot is eaten by Pacman
                    // Save the dot's location to eatenDots
                    eatenDots.AddRange(samePoints);
                    return true;

                }
            }
            return false;
        }
    }

    public class Energy : StaticEntity
    {
        List<Point> eatenEnergies = new List<Point>();
        public Energy(int width, int height) : base(EntityType.Energy, width, height)
        {
        }
        public override void Draw()
        {
            // Draw the energy on the screen using List<Point> points
            // For each point in points, draw a rectangle with width and height
            // The color of the rectangle is yellow
            // The energy is drawn on the screen only when the game state differs from GameState.Init
            isPointsChanged = false;
        }

        public override void onChangeGameState(GameState state)
        {
        }

        public override bool CheckCollision(Entity entity)
        {
            // Check collision between this energy and another entity
            // If the entity is a dynamic entity, check collision between the entity's perimeter and this energy
            // If the entity is a static entity, check collision between the entity's location and this energy
            if (entity.entityType == EntityType.Pacman)
            {
                List<Point> otherPoints = entity.GetPoints();
                List<Point> samePoints = points.Intersect(otherPoints).ToList();
                if (samePoints.Count > 0)
                {
                    // The energy is eaten by Pacman
                    // Save the energy's location to eatenEnergies
                    eatenEnergies.AddRange(samePoints);
                    return true;
                }
            }
            return false;
        }
    }
}
