﻿namespace PacmanWindowForms.Forms
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
            pnlGameBoard.Location = new Point(10, 73);
            pnlGameBoard.Name = "pnlGameBoard";
            pnlGameBoard.Size = new Size(522, 576);
            pnlGameBoard.TabIndex = 0;
            pnlGameBoard.Paint += panel1_Paint;
            // 
            // pnlScoreBoard
            // 
            pnlScoreBoard.Controls.Add(lblLevel);
            pnlScoreBoard.Controls.Add(lblLevelText);
            pnlScoreBoard.Controls.Add(lblScore);
            pnlScoreBoard.Controls.Add(lblScoreText);
            pnlScoreBoard.Location = new Point(10, 7);
            pnlScoreBoard.Name = "pnlScoreBoard";
            pnlScoreBoard.Size = new Size(280, 58);
            pnlScoreBoard.TabIndex = 1;
            // 
            // lblLevel
            // 
            lblLevel.AutoSize = true;
            lblLevel.Font = new Font("Gill Sans Ultra Bold", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblLevel.ForeColor = SystemColors.HighlightText;
            lblLevel.Location = new Point(198, 31);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(27, 26);
            lblLevel.TabIndex = 3;
            lblLevel.Text = "1";
            lblLevel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLevelText
            // 
            lblLevelText.AutoSize = true;
            lblLevelText.Font = new Font("Gill Sans Ultra Bold", 15F, FontStyle.Regular, GraphicsUnit.Point);
            lblLevelText.ForeColor = SystemColors.HighlightText;
            lblLevelText.Location = new Point(171, 2);
            lblLevelText.Name = "lblLevelText";
            lblLevelText.Size = new Size(78, 29);
            lblLevelText.TabIndex = 2;
            lblLevelText.Text = "Level";
            lblLevelText.Click += label1_Click;
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Font = new Font("Gill Sans Ultra Bold", 13F, FontStyle.Regular, GraphicsUnit.Point);
            lblScore.ForeColor = SystemColors.HighlightText;
            lblScore.Location = new Point(59, 31);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(27, 26);
            lblScore.TabIndex = 1;
            lblScore.Text = "0";
            lblScore.TextAlign = ContentAlignment.MiddleRight;
            lblScore.Click += label2_Click;
            // 
            // lblScoreText
            // 
            lblScoreText.AutoSize = true;
            lblScoreText.Font = new Font("Gill Sans Ultra Bold", 15F, FontStyle.Regular, GraphicsUnit.Point);
            lblScoreText.ForeColor = SystemColors.HighlightText;
            lblScoreText.Location = new Point(26, 2);
            lblScoreText.Name = "lblScoreText";
            lblScoreText.Size = new Size(93, 29);
            lblScoreText.TabIndex = 0;
            lblScoreText.Text = "SCORE";
            // 
            // pnlAliveBoard
            // 
            pnlAliveBoard.Controls.Add(pnlLife4);
            pnlAliveBoard.Controls.Add(pnlLife3);
            pnlAliveBoard.Controls.Add(pnlLife2);
            pnlAliveBoard.Controls.Add(pnlLife1);
            pnlAliveBoard.Location = new Point(312, 7);
            pnlAliveBoard.Name = "pnlAliveBoard";
            pnlAliveBoard.Size = new Size(220, 58);
            pnlAliveBoard.TabIndex = 2;
            // 
            // pnlLife4
            // 
            pnlLife4.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife4.Location = new Point(171, 9);
            pnlLife4.Name = "pnlLife4";
            pnlLife4.Size = new Size(40, 40);
            pnlLife4.TabIndex = 3;
            // 
            // pnlLife3
            // 
            pnlLife3.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife3.Location = new Point(120, 9);
            pnlLife3.Name = "pnlLife3";
            pnlLife3.Size = new Size(40, 40);
            pnlLife3.TabIndex = 2;
            // 
            // pnlLife2
            // 
            pnlLife2.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife2.Location = new Point(66, 9);
            pnlLife2.Name = "pnlLife2";
            pnlLife2.Size = new Size(40, 40);
            pnlLife2.TabIndex = 1;
            // 
            // pnlLife1
            // 
            pnlLife1.BackgroundImage = Properties.Resources.pacman_alive;
            pnlLife1.Location = new Point(11, 9);
            pnlLife1.Name = "pnlLife1";
            pnlLife1.Size = new Size(40, 40);
            pnlLife1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackgroundImage = Properties.Resources.pngegg_21;
            panel2.Location = new Point(10, 655);
            panel2.Name = "panel2";
            panel2.Size = new Size(522, 112);
            panel2.TabIndex = 3;
            // 
            // frmGameBoard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(544, 764);
            Controls.Add(panel2);
            Controls.Add(pnlAliveBoard);
            Controls.Add(pnlScoreBoard);
            Controls.Add(pnlGameBoard);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "frmGameBoard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pacman";
            Load += frmGameBoard_Load;
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