using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacmanWindowForms.Scripts.Models;
using PacmanWindowForms.Scripts.Controllers;
using System.Runtime.CompilerServices;
using PacmanWindowForms.Scripts.Views;
using PacmanWindowForms.Forms;

namespace PacmanWindowForms.Scripts.Models_refactor
{
    public class GameController
    {
        private string TAG = "GameController";
        private static GameController gameController = null;
        private CollisionController collisionController = null;
        private GameBoard gameBoard = null;
        private GameBoardView gameBoardView = null;
        private Pacman pacman = null;
        private GhostFactory ghostFactory = null;
        private GameState gameState = GameState.Init;

        private Task gameLoopTask = null;
        private Task ghostLoopTask = null;
        private Task pacmanLoopTask = null;
        private Dictionary<GhostColor, bool> ghostMovingStatus = new Dictionary<GhostColor, bool>();

        private int level = 1; // Default level is 1

        public static GameController Instance
        {
            get
            {
                if (gameController == null)
                {
                    gameController = new GameController();
                }
                return gameController;
            }
        }

        public void onInit()
        {
            if (gameBoard == null) { gameBoard = GameBoard.Instance; }
            if (collisionController == null) { collisionController = new CollisionController(); }
            if (pacman == null) { pacman = Pacman.Instance; }
            if (ghostFactory == null) { ghostFactory = GhostFactory.Instance; }
            if (gameBoardView == null) { gameBoardView = new GameBoardView(); }

            gameBoard.onInit(this.level);
            Logger.Log(TAG + "onInit: Game board is loaded");
            pacman.LoadNew();
            ghostFactory.LoadNew();
            gameBoardView.onInit();
        }

        public void RunGame()
        {
            pacman.StartMoving();
            ghostFactory.StartMoving();
            collisionController.StartMonitorCollision();
            ghostLoopTask = new Task(() => GhostMovingController());
            ghostLoopTask.Start();
        }

        public void StopGame()
        {

        }

        public void onStartGame(int pnlWidth, int pnlHeight)
        {
            Logger.Log(TAG + "onStartGame is called");
            // Create a new map controller to load map in case the game is restarted
            this.onInit();
            this.RunGame();

            int counter = 0;
            while (true)
            {
                if (this.gameState == GameState.GameOver)
                {
                    break;
                }
                counter++;
                if (counter == 100)
                {
                    GameBoard.Instance.PrintMap();
                    counter = 0;
                }
                Thread.Sleep(10);
            }
        }

        public void onNewLevel()
        {
            Logger.Log(TAG + "onNewLevel is called - Current level: " + this.level);
        }

        public void GhostMovingController()
        {
            Logger.Log(TAG + "GhostMovingController is called");
            while (true)
            {
                if (!this.IsGhostMoving(GhostColor.Red))
                {
                    ghostFactory.Move(GhostColor.Red, NewDirection(ghostFactory.GetDirection(GhostColor.Red)));
                }
                if (!this.IsGhostMoving(GhostColor.Pink))
                {
                    ghostFactory.Move(GhostColor.Pink, NewDirection(ghostFactory.GetDirection(GhostColor.Pink)));
                }
                if (!this.IsGhostMoving(GhostColor.Blue))
                {
                    ghostFactory.Move(GhostColor.Blue, NewDirection(ghostFactory.GetDirection(GhostColor.Blue)));
                }
                if (!this.IsGhostMoving(GhostColor.Yellow))
                {
                    ghostFactory.Move(GhostColor.Yellow, NewDirection(ghostFactory.GetDirection(GhostColor.Yellow)));
                }
                Thread.Sleep(10);
            }
        }

        public Direction NewDirection(Direction currentDirection)
        {
            Direction newDirection = Direction.None;
            switch (currentDirection)
            {
                case Direction.Up:
                    {
                        newDirection = Direction.Down;
                        break;
                    }
                case Direction.Down:
                    {
                        newDirection = Direction.Up;
                        break;
                    }
                case Direction.Left:
                    {
                        newDirection = Direction.Right;
                        break;
                    }
                case Direction.Right:
                    {
                        newDirection = Direction.Left;
                        break;
                    }
                default:
                    {
                        newDirection = Direction.None;
                        break;
                    }
            }
            return newDirection;
        }

        private bool IsGhostMoving(GhostColor color)
        {
            lock (this)
            {
                return ghostMovingStatus[color];
            }
        }

        private void SetGhostMoving(GhostColor color, bool isMoving)
        {
            lock (this)
            {
                ghostMovingStatus[color] = isMoving;
            }
        }

        public void onEntityMove(EntityType type)
        {
            collisionController.onEntityMoving(type);
            gameBoardView.onEntityMove(type);
        }
        public void onNotifyCollisionDetected(int collisionType, List<Point> collisionPoints, EntityBase entity = null)
        {
            Logger.Log(TAG + "onNotifyCollisionDetected is called");
            switch (collisionType)
            {
                case 1:
                    {
                        Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and dot");
                        break;
                    }
                case 2:
                    {
                        Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and ghost");
                        break;
                    }
                case 3:
                    {
                        Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and energy");
                        break;
                    }
                case 4:
                    {
                        Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and wall");
                        break;
                    }
                case 5:
                    {
                        GhostColor ghostColor = ((Ghost)entity).GetColor();
                        this.SetGhostMoving(ghostColor, false);
                        Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between ghost and wall");
                        break;
                    }
                default:
                    {
                        Logger.Log(TAG + "onNotifyCollisionDetected: Unknown collision type" + collisionType.ToString());
                        break;
                    }
            }
        }


    }
}