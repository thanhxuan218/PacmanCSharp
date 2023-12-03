using PacmanWindowForms;
using PacmanWindowForms.Script.Controllers;
using System.Diagnostics;
using System.Drawing;
using System.Media;
namespace PacmanWindowForms.Forms
{
    public partial class frmGameBoard : Form
    {
        public delegate void FrmGameBoardClosedHandler(object sender, FormClosedEventArgs e, string text);
        public event FrmGameBoardClosedHandler frmGameBoardClosed;

        GameController gameController;

        public frmGameBoard()
        {
            InitializeComponent();
        }

        private void frmGameBoard_Load(object sender, EventArgs e)
        {
            gameController = new GameController(this, pnlGameBoard);
            pnlGameBoard.Paint += new PaintEventHandler(pnlGameBoard_Paint);
            gameController.State = GameState.GamePaused;
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
            gameController.RePaint();
        }
        private void frmPacmanGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameController == null)
                return;
            Debug.WriteLine($"Board receive: {e.KeyCode}");
            switch (e.KeyCode)
            {
                case Keys.Up:
                    gameController.SetDirection(Direction.Up);
                    break;
                case Keys.Down:
                    gameController.SetDirection(Direction.Down);
                    break;
                case Keys.Left:
                    gameController.SetDirection(Direction.Left);
                    break;
                case Keys.Right:
                    gameController.SetDirection(Direction.Right);
                    break;
                case Keys.Space:
                    if (gameController.State == GameState.GamePaused)
                    {
                        gameController.Resume();
                    }
                    else if (gameController.State == GameState.GameOn)
                    {
                        gameController.Pause();
                    }
                    break;
                default:
                    break;
            }
        }

        public void PlaySound(Stream s)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Stream>(PlaySound), new object[] { s });
                return;
            }
            SoundPlayer player = new SoundPlayer(s);
            //player.Play();
        }

        public void Show(string score, string level, int lives)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, string, int>(Show), new object[] { score, level, lives });
                return;
            }    
            this.lblScore.Text = score;
            this.lblLevel.Text = level;
            if (lives == 4)
            {
                this.pnlLife1.Visible = true;
                this.pnlLife2.Visible = true;
                this.pnlLife3.Visible = true;
                this.pnlLife4.Visible = true;
            }
            else if (lives == 3) 
            {
                this.pnlLife1.Visible = true;
                this.pnlLife2.Visible = true;
                this.pnlLife3.Visible = true;
                this.pnlLife4.Visible = false ;
            }
            else if (lives == 2)
            {
                this.pnlLife1.Visible = true;
                this.pnlLife2.Visible = true;
                this.pnlLife3.Visible = false;
                this.pnlLife4.Visible = false;
            }
            else if (lives == 1)
            {
                this.pnlLife1.Visible = true;
                this.pnlLife2.Visible = false;
                this.pnlLife3.Visible = false;
                this.pnlLife4.Visible = false;
            }
            else
            {
                this.pnlLife1.Visible = false;
                this.pnlLife2.Visible = false;
                this.pnlLife3.Visible = false;
                this.pnlLife4.Visible = false;
            }
        }


        private void frmGameBoard_Shown(object sender, EventArgs e)
        {
            MessageBox.Show("frmGameBoard_Shown is called");
            if (gameController == null) return;
                gameController.Run();
        }
    }
}
