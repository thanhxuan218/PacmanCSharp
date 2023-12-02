
using System.Windows.Forms;
using System.Drawing;
using System;
using PacmanWinForms;
using PacmanWindowForms;
using PacmanWindowsForm.Script.Models;

public delegate void GhostPaint(Point point, Direction D, GhostColor color, bool sprite1, GhostState state);
namespace PacmanWinForms
{
    public static class ResourceHandler
    {
        public static Dictionary<GhostColor, Dictionary<Direction, Image>> ghostNormalImg = new Dictionary<GhostColor, Dictionary<Direction, Image>>()
        {
            { GhostColor.Blue, new Dictionary<Direction, List<Image>>()
                {
                    { Direction.Down, new List<Image> () {Properties.Resources.BlueGhostDown1, Properties.Resources.BlueGhostDown2 }},
                    { Direction.Up, new List<Image> () {Properties.Resources.BlueGhostUp1, Properties.Resources.BlueGhostUp2 }},
                    { Direction.Right, new List<Image> () {Properties.Resources.BlueGhostRight1, Properties.Resources.BlueGhostRight2 }},
                    { Direction.Left, new List<Image> () {Properties.Resources.BlueGhostLeft1, Properties.Resources.BlueGhostLeft2 }}
                }
            },
            { GhostColor.Pink, new Dictionary<Direction, List<Image>>()
            {
                {Direction.Down, new List<Image> () {Properties.Resources.PinkGhostDown1, Properties.Resources.PinkGhostDown2 }},
                {Direction.Up, new List<Image> () {Properties.Resources.PinkGhostUp1, Properties.Resources.PinkGhostUp2 }},
                {Direction.Right, new List<Image> () {Properties.Resources.PinkGhostRight1, Properties.Resources.PinkGhostRight2 }},
                {Direction.Left, new List<Image> () {Properties.Resources.PinkGhostLeft1, Properties.Resources.PinkGhostLeft2 }}
            }
            },
            { GhostColor.Red, new Dictionary<Direction, List<Image>>()
            {
                {Direction.Down, new List<Image> () {Properties.Resources.RedGhostDown1, Properties.Resources.RedGhostDown2 }},
                {Direction.Up, new List<Image> () {Properties.Resources.RedGhostUp1, Properties.Resources.RedGhostUp2 }},
                {Direction.Right, new List<Image> () {Properties.Resources.RedGhostRight1, Properties.Resources.RedGhostRight2 }},
                {Direction.Left, new List<Image> () {Properties.Resources.RedGhostLeft1, Properties.Resources.RedGhostLeft2 }}
            }
            },
            { GhostColor.Yellow, new Dictionary<Direction, List<Image>>()
            {
                {Direction.Down, new List<Image> () {Properties.Resources.YellowGhostDown1, Properties.Resources.YellowGhostDown2 }},
                {Direction.Up, new List<Image> () {Properties.Resources.YellowGhostUp1, Properties.Resources.YellowGhostUp2 }},
                {Direction.Right, new List<Image> () {Properties.Resources.YellowGhostRight1, Properties.Resources.YellowGhostRight2 }},
                {Direction.Left, new List<Image> () {Properties.Resources.YellowGhostLeft1, Properties.Resources.YellowGhostLeft2 }}
            }
            }};

        public static Dictionary<Direction, Image> ghostEatenImg = new Dictionary<Direction, Image>()
        {
            { Direction.Down, Properties.Resources.eyesDown },
            { Direction.Up, Properties.Resources.eyesUp },
            { Direction.Right, Properties.Resources.eyesRight },
            { Direction.Left, Properties.Resources.eyesLeft }
        };

        public static Dictionary<Direction, Image> ghostBonusImg = new Dictionary<Direction, Image>()
        {
            { Direction.Down, Properties.Resources.BonusBGhost1 },
            { Direction.Up, Properties.Resources.BonusBGhost1 },
            { Direction.Right, Properties.Resources.BonusBGhost1 },
            { Direction.Left, Properties.Resources.BonusBGhost1 }
        };

        public static Dictionary<Direction, Image> ghostBonusEndImg = new Dictionary<Direction, Image>()
        {
            { Direction.Down, Properties.Resources.BonusWGhost1 },
            { Direction.Up, Properties.Resources.BonusWGhost1 },
            { Direction.Right, Properties.Resources.BonusWGhost1 },
            { Direction.Left, Properties.Resources.BonusWGhost1 }
        };

        public static Bitmap FindGhostImage(Direction D, GhostColor color, bool sprite, GhostState state)
        {
            if (state == GhostState.BonusEnd)
            {
                if (sprite) return Properties.Resources.BonusWGhost1;
                else return Properties.Resources.BonusWGhost2;
            }
            else if (state == GhostState.Bonus)
            {
                if (sprite) return Properties.Resources.BonusBGhost1;
                else return Properties.Resources.BonusBGhost2;
            }
            else if (state == GhostState.Eaten)
            {
                return ghostEatenImg[D];
            }
            else
            {
                return ghostNormalImg[color][D][sprite ? 0 : 1];
            }
        }

    }

    public class PacmanBoard
    {
        public int Rows;
        public int Cols;
        public Color BgColor;
        private readonly Panel pnl;
        private Graphics g;
        private Control.ControlCollection c;
        private float cellHeight;
        private float cellWidth;
        private int state = 1;

        private Dictionary<GhostColor, PictureBox> picsGhosts = new Dictionary<GhostColor, PictureBox>()
        {
            { GhostColor.Blue, null },
            { GhostColor.Pink, null },
            { GhostColor.Red, null },
            { GhostColor.Yellow, null }
        };

