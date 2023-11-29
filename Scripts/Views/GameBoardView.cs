using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using PacmanWindowForms.Scripts.Models;
using Microsoft.VisualBasic.Logging;
using PacmanWindowForms.Forms;


namespace PacmanWindowForms.Scripts.Models_refactor
{
    public class ImageHandler
    {
        private string TAG = "ImageHandler";
        private static ImageHandler imageHandler = null;
        private Dictionary<string, Image> images = new Dictionary<string, Image>();
        private ImageHandler() { }

        public static ImageHandler Instance
        {
            get
            {
                if (imageHandler == null)
                {
                    imageHandler = new ImageHandler();
                }
                return imageHandler;
            }
        }

        private readonly Dictionary<GhostColor, Dictionary<Direction, List<Image>>> imgNormal = new Dictionary<GhostColor, Dictionary<Direction, List<Image>>>
        {
            { GhostColor.Red, new Dictionary<Direction, List<Image>>
                {
                    { Direction.Up, new List<Image> {Properties.Resources.RedGhostUp1, Properties.Resources.RedGhostUp2} },
                    { Direction.Down, new List<Image> {Properties.Resources.RedGhostDown1, Properties.Resources.RedGhostDown2} },
                    { Direction.Left, new List<Image> {Properties.Resources.RedGhostLeft1, Properties.Resources.RedGhostLeft2} },
                    { Direction.Right, new List<Image> {Properties.Resources.RedGhostRight1, Properties.Resources.RedGhostRight2} }
                }
            },
            { GhostColor.Blue, new Dictionary<Direction, List<Image>>
                {
                    { Direction.Up, new List<Image> {Properties.Resources.BlueGhostUp1, Properties.Resources.BlueGhostUp2} },
                    { Direction.Down, new List<Image> {Properties.Resources.BlueGhostDown1, Properties.Resources.BlueGhostDown2} },
                    { Direction.Left, new List<Image> {Properties.Resources.BlueGhostLeft1, Properties.Resources.BlueGhostLeft2} },
                    { Direction.Right, new List<Image> {Properties.Resources.BlueGhostRight1, Properties.Resources.BlueGhostRight2} }
                }
            },
            { GhostColor.Yellow, new Dictionary<Direction, List<Image>>
                {
                    { Direction.Up, new List<Image> {Properties.Resources.YellowGhostUp1, Properties.Resources.YellowGhostUp2} },
                    { Direction.Down, new List<Image> {Properties.Resources.YellowGhostDown1, Properties.Resources.YellowGhostDown2} },
                    { Direction.Left, new List<Image> {Properties.Resources.YellowGhostLeft1, Properties.Resources.YellowGhostLeft2} },
                    { Direction.Right, new List<Image> {Properties.Resources.YellowGhostRight1, Properties.Resources.YellowGhostRight2} }
                }
            },
            { GhostColor.Pink, new Dictionary<Direction, List<Image>>
                {
                    { Direction.Up, new List<Image> {Properties.Resources.PinkGhostUp1, Properties.Resources.PinkGhostUp2} },
                    { Direction.Down, new List<Image> {Properties.Resources.PinkGhostDown1, Properties.Resources.PinkGhostDown2} },
                    { Direction.Left, new List<Image> {Properties.Resources.PinkGhostLeft1, Properties.Resources.PinkGhostLeft2} },
                    { Direction.Right, new List<Image> {Properties.Resources.PinkGhostRight1, Properties.Resources.PinkGhostRight2} }
                }
            }
        };

        private Image imgGhostScared = Properties.Resources.GhostScare;
        private Dictionary<Direction, Image> imgGhostEyes = new Dictionary<Direction, Image>
        {
            { Direction.Up, Properties.Resources.eyesUp },
            { Direction.Down, Properties.Resources.eyesDown },
            { Direction.Left, Properties.Resources.eyesLeft },
            { Direction.Right, Properties.Resources.eyesRight }
        };

        public enum ImageType { Normal, Scared, Eyes }

        public Image GetImage(ImageType imgType, GhostColor color, Direction dir, bool isSecondImage = false)
        {
            switch (imgType)
            {
                case ImageType.Normal:
                    {
                        return imgNormal[color][dir][isSecondImage ? 1 : 0];
                    }
                case ImageType.Scared:
                    {
                        return imgGhostScared;
                    }
                case ImageType.Eyes:
                    {
                        return imgGhostEyes[dir];
                    }
                default:
                    {
                        Logger.Log(TAG + "GetImage: Unknown image type");
                        return null;
                    }
            }
        }
    }

