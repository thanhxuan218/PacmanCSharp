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
        public frmGameBoard()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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
            Application.Exit();
        }

        private void pnlGameBoard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = sender as Panel;
            if (GameController.Instance.IsOnChane())
            {
                GameController.Instance.DrawBoard();
            }
               
        }
    }
}