        public PacmanBoard(Panel pnl, Color? bgColor = null)
        {
            this.Rows = GameBoard.Instance.BoardHeight;
            this.Cols = GameBoard.Instance.BoardWidth;
            this.BgColor = bgColor ?? Color.Black;
            this.pnl = pnl;
        }

        public void Resize()
        {
            cellHeight = pnl.Height / (float)Rows;
            cellWidth = pnl.Width / (float)Cols;

            g = pnl.CreateGraphics();
            c = pnl.Controls;
        }

        public void Clear()
        {
            lock (this)
            {
                g.Clear(BgColor);
            }
        }
        public void DrawRect(Point p, Color col)
        {
            Brush b = new SolidBrush(col);
            lock (this)
            {
                g.FillRectangle(b, p.X * cellWidth, p.Y * cellHeight, cellWidth, cellHeight);
            }
        }
        public void DrawDoor(Point p, Color col)
        {
            Brush b = new SolidBrush(col);
            lock (this)
            {
                g.FillRectangle(b, p.X * cellWidth, p.Y * cellHeight + cellHeight / 4, cellWidth, cellHeight / 2);
            }
        }
        public void DrawDot(Point p, Color col)
        {
            float dotWidth = 2 * cellWidth / 5;
            float dotHeight = 2 * cellHeight / 5;
            Brush b = new SolidBrush(col);
            lock (this)
            {
                g.FillRectangle(b, p.X * cellWidth - dotWidth / 2, p.Y * cellHeight - dotHeight / 2, dotWidth, dotHeight);
            }
        }
        public void DrawBonus(Point p, Color col, int bonusState)
        {
            float dotWidth;
            float dotHeight;
            Brush b = new SolidBrush(col);
            if (bonusState == 1)
            {
                dotWidth = cellWidth; dotHeight = cellHeight;
            }
            else if (bonusState == 2)
            {
                dotWidth = (float)1.25 * cellWidth; dotHeight = (float)1.25 * cellHeight;
            }
            else if (bonusState == 3)
            {
                dotWidth = (float)2 * cellWidth; dotHeight = (float)2 * cellHeight;
            }
            else
            {
                dotWidth = (float)1.25 * cellWidth; dotHeight = (float)1.25 * cellHeight;
            }
            lock (this)
            {
                g.FillEllipse(b, p.X * cellWidth - dotWidth / 2, p.Y * cellHeight - dotHeight / 2, dotWidth, dotHeight);
            }
        }
        public void ClearBonus(Point p)
        {
            lock (this)
            {
                g.FillEllipse(new SolidBrush(Color.Black), p.X * cellWidth - cellWidth, p.Y * cellHeight - cellHeight, 2 * cellWidth, 2 * cellHeight);
            }
        }
        public void DrawGhost(Point P, Direction D, GhostColor color, bool sprite1, GhostState state)
        {
            pnl.Invoke(new GhostPaint(ChangePic), P, D, color, sprite1, state);
        }
        private void UpdateGhostImage(Point P, Direction D, GhostColor color, bool sprite1, GhostState state)
        {
            if (picsGhosts[color] != null)
            {
                pnl.Controls.Remove(picsGhosts[color]);
            }
            picsGhosts[color] = new PictureBox();
            picsGhosts[color].BackgroundImageLayout = ImageLayout.Stretch;
            picsGhosts[color].Location = new Point((int)(P.X * cellWidth + 4), (int)(P.Y * cellHeight + 4));
            picsGhosts[color].Size = new Size((int)(4 * cellWidth) - 6, (int)(4 * cellHeight) - 6);
            picsGhosts[color].BackgroundImage = ResourceHandler.FindGhostImage(D, color, sprite1, state);
            pnl.Controls.Add(picsGhosts[color]);
        }

        public void DrawPacMan(int x, int y, Color color, Direction dir)
        {
            Brush b = new SolidBrush(color);
            Rectangle rect = new Rectangle((int)(x * cellWidth + 4), (int)(y * cellHeight + 4), (int)(cellWidth * 4 - 8), (int)(cellHeight * 4 - 8));
            int startAngle;
            int sweepAngle;
            CalculateAngles(dir, out startAngle, out sweepAngle);
            lock (this)
            {
                g.FillPie(b, rect, startAngle, sweepAngle);

            }
        }

        private void CalculateAngles(Direction dir, out int startAngle, out int sweepAngle)
        {
            int stAngle, swAngle;
            if (state == 1)
            {
                stAngle = 0; swAngle = 380;
                state++;
            }
            else if (state == 2)
            {
                stAngle = 25; swAngle = 310;
                state++;
            }
            else if (state == 3)
            {
                stAngle = 58; swAngle = 240;
                state++;
            }
            else
            {
                stAngle = 25; swAngle = 310;
                state = 1;
            }

            startAngle = stAngle; sweepAngle = swAngle;
            switch (dir)
            {
                case Direction.Right:

                    break;
                case Direction.Left:
                    startAngle += 180;
                    break;
                case Direction.Up:
                    startAngle -= 90;
                    break;
                case Direction.Down:
                    startAngle += 90;
                    break;
                case Direction.Stop:
                    startAngle = 0; sweepAngle = 380;
                    break;
            }

        }

        public void DrawPacMan(Point p, Color color, Direction dir)
        {
            DrawPacMan(p.X, p.Y, color, dir);
        }

        public void ClearPacMan(Point p)
        {

            Brush b = new SolidBrush(BgColor);
            lock (this)
            {
                g.FillEllipse(b, p.X * cellWidth, p.Y * cellHeight, cellWidth * 4, cellHeight * 4);
            }
        }
    }
}
