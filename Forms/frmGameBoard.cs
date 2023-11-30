using PacmanWindowForms;
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

            pnlGameBoard.Paint += new PaintEventHandler(pnlGameBoard_Paint);
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
            var g = e.Graphics;
            var p = sender as Panel;
        }
        private void frmPacmanGame_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:

                    break;
                case Keys.Space:
                    break;
                default:
                    break;
            }
        }
    }
}
