using PacmanWindowForms.Forms;
using PacmanWindowForms.Scripts.Controllers;
using PacmanWindowForms.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWindowForms.Scripts.Views
{
    public class Displayer
    {
        private static Displayer instance = null;

        private static readonly object padlock = new object();

        public Form parentForm = null;
        public Panel pnl = null;
        public Graphics g = null;

        public int boardHeight = 0;
        public int boardWidth = 0;

        public float cellHeight = 0;
        public float cellWidth = 0;

        public float cellHeightOffset = 0;
        public float cellWidthOffset = 0;

        public static Displayer Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Displayer();
                    }
                    return instance;
                }
            }
        }

        public void onRequestDisplay(List<Point> points, EntityType type, string postfix = "")
        {
            lock (padlock)
            {
                if (pnl == null)
                {
                    Logger.Log("Panel is null");
                    return;
                }

                points = points.OrderBy(points => points.X).ThenBy(points => points.Y).ToList();

                switch (type)
                {
                    case EntityType.Pacman:
                        DrawPacman(points);
                        break;
                    case EntityType.Ghost:
                        DrawGhost(points, postfix);
                        break;
                    case EntityType.Wall:
                        DrawWall(points);
                        break;
                    case EntityType.Dot:
                        DrawDot(points);
                        break;
                    case EntityType.Border:
                        DrawBorder(points);
                        break;
                    case EntityType.Energy:
                        DrawEnergy(points);
                        break;
                    default:
                        Logger.Log("Invalid entity type");
                        break;
                }
            }
        }

        private void DrawPacman(List<Point> points)
        {
            Logger.Log("Enter DrawPacman() points: " + points.Count);
            for (int i = 0; i < points.Count; i++)
            {
                Logger.Log("Draw Pacman at point: " + points[i].ToString());
                DrawCircle(points[i], cellWidth, cellHeight, cellHeight, System.Drawing.Color.Yellow);
            }
        }

        private PictureBox picRedGhost;
        private PictureBox picBlueGhost;
        private PictureBox picPinkGhost;
        private PictureBox picYellowGhost;

        private void DrawGhost(List<Point> points, string postfix)
        {
            GhostColor color = GhostColor.None;
            DynamicEntityState state = DynamicEntityState.Normal;
            Direction direction = Direction.Down;
            PictureBox pic = null;

            if (postfix == "")
            {
                Logger.Log("Invalid ghost postfix");
                return;
            }

            if (postfix == "Red")
            {
                color = GhostColor.Red;
                pic = picRedGhost;
            }
            else if (postfix == "Blue")
            {
                color = GhostColor.Blue;
                pic = picBlueGhost;
            }
            else if (postfix == "Pink")
            {
                color = GhostColor.Pink;
                pic = picPinkGhost;
            }
            else if (postfix == "Yellow")
            {
                color = GhostColor.Yellow;
                pic = picYellowGhost;
            }
            else
            {
                Logger.Log("Invalid ghost postfix");
                return;
            }

            if (points.Count == 0)
            {
                Logger.Log("Invalid ghost points");
                return;
            }

            Bitmap img = FindGhostImage(color, state, direction);
            if (img == null)
            {
                Logger.Log($"Can not find image for {color.ToString()} state {state.ToString()} direction {direction.ToString()}");
                return;
            }

            if (pic != null) pnl.Controls.Remove(pic);
            pic = new PictureBox();
            pic.BackgroundImageLayout = ImageLayout.Zoom;
            pic.Location = new Point((int)(points[0].X * cellWidth + 4), (int)(points[0].Y * cellHeight + 4));
            pic.Size = new Size((int)(4 * cellWidth) - 6, (int)(4 * cellHeight) - 6);
            pic.BackgroundImage = img;
            pnl.Controls.Add(pic);
        }

        private void DrawWall(List<Point> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                DrawRectangle(new Point(points[i].X, points[i].Y), cellWidth, cellHeight, System.Drawing.Color.Blue);
            }
        }

        private void DrawDot(List<Point> points)
        {
            float dotWidth = 2 * cellWidth / 5;
            float dotHeight = 2 * cellHeight / 5;

            Brush brush = new SolidBrush(System.Drawing.Color.OrangeRed);
            for (int i = 0; i < points.Count; i++)
            {
                float posX = points[i].X * cellHeight - dotHeight / 2;
                float posY = points[i].Y * cellWidth - dotWidth / 2;
                g.FillRectangle(brush, posX, posY, dotWidth, dotHeight);
            }
        }

        private void DrawBorder(List<Point> points)
        {
        }

        private void DrawEnergy(List<Point> points)
        {
        }

        public void onUpdateBoardSize(ref Panel pnl, int width, int height)
        {
            this.pnl = pnl;
            g = pnl.CreateGraphics();

            boardWidth = MapController.Instance.GetBoardWidth();
            boardHeight = MapController.Instance.GetBoardHeight();
            cellHeight = height / (float)boardHeight;
            cellWidth = width / (float)boardWidth;
            cellHeightOffset = (height - cellHeight * boardHeight) / 2;
            cellWidthOffset = (width - cellWidth * boardWidth) / 2;
            Logger.Log("Panel size: " + width + " " + height);
            Logger.Log("Board size: " + boardWidth + " " + boardHeight);
            Logger.Log("Cell size: " + cellWidth + " " + cellHeight);
        }

        public void setPanel(Panel pnl)
        {
            lock (padlock)
            {
                this.pnl = pnl;
                this.g = pnl.CreateGraphics();
            }
        }

        public Panel getPanel()
        {
            lock (padlock)
            {
                return pnl;
            }
        }

        public void clearPanel()
        {
            lock (padlock)
            {
                pnl.Controls.Clear();
            }
        }

        public void SetParentForm(Form parentForm)
        {
            lock (padlock)
            {
                this.parentForm = parentForm;
            }
        }

        public void DrawRectangle(Point p, float width, float height, System.Drawing.Color color)
        {
            lock (padlock)
            {

                Brush brush = new SolidBrush(color);
                this.g.FillRectangle(brush, p.X * width, p.Y * height, width, height);
            }
        }

        private void DrawCircle(Point p, float width, float height, float radius, System.Drawing.Color color)
        {
            lock (padlock)
            {
                Brush b = new SolidBrush(color);
                Brush bb = new SolidBrush(Color.Black);
                Rectangle rect = new Rectangle((int)(p.X * width + 5), (int)(p.Y * height + 5), (int)(cellWidth * 4 - 8), (int)(cellHeight * 4 - 8));
                lock (this)
                {
                    g.FillEllipse(bb, p.X * width, p.Y * height, cellWidth * 4, cellHeight * 4);
                    g.FillPie(b, rect, 24, 310);
                }
            }
        }

        private Bitmap FindGhostImage(GhostColor color, DynamicEntityState state, Direction direction)
        {
            switch (state)
            {
                case DynamicEntityState.Normal:
                case DynamicEntityState.Respawn:
                    return FindGhostNormalImg(color, direction);
                case DynamicEntityState.Special:
                    return FindGhostScareImg(color, direction);
                case DynamicEntityState.Dead:
                    return FindGhostDeadImg(color);
                default:
                    Logger.Log($"Can not load image for {color.ToString()} state {state.ToString()} direction {direction.ToString()}");
                    break;
            }
            return null;
        }


        private readonly Dictionary<GhostColor, Dictionary<Direction, Image>> imgNormal = new Dictionary<GhostColor, Dictionary<Direction, Image>>
        {
            { GhostColor.Red, new Dictionary<Direction, Image>
                {
                    { Direction.Up, Properties.Resources.RedGhostUp1 },
                    { Direction.Down, Properties.Resources.RedGhostDown1 },
                    { Direction.Left, Properties.Resources.RedGhostLeft1 },
                    { Direction.Right, Properties.Resources.RedGhostRight1 }
                }
            },
            { GhostColor.Blue, new Dictionary<Direction, Image>
                {
                    { Direction.Up, Properties.Resources.BlueGhostUp1 },
                    { Direction.Down, Properties.Resources.BlueGhostDown1 },
                    { Direction.Left, Properties.Resources.BlueGhostLeft1 },
                    { Direction.Right, Properties.Resources.BlueGhostRight1 }
                }
            },
            { GhostColor.Yellow, new Dictionary<Direction, Image>
                {
                    { Direction.Up, Properties.Resources.YellowGhostUp1 },
                    { Direction.Down, Properties.Resources.YellowGhostDown1 },
                    { Direction.Left, Properties.Resources.YellowGhostLeft1 },
                    { Direction.Right, Properties.Resources.YellowGhostRight1 }
                }
            },
            { GhostColor.Pink, new Dictionary<Direction, Image>
                {
                    { Direction.Up, Properties.Resources.PinkGhostUp1 },
                    { Direction.Down, Properties.Resources.PinkGhostDown1 },
                    { Direction.Left, Properties.Resources.PinkGhostLeft1 },
                    { Direction.Right, Properties.Resources.PinkGhostRight1 }
                }
            }
        };

        private Bitmap FindGhostNormalImg(GhostColor color, Direction d)
        {
            if (imgNormal.ContainsKey(color))
            {
                if (imgNormal[color].ContainsKey(d))
                {
                    return new Bitmap(imgNormal[color][d]);
                }
            }
            return null;
        }

        private Bitmap FindGhostScareImg(GhostColor color, Direction d)
        {
            return null;
        }

        private Bitmap FindGhostDeadImg(GhostColor color)
        {
            return null;
        }

    }
}
