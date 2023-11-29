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
        private Task pacmanLoopTask = null;
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

        public void onInit(frmGameBoard frmGameBoard)
        {
            if (gameBoard == null) { gameBoard = GameBoard.Instance; }
            if (collisionController == null) { collisionController = new CollisionController(); }
            if (pacman == null) { pacman = Pacman.Instance; }
            if (ghostFactory == null) { ghostFactory = GhostFactory.Instance; }
            if (gameBoardView == null) { gameBoardView = new GameBoardView(frmGameBoard); }

            gameBoard.onInit(this.level);
            Logger.Log(TAG + "onInit: Game board is loaded");
            pacman.LoadNew();
            ghostFactory.LoadGhosts();
            gameBoardView.onInit();
        }

        public void RunGame()
        {
            Logger.Log(TAG + "RunGame is called");
            gameBoardView.DrawBoard();
            pacman.StartMoving();
            ghostFactory.StartMoving();
            collisionController.StartMonitorCollision();
            gameLoopTask = new Task(() => GameLoop());
        }

        public void GameLoop()
        {
            int counter = 0;
            Logger.Log(TAG + "GameLoop is called");
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
                    // this.pacman.Print();
                    // this.ghostFactory.Print();
                    Logger.Log("==========================================");
                    counter = 0;
                }
                Thread.Sleep(10);
            }
        }
        public void StopGame()
        {
        }

        public void onStartGame(frmGameBoard frmGameBoard)
        {
            Logger.Log(TAG + ": onStartGame is called");
            // Create a new map controller to load map in case the game is restarted
            this.onInit(frmGameBoard);
            this.RunGame();
            this.pacman.Print();
            this.ghostFactory.Print();
        }

        public void onNewLevel()
        {
            Logger.Log(TAG + "onNewLevel is called - Current level: " + this.level);
        }

        public void onEntityMove(EntityType type)
        {
            collisionController.onEntityMoving(type);
        }
        public void onNotifyCollisionDetected(int collisionType, List<Point> collisionPoints, EntityBase entity = null)
        {
            // Logger.Log(TAG + "onNotifyCollisionDetected is called");
            switch (collisionType)
            {
                case 1:
                    {
                        // Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and dot");
                        break;
                    }
                case 2:
                    {
                        // Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and ghost");
                        break;
                    }
                case 3:
                    {
                        // Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and energy");
                        break;
                    }
                case 4:
                    {
                        // Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between pacman and wall");
                        break;
                    }
                case 5:
                    {
                        // Logger.Log(TAG + "onNotifyCollisionDetected: Collision detected between ghost and wall");
                        break;
                    }
                default:
                    {
                        // Logger.Log(TAG + "onNotifyCollisionDetected: Unknown collision type" + collisionType.ToString());
                        break;
                    }
            }
        }

        public GameState GetGameState()
        {
            lock (this)
            {
                return gameState;
            }
        }

    }
}