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
            GameController.Instance.onStartGame(form);

            form.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}