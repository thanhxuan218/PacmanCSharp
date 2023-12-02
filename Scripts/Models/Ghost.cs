using PacmanWindowForms;
using PacmanWindowsForm;
using PacmanWindowsForm.Script.Models;
using PacmanWinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PacmanWindowForms
{
    public class Ghost : CharacterBase
    {
        private readonly Dictionary<GhostColor, Point> initialPoints = new Dictionary<GhostColor, Point>()
        {
            { GhostColor.Blue, new Point(30, 28) },
            { GhostColor.Pink, new Point(26, 28) },
            { GhostColor.Red, new Point(26, 21) },
            { GhostColor.Yellow, new Point(22, 28) }
        };
        private readonly Dictionary<GhostColor, Direction> initialDirections = new Dictionary<GhostColor, Direction>()
        {
            { GhostColor.Blue, Direction.Up },
            { GhostColor.Pink, Direction.Down },
            { GhostColor.Red, Direction.Right },
            { GhostColor.Yellow, Direction.Up }
        };

        private readonly GhostColor ghostColor;
        public GhostColor Color
        {
            get
            {
                return ghostColor;
            }
        }
        private bool isSprite = true;
        public Ghost(GhostColor color)
        {
            this.ghostColor = color;
        }
        private GhostState ghostState = GhostState.Normal;
        private List<Point> wallList = null;
        private List<Point> boxDoorList = null;
        private List<Point> boxList = null;

        public GhostState GhostState
        {
            get
            {
                return ghostState;
            }
            set
            {
                ghostState = value;
            }
        }

        public override void Initialize()
        {
            boxDoorList = GameBoard.Instance.BoxDoorList();
            wallList = GameBoard.Instance.WallList();
            boxList = GameBoard.Instance.BoxList();
            wallList.OrderBy(p => p.X).ThenBy(p => p.Y);
            ghostState = GhostState.Normal;
            point = initialPoints[ghostColor];
            currentDirection = initialDirections[ghostColor];
            this.body = DetermineBody(point);
            this.core = DetermineCore(point);

            // TODO: Load from Controller
            gameState = GameState.GameOver;
        }

        public override void Reset()
        {
            ghostState = GhostState.Normal;
            point = initialPoints[ghostColor];
            currentDirection = initialDirections[ghostColor];
            this.body = DetermineBody(point);
            this.core = DetermineCore(point);
        }

        public override void Stop()
        {
            gameState = GameState.GameOver;
        }

        public override Point[] DetermineBody(Point p)
        {
            Point[] body = new Point[12];
            body[0] = p;
            body[1] = new Point(p.X + 1, p.Y);
            body[2] = new Point(p.X + 2, p.Y);

            body[3] = new Point(p.X + 3, p.Y);
            body[4] = new Point(p.X + 3, p.Y + 1);
            body[5] = new Point(p.X + 3, p.Y + 2);

            body[6] = new Point(p.X + 3, p.Y + 3);
            body[7] = new Point(p.X + 2, p.Y + 3);
            body[8] = new Point(p.X + 1, p.Y + 3);

            body[9] = new Point(p.X, p.Y + 3);
            body[10] = new Point(p.X, p.Y + 2);
            body[11] = new Point(p.X, p.Y + 1);
            return body;
        }

        public override Point[] DetermineCore(Point p)
        {
            Point[] core = new Point[4];
            core[0] = new Point(p.X + 2, p.Y + 2);
            core[1] = new Point(p.X + 2, p.Y + 3);
            core[2] = new Point(p.X + 3, p.Y + 3);
            core[3] = new Point(p.X + 3, p.Y + 2);
            return core;
        }
        private override void MainTask()
        {
            while (gameState != GameState.GameOver)
            {
                try
                {
                    this.point = Move(this.point, this.currentDirection);
                    this.Draw();
                    isSprite = !isSprite;
                    Wait();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private override void Wait()
        {
            if (ghostState == GhostState.Normal)
            {
                handler.Wait(speed + 5);
            }
            else if (ghostState == GhostState.Bonus || ghostState == GhostState.BonusEnd)
            {
                int tempSpeed = speed + 50;
                handler.Wait(tempSpeed);
            }
            else
            {
                handler.Wait(25);
            }
        }

        private override void Move(Point p, Direction dir)
        {
            Random rnd = new Random();

            List<Direction> nextPossibleDir = PossibleDirections(startPoint, d);

            if (nextPossibleDir.Count != 0)
            {
                int i = rnd.Next(0, nextPossibleDir.Count);
                d = nextPossibleDir[i];
            }

            this.point = NextPoint(startPoint, d);
            this.currentDirection = d;
            return this.point;
        }

        private override List<Direction> PossibleDirections(Point p, Direction d)
        {
            List<Direction> nextPossibleDir = new List<Direction>();
            for (Direction dir = Direction.Up; dir <= Direction.Right; dir++)
            {
                if (!IsCollision(NextPoint(p, d), p) && d != curDir && Math.Abs(d - curDir) != 2)
                {
                    nextPossibleDir.Add(d);
                }
            }
            return nextPossibleDir;
        }

        private bool IsOutOfBox(Point p)
        {
            List<Point> commonPoints = DetermineBody(p).Intersect(boxList.Select(u => u)).ToList();
            return commonPoints.Count == 0;
        }


        private bool IsCollision(Point curr, Point prev)
        {
            List<Point> mergedList = new List<Point>();
            if (IsOutOfBox(prev))
            {
                mergedList = boxDoorList.Union(wallList).ToList();
            }
            else
            {
                mergedList = wallList;
            }
            List<Point> commonPoints = DetermineBody(curr).Intersect(mergedList.Select(u => u)).ToList();

            return (commonPoints.Count != 0);
        }

        private Point NextPoint(Point p, Direction d)
        {
            Point nextP = new Point();
            nextP = p;
            switch (d)
            {
                case Direction.Up:
                    nextP.Y--;
                    break;
                case Direction.Down:
                    nextP.Y++;
                    break;
                case Direction.Left:
                    nextP.X--;
                    break;
                case Direction.Right:
                    nextP.X++;
                    break;
                default:
                    break;
            }
            return nextP;
        }

        public override void Draw()
        {
            board.DrawGhost(this.point, this.currentDirection, this.ghostColor, isSprite, this.ghostState);
        }
    }
}