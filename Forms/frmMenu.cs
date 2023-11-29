using PacmanWindowForms.Forms;
using System.Windows.Forms;
using PacmanWindowForms.Scripts.Controllers;
using PacmanWindowForms.Scripts;

namespace PacmanWindowForms
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            //GameController.Instance.onStartGame();
            if (this.Visible == false)
            {
                this.Visible = true;
                this.Show();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            frmGameBoard form = new frmGameBoard();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormClosed += (s, args) => { frmGameBoard_Closed(s, args, "Game board closed"); };
            PacmanWindowForms.Scripts.Models_refactor.GameController.Instance.onStartGame(100, 100);
            form.Show();
            this.Hide();
        }

        private void frmGameBoard_Closed(object sender, FormClosedEventArgs e, string text)
        {
            Logger.Log(text);
            this.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}