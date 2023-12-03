namespace PacmanWindowForms.Forms
{
    partial class frmGameBoard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlGameBoard = new Panel();
            pnlScoreBoard = new Panel();
            lblLevel = new Label();
            lblLevelText = new Label();
            lblScore = new Label();
            lblScoreText = new Label();
            pnlAliveBoard = new Panel();
            pnlLife4 = new Panel();
            pnlLife3 = new Panel();
            pnlLife2 = new Panel();
            pnlLife1 = new Panel();
            panel2 = new Panel();
            pnlScoreBoard.SuspendLayout();
            pnlAliveBoard.SuspendLayout();
            SuspendLayout();
            // 
            // pnlGameBoard
            // 
            pnlGameBoard.BackColor = SystemColors.Desktop;
            pnlGameBoard.BackgroundImageLayout = ImageLayout.None;
            pnlGameBoard.Location = new Point(11, 97);
            pnlGameBoard.Margin = new Padding(3, 4, 3, 4);
            pnlGameBoard.Name = "pnlGameBoard";
            pnlGameBoard.Size = new Size(597, 768);
            pnlGameBoard.TabIndex = 0;
            // 
            // pnlScoreBoard
            // 
            pnlScoreBoard.Controls.Add(lblLevel);
            pnlScoreBoard.Controls.Add(lblLevelText);
            pnlScoreBoard.Controls.Add(lblScore);
            pnlScoreBoard.Controls.Add(lblScoreText);
            pnlScoreBoard.Location = new Point(11, 9);
            pnlScoreBoard.Margin = new Padding(3, 4, 3, 4);
            pnlScoreBoard.Name = "pnlScoreBoard";
            pnlScoreBoard.Size = new Size(320, 77);
            pnlScoreBoard.TabIndex = 1;
            // 
            // lblLevel
            // 
            lblLevel.AutoSize = true;
            lblLevel.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblLevel.ForeColor = SystemColors.HighlightText;
            lblLevel.Location = new Point(226, 41);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(24, 26);
            lblLevel.TabIndex = 3;
            lblLevel.Text = "1";
            lblLevel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLevelText
            // 
            lblLevelText.AutoSize = true;
            lblLevelText.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point);
            lblLevelText.ForeColor = SystemColors.HighlightText;
            lblLevelText.Location = new Point(195, 3);
            lblLevelText.Name = "lblLevelText";
            lblLevelText.Size = new Size(74, 29);
            lblLevelText.TabIndex = 2;
            lblLevelText.Text = "Level";
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblScore.ForeColor = SystemColors.HighlightText;
            lblScore.Location = new Point(67, 41);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(24, 26);
            lblScore.TabIndex = 1;
            lblScore.Text = "0";
            lblScore.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblScoreText
            // 
            lblScoreText.AutoSize = true;
            lblScoreText.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point);
            lblScoreText.ForeColor = SystemColors.HighlightText;
            lblScoreText.Location = new Point(30, 3);
            lblScoreText.Name = "lblScoreText";
            lblScoreText.Size = new Size(102, 29);
            lblScoreText.TabIndex = 0;
            lblScoreText.Text = "SCORE";
            // 
            // pnlAliveBoard
            // 
            pnlAliveBoard.Controls.Add(pnlLife4);
            pnlAliveBoard.Controls.Add(pnlLife3);
            pnlAliveBoard.Controls.Add(pnlLife2);
            pnlAliveBoard.Controls.Add(pnlLife1);
            pnlAliveBoard.Location = new Point(357, 9);
            pnlAliveBoard.Margin = new Padding(3, 4, 3, 4);
            pnlAliveBoard.Name = "pnlAliveBoard";
            pnlAliveBoard.Size = new Size(251, 77);
            pnlAliveBoard.TabIndex = 2;
            // 
            // pnlLife4
            // 
            pnlLife4.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife4.Location = new Point(195, 12);
            pnlLife4.Margin = new Padding(3, 4, 3, 4);
            pnlLife4.Name = "pnlLife4";
            pnlLife4.Size = new Size(46, 53);
            pnlLife4.TabIndex = 3;
            // 
            // pnlLife3
            // 
            pnlLife3.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife3.Location = new Point(137, 12);
            pnlLife3.Margin = new Padding(3, 4, 3, 4);
            pnlLife3.Name = "pnlLife3";
            pnlLife3.Size = new Size(46, 53);
            pnlLife3.TabIndex = 2;
            // 
            // pnlLife2
            // 
            pnlLife2.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife2.Location = new Point(75, 12);
            pnlLife2.Margin = new Padding(3, 4, 3, 4);
            pnlLife2.Name = "pnlLife2";
            pnlLife2.Size = new Size(46, 53);
            pnlLife2.TabIndex = 1;
            // 
            // pnlLife1
            // 
            pnlLife1.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife1.Location = new Point(13, 12);
            pnlLife1.Margin = new Padding(3, 4, 3, 4);
            pnlLife1.Name = "pnlLife1";
            pnlLife1.Size = new Size(46, 53);
            pnlLife1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackgroundImage = Properties.Resources.pngegg_21;
            panel2.Location = new Point(11, 873);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(597, 149);
            panel2.TabIndex = 3;
            // 
            // frmGameBoard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(622, 1019);
            Controls.Add(panel2);
            Controls.Add(pnlAliveBoard);
            Controls.Add(pnlScoreBoard);
            Controls.Add(pnlGameBoard);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmGameBoard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pacman";
            Load += frmGameBoard_Load;
            Shown += frmGameBoard_Shown;
            KeyDown += frmPacmanGame_KeyDown;
            pnlScoreBoard.ResumeLayout(false);
            pnlScoreBoard.PerformLayout();
            pnlAliveBoard.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlGameBoard;
        private Panel pnlScoreBoard;
        private Panel pnlAliveBoard;
        private Label lblScore;
        private Label lblScoreText;
        private Panel pnlLife4;
        private Panel pnlLife3;
        private Panel pnlLife2;
        private Panel pnlLife1;
        private Panel panel2;
        private Label lblLevelText;
        private Label lblLevel;
    }
}