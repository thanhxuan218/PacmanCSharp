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
        // Private fields
        private MapController mapController = null;
        private static GameController gameController = null;
        private frmGameBoard gameBoardFrm = null;
        private Displayer displayer = null;
        private int level = 1; // Default level is 1
        private GameState gameState = GameState.Init;
        private bool haveDisplayRequet = true;

        private List<Task> monitorDynamicTasks = new List<Task>();

        // Public methods
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
            
            // Set game state to playing
            this.SetGameState(GameState.Playing);
            Displayer.Instance.setPanel(frmGameBoard.GetPanelGameBoard());
            Displayer.Instance.SetParentForm(frmGameBoard);

            frmGameBoard.Show();
        }

        // This method is used to set the game state
        // Every time the game state is changed, the factory will be notified
        public void SetGameState(GameState gameState)
        {
            if (gameState != this.gameState)
            {
                this.gameState = gameState;
                this.NotifyGameStateChanged();
            }
        }


        // Private methods

        // This method is used to notify the factory that the game state has changed
        // As expectation, this method only be called when the game state has changed
        private void NotifyGameStateChanged()
        {
            // Notify the factory that the game state has changed
            // The factory will notify all entities that the game state has changed
            EntityFactory.Instance.NotifyGameStateChanged(this.gameState);
        }

        public void DrawBoard()
        {
            EntityFactory.Instance.Draw();
        }

        public void RequestDisplay()
        {
            lock (this)
            {
                haveDisplayRequet = true;
            }
        }

        public bool IsOnChane()
        {
            lock (this)
            {
                Logger.Log("Is on change");
                if (haveDisplayRequet)
                {
                    haveDisplayRequet = false;
                    return true;
                }
                return false;
            }
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        
    }
}
