using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PacmanWindowForms.Scripts.Models;


namespace PacmanWindowForms.Scripts.Models_refactor
{
    public class CollisionController
    {
        private Task monitorCollisionTask = null;
        // Protect this variable from being accessed by multiple threads at the same time
        private bool isEntityMoving = true;
        public CollisionController() { }

        private List<Point> wallPoints = new List<Point>();
        private List<Point> dotPoints = new List<Point>();
        private List<Point> energyPoints = new List<Point>();
        private List<Point> ghostPoints = new List<Point>();
        private List<Point> pacmanPoints = new List<Point>();

        public void StartMonitorCollision()
        {
            if (monitorCollisionTask != null)
            {
                return; // Already started
            }

            monitorCollisionTask = new Task(() =>
            {
                while (true)
                {
                    // if (GameController.Instance.GetGameState() == GameState.Running)
                    {
                        if (this.IsEntityMoving())
                        {
                            this.LoadPoints();
                            CheckCollisionGhostAndPacman();
                            CheckCollisionGhostAndGhost();
                            CheckCollisionGhostAndWall();
                            CheckCollisionPacmanAndDot();
                            CheckCollisionPacmanAndEnergy();
                            CheckCollisionPacmanAndWall();
                        }
                    }
                    Thread.Sleep(20);
                    // Logger.Log("CollisionController: Monitor collision");
                }
            });
            monitorCollisionTask.Start();
        }



        public void CheckCollisionGhostAndPacman()
        {
            // Logger.Log("CheckCollisionGhostAndPacman");
            List<Point> commonPoints = new List<Point>();
            commonPoints = ghostPoints.Intersect(pacmanPoints).ToList();
            if (commonPoints.Count > 0)
            {
                // Logger.Log("Collision detected between ghost and pacman");
                this.NotifyCollisionDetected(1, commonPoints);
            }
        }

        public void CheckCollisionGhostAndGhost()
        {
            // Logger.Log("CheckCollisionGhostAndGhost - Do nothing");
        }

        public void CheckCollisionGhostAndWall()
        {
            // Logger.Log("CheckCollisionGhostAndWall");
            List<Point> commonPoints = new List<Point>();
            commonPoints = ghostPoints.Intersect(wallPoints).ToList();

            Logger.Log("CheckCollisionGhostAndWall: commonPoints.Count = " + commonPoints.Count);
            Logger.Log("Wall points: " + wallPoints + " ghost points: " + ghostPoints);
            Logger.Log("Wall points: " + wallPoints.Count + " ghost points: " + ghostPoints.Count);

            if (commonPoints.Count > 0)
            {
                Logger.Log("Collision detected between ghost and wall");
                Ghost ghost = GhostFactory.Instance.GetGhostByPoints(commonPoints);
                this.NotifyCollisionDetected(5, commonPoints, ghost);
            }
        }

        public void CheckCollisionPacmanAndDot()
        {
            // Logger.Log("CheckCollisionPacmanAndDot");
            List<Point> commonPoints = new List<Point>();
            commonPoints = dotPoints.Intersect(pacmanPoints).ToList();
            if (commonPoints.Count > 0)
            {
                // Logger.Log("Collision detected between pacman and dot");
                this.NotifyCollisionDetected(2, commonPoints);
            }
        }

        public void CheckCollisionPacmanAndEnergy()
        {
            // Logger.Log("CheckCollisionPacmanAndEnergy");
            List<Point> commonPoints = new List<Point>();
            commonPoints = energyPoints.Intersect(pacmanPoints).ToList();
            if (commonPoints.Count > 0)
            {
                // Logger.Log("Collision detected between pacman and energy");
                this.NotifyCollisionDetected(3, commonPoints);
            }
        }

        public void CheckCollisionPacmanAndWall()
        {
            // Logger.Log("CheckCollisionPacmanAndWall");
            List<Point> commonPoints = new List<Point>();
            commonPoints = wallPoints.Intersect(pacmanPoints).ToList();
            if (commonPoints.Count > 0)
            {
                // Logger.Log("Collision detected between pacman and wall");
                this.NotifyCollisionDetected(4, commonPoints);
            }
        }

        private void LoadPoints()
        {
            wallPoints = GameBoard.Instance.GetEntityLocs(EntityType.Wall);
            dotPoints = GameBoard.Instance.GetEntityLocs(EntityType.Dot);
            energyPoints = GameBoard.Instance.GetEntityLocs(EntityType.Energy);
            ghostPoints = PacmanWindowForms.Scripts.Models_refactor.GhostFactory.Instance.GetAllPoints();
            pacmanPoints = PacmanWindowForms.Scripts.Models_refactor.Pacman.Instance.GetPoints();

            // Logger.Log("LoadPoints: wallPoints.Count = " + wallPoints.Count);
            // Logger.Log("LoadPoints: dotPoints.Count = " + dotPoints.Count);
            // Logger.Log("LoadPoints: energyPoints.Count = " + energyPoints.Count);
            // Logger.Log("LoadPoints: ghostPoints.Count = " + ghostPoints.Count);
            // Logger.Log("LoadPoints: pacmanPoints.Count = " + pacmanPoints.Count);

        }

        public void onEntityMoving(EntityType entityType)
        {
            Logger.Log($"onEntityMoving {entityType} is moving");
            lock (this)
            {
                isEntityMoving = true;
            }
        }

        public bool IsEntityMoving()
        {
            lock (this)
            {
                return isEntityMoving;
            }
        }

        public void NotifyCollisionDetected(int collisionType, List<Point> collisionPoints, EntityBase entity = null)
        {
            Logger.Log($"Collision detected {collisionType}");
        }
    }
}