namespace PacmanWindowForms
{
    partial class frmMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Gill Sans Ultra Bold", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnStart.Location = new Point(415, 45);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(109, 40);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start Game";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // frmMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.back_ground_menu_2;
            ClientSize = new Size(551, 345);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "frmMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pacman";
            Load += frmMenu_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnStart;
    }
}