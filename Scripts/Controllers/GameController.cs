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

namespace PacmanWindowForms.Scripts.Controllers
{
    public class GameController
    {
        private MapController mapController = null;
        private static GameController gameController = null;
        private frmGameBoard gameBoardFrm = null;
        private Displayer displayer = null;
        private int level = 1; // Default level is 1
        private GameState gameState = GameState.Init;
        private bool haveDisplayRequet = true;

        private List<Task> monitorDynamicTasks = new List<Task>();

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
        public void onStartGame(frmGameBoard frmGameBoard)
        {
            Logger.Log("Game started");
            // Create a new map controller to load map in case the game is restarted
            if (mapController == null) { mapController = MapController.Instance; }
            if (displayer == null) { displayer = Displayer.Instance; }

            mapController.onLoadMap(this.level);
            // Set game state to Pause
            Displayer.Instance.setPanel(frmGameBoard.GetPanelGameBoard());
            Displayer.Instance.SetParentForm(frmGameBoard);

            this.NotifyGameStateChanged(GameState.Paused);
            frmGameBoard.Show();

            Logger.Log("Start testing");
            // GameBoard.Instance.Load(1);
            // GameBoard.Instance.PrintMap();
        }

        // This method is used to notify the factory that the game state has changed
        // As expectation, this method only be called when the game state has changed
        public void NotifyGameStateChanged(GameState gameState)
        {
            lock (this)
            {
                this.gameState = gameState;
                EntityFactory.Instance.NotifyGameStateChanged(gameState);
            }
        }
        public void DrawBoard()
        {
            EntityFactory.Instance.Draw();
        }

        public GameState GetGameState()
        {
            return gameState;
        }


    }
}
