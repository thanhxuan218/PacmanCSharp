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

        // Public fields



        // Private fields
        private MapController mapController = null;

        private static GameController gameController = null;

        private frmGameBoard gameBoardFrm = null;

        private Displayer displayer = null;

        private int level = 1; // Default level is 1

        private GameState gameState = GameState.Init;

        private bool haveDisplayRequet = true;

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
        public void onStartGame()
        {
            Logger.Log("Game started");
            // Create a new map controller to load map in case the game is restarted
            if (mapController == null) { mapController = MapController.Instance; }
            if (displayer == null) { displayer = Displayer.Instance; }
            // Load map according to the level

            // TODO: please complete this method
            
            mapController.onLoadMap(this.level);

            gameBoardFrm = new frmGameBoard();
            gameBoardFrm.StartPosition = FormStartPosition.CenterScreen;
            
            // Set game state to playing
            this.SetGameState(GameState.Playing);
            Thread.Sleep(10);

            gameBoardFrm.SuspendLayout();

            gameBoardFrm.ResumeLayout(false);
            Displayer.Instance.setPanel(gameBoardFrm.GetPanelGameBoard());
            Displayer.Instance.SetParentForm(gameBoardFrm);
            Logger.Log(gameBoardFrm.GetPanelGameBoard().GetHashCode().ToString());
            Logger.Log(Displayer.Instance.pnl.GetHashCode().ToString());

            EntityFactory.Instance.SetIsPointsChanged(EntityType.Wall, "", true);   

            gameBoardFrm.Show();
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

        public bool IsOnChane()
        {
            if (haveDisplayRequet)
            {
                haveDisplayRequet = false;
                return true;
            }
            return false;
        }
    }
}