    public class GameBoardView
    {
        private string TAG = "GameBoardView";
        public GameBoardView(frmGameBoard frm)
        {
            this.frmGameBoard = frm;
        }

        private int width = 0;
        private int height = 0;
        private float cellWidth = 0;
        private float cellHeight = 0;
        frmGameBoard frmGameBoard = null;

        Graphics graphics = null;

        private List<Point> wallPoints = null;
        private List<Point> dotPoints = null;
        private List<Point> energyPoints = null;
        private List<Point> ghostPoints = null;
        private Point pacmanPoints = new Point();
        public void onInit()
        {
            Logger.Log(TAG + "onInit");
            graphics = frmGameBoard.GetPanelGameBoard().CreateGraphics();
            this.width = frmGameBoard.GetPanelGameBoard().Width;
            this.height = frmGameBoard.GetPanelGameBoard().Height;

            this.cellWidth = (float)this.width / GameBoard.Instance.GetMapWidth();
            this.cellHeight = (float)this.height / GameBoard.Instance.GetMapHeight();
        }

        public void DrawBoard()
        {
            Logger.Log(TAG + "DrawBoard");
            graphics.Clear(Color.Black);
            DrawWall();
            DrawDot();
            DrawEnergy();
            DrawPacman();
        }

        public void LoadPoints()
        {
            Logger.Log(TAG + "LoadPoints");
            wallPoints = GameBoard.Instance.GetEntityLocs(EntityType.Wall);
            dotPoints = GameBoard.Instance.GetEntityLocs(EntityType.Dot);
            energyPoints = GameBoard.Instance.GetEntityLocs(EntityType.Energy);
            ghostPoints = GameBoard.Instance.GetEntityLocs(EntityType.Ghost);
            pacmanPoints = Pacman.Instance.GetPosition();
        }

        public void DrawWall()
        {

        }

        public void DrawDot()
        {

        }

        public void DrawEnergy()
        {

        }

        public void DrawPacman()
        {

        }

        public void RemoveDot()
        {

        }

        public void RemoveEnergy()
        {

        }

        public void RemovePacman()
        {

        }

        public void RemoveGhost()
        {

        }
        private void RespawnGhost()
        {

        }

        private void RespawnPacman()
        {

        }


        private void DrawRectangle(Point point, float width, float height, Color color)
        {
            lock (graphics)
            {
                SolidBrush brush = new SolidBrush(color);
                graphics.FillRectangle(brush, point.X * cellWidth, point.Y * cellHeight, width, height);
            }
        }

        private void DrawImage(Point point, float width, float height, Image image)
        {
            lock (graphics)
            {
                graphics.DrawImage(image, point.X * cellWidth, point.Y * cellHeight, width, height);
            }
        }











        public void onEntityChange(EntityType type)
        {
            switch (type)
            {
                case EntityType.Pacman:
                    {

                        break;
                    }
                case EntityType.Ghost:
                    {
                        Logger.Log(TAG + "onEntityMove: Ghost - Update ghost view");
                        break;
                    }
                default:
                    {
                        Logger.Log(TAG + "onEntityMove: Unknown entity type" + type.ToString());
                        break;
                    }
            }
        }

        public void onPacmanCollide(int collisionType, List<Point> collisionPoints)
        {
            switch (collisionType)
            {
                case 1:
                    {
                        // Logger.Log(TAG + "onPacmanCollide: Collision detected between pacman and dot");
                        RemoveDot();
                        break;
                    }
                case 2:
                    {
                        // Logger.Log(TAG + "onPacmanCollide: Collision detected between pacman and ghost");
                        if (Pacman.Instance.GetState() == DynamicEntityState.Special)
                        {
                            RemoveGhost();
                            RespawnGhost();
                        }
                        else if (GameController.Instance.GetGameState() != GameState.GameOver)
                        {
                            RemovePacman();
                            RemoveGhost();
                            RespawnPacman();
                            RespawnGhost();
                        }

                        break;
                    }
                case 3:
                    {
                        // Logger.Log(TAG + "onPacmanCollide: Collision detected between pacman and energy");
                        RemoveEnergy();
                        break;
                    }
                default:
                    {
                        // Logger.Log(TAG + "onPacmanCollide: Unknown collision type" + collisionType.ToString());
                        break;
                    }
            }
        }
    }
}