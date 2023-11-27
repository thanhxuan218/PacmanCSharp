using PacmanWindowForms.Scripts.Controllers;
using PacmanWindowForms.Scripts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                Logger.Log("Pacman is drawing");
                Displayer.Instance.onRequestDisplay(new List<Point>() { position }, EntityType.Pacman);
            }
            isPointsChanged = false;
        }

        public override void onChangeGameState(GameState state)
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

        // public void EntityHandler()
        // {
        //     // This method is called when the game state is GameState.Running
        //     // This method is used to handle the pacman's movement
        //     // The pacman moves in the direction specified by the user
        //     // The pacman can only move to the next location if the next location is not a wall
        //     // If the next location is a dot, the dot is eaten
        //     // If the next location is an energy, the energy is eaten and the pacman becomes special
        //     // If the next location is a ghost, the pacman loses a life and the game state is changed to GameState.GameOver
        //     // If the next location is a border, the pacman is teleported to the opposite border
        //     // If the next location is a none, the pacman moves to the next location
        //     // If the next location is a special dot, the special dot is eaten and the pacman becomes special
        //     // If the next location is a special energy, the special energy is eaten and the pacman becomes special
        //     // If the next location is a special ghost, the special ghost is eaten and the pacman becomes special
        //     // If the next location is a special border, the pacman is teleported to the opposite border
        //     // If the next location is a special none, the pacman moves to the next location
        //     // If the next location is a special special dot, the special special dot is eaten and the pacman becomes special
        //     // If the next location is a special special energy, the special special energy is eaten and the pacman becomes special
        //     // If the next location is a special special ghost, the special special ghost is eaten and the pacman becomes special
        //     // If the next location is a special special border, the pacman is teleported to the opposite border
        //     // If the next location is a special special none, the pacman moves to the next location
        //     // If the next location is a dead ghost, the dead ghost is respawned
        //     // If the next location is a dead border, the pacman is teleported to the opposite border
        //     // If the next location is a dead none, the pacman moves to the next location
        //     // If the next location is a dead dot, the dead dot is eaten
        //     // If the next location is a dead energy, the dead energy is eaten and the pacman becomes special
        //     // If the next location is a dead ghost, the dead ghost is eaten and the pacman becomes special
        //     // If the next location is a dead border, the pacman is teleported to the opposite border
        //     // If the next location is a dead none, the pacman moves to the next location
        //     // If the next location is a dead special dot, the dead special dot is eaten and the pacman becomes special
        //     // If the next location is a dead special energy, the dead special energy is eaten and the pacman becomes special
        //     // If the next location is a dead special ghost, the dead special ghost is eaten and the pacman becomes special
        //     // If the next location is a dead special border, the pacman is teleported to the opposite border
        //     // If the next location is a dead special none, the pacman moves to the next location
        //     // If the next location is a dead special special dot, the dead special special dot is eaten and the pacman becomes special
        //     // If the next location is a dead special special energy, the dead special special energy is eaten and the pacman becomes special
        //     // If the next location is a dead special special ghost, the dead special special ghost is eaten and the pacman becomes special
        //     // If the next location is a dead special special border, the pacman is teleported to the opposite border
        //     // If the next location is a dead special special none, the pacman moves to the next location
        //     // If the next location is a dead dead dot, the dead dead dot is eaten
        //     // If the next location is a dead dead energy, the dead dead energy is eaten and the pacman becomes special
        //     // If the next location is a dead dead ghost, the dead dead ghost is eaten and the pacman becomes special
        //     // If the next location is a dead dead border, the pacman is teleported to the opposite border
        //     // If the next location is a dead dead none, the pacman moves to the next location
        //     // If the next location is a dead dead special dot, the dead dead special dot is eaten and the pacman becomes special

        //     Logger.Log("Pacman is moving");
        //     // Get the next location
        //     Point nextLocation = GetNextLocation();
        //     // Get the entity at the next location
        //     Entity entity = Map.Instance.GetEntity(nextLocation);
        //     // Check if the entity is null
        //     if (entity == null)
        //     {
        //         // If the entity is null, the pacman moves to the next location
        //         MoveToNextLocation();
        //     }
        //     else
        //     {
        //         // If the entity is not null, check if the entity is a wall
        //         if (entity.entityType == EntityType.Wall)
        //         {
        //             // If the entity is a wall, the pacman does not move
        //         }
        //         else
        //         {
        //             // If the entity is not a wall, check if the entity is a dot
        //             if (entity.entityType == EntityType.Dot)
        //             {
        //                 // If the entity is a dot, the dot is eaten
        //                 // The pacman moves to the next location
        //                 // The score is increased by 10
        //                 // The number of dots is decreased by 1
        //                 // Check if the number of dots is 0
        //                 // If the number of dots is 0, the game state is changed to GameState.GameOver
        //             }
        //             else
        //             {
        //                 // If the entity is not a dot, check if the entity is an energy
        //                 if (entity.entityType == EntityType.Energy)
        //                 {
        //                     // If the entity is an energy, the energy is eaten
        //                     // The pacman becomes special
        //                     // The pacman moves to the next location
        //                     // The score is increased by 50
        //                     // The number of energies is decreased by 1
        //                     // Check if the number of energies is 0
        //                     // If the number of energies is 0, the game state is changed to GameState.GameOver
        //                 }
        //                 else
        //                 {
        //                     // If the entity is not an energy, check if the entity is a ghost
        //                     if (entity.entityType == EntityType.Ghost)
        //                     {
        //                         // If the entity is a ghost, check if the pacman is special
        //                         if (this.state == DynamicEntityState.Special)
        //                         {
        //                             // If the pacman is special, the ghost is eaten
        //                             // The pacman becomes normal
        //                             // The pacman moves to the next location
        //                             // The score is increased by 200
        //                             // The number of ghosts is decreased by 1
        //                             // Check if the number of ghosts is 0
        //                             // If the number of ghosts is 0, the game state is changed to GameState.GameOver
        //                         }
        //                         else
        //                         {
        //                             // If the pacman is not special, the pacman loses a life
        //                             //
        //                         }

    }
}
