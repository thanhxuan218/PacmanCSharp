using PacmanWindowForms.Scripts.Models;
using PacmanWindowForms;
using PacmanWindowForms.Scripts;
using PacmanWindowForms.Scripts.Controllers;
using PacmanWindowForms.Scripts.Views;
using System.Drawing;
namespace PacmanWindowForms.Forms
{
    public partial class frmGameBoard : Form
    {
        public delegate void FrmGameBoardClosedHandler(object sender, FormClosedEventArgs e, string text);
        public event FrmGameBoardClosedHandler frmGameBoardClosed;

        public frmGameBoard()
        {
            InitializeComponent();
        }

        private void frmGameBoard_Load(object sender, EventArgs e)
        {

            Logger.Log("Game board loaded");
            pnlGameBoard.Paint += new PaintEventHandler(pnlGameBoard_Paint);

            Displayer.Instance.onUpdateBoardSize(ref pnlGameBoard, pnlGameBoard.Width, pnlGameBoard.Height);

        }

        // Game board panel
        public Panel GetPanelGameBoard()
        {
            return pnlGameBoard;
        }

        // On close form
        private void frmGameBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmGameBoardClosed(sender, e, "Game board closed");
        }

        private void pnlGameBoard_Paint(object sender, PaintEventArgs e)
        {
            Logger.Log("onPaint");
            var g = e.Graphics;
            var p = sender as Panel;
            GameController.Instance.DrawBoard();
        }

        private Direction GetDirectionByKey(Keys keys)
        {
            switch (keys)
            {
                case Keys.Left:
                    return Direction.Left;
                case Keys.Right:
                    return Direction.Right;
                case Keys.Up:
                    return Direction.Up;
                case Keys.Down:
                    return Direction.Down;
                default:
                    return Direction.None;
            }
        }

        private void frmPacmanGame_KeyDown(object sender, KeyEventArgs e)
        {
            Logger.Log("onKeyDown Press with key" + e.KeyCode);
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    ((DynamicEntity)EntityFactory.Instance.GetEntity(EntityType.Pacman, "")).SetDirection(GetDirectionByKey(e.KeyCode));
                    break;
                case Keys.Space:
                    if (GameController.Instance.GetGameState() == GameState.Paused)
                    {
                        GameController.Instance.NotifyGameStateChanged(GameState.Running);
                    }
                    else if (GameController.Instance.GetGameState() == GameState.Running)
                    {
                        GameController.Instance.NotifyGameStateChanged(GameState.Paused);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
