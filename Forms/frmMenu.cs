using PacmanWindowForms.Forms;
using System.Windows.Forms;


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
            form.Show();
            this.Hide();
        }
    }
}