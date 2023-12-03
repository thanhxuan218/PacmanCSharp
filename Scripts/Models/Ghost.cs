using PacmanWindowForms.Script.Views;
using PacmanWindowForms.Script.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacmanWindowForms.Script.Models
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
        public Ghost(PacmanBoard board, GhostColor color)
        {
            this.board = board;
            this.ghostColor = color;
        }
        private GhostState ghostState = GhostState.Normal;
        private List<Point> wallList = null;
        private List<Point> boxDoorList = null;
        private List<Point> boxList = null;
        private readonly PacmanBoard board;

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
            boxDoorList = MapLoader.Instance.BoxDoorList();
            wallList = MapLoader.Instance.WallList();
            boxList = MapLoader.Instance.BoxList();
            wallList.OrderBy(p => p.X).ThenBy(p => p.Y);
            ghostState = GhostState.Normal;
            point = initialPoints[ghostColor];
            currentDirection = initialDirections[ghostColor];
            this.body = DetermineBody(point);
            this.core = DetermineCore(point);
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
        protected override void MainTask()
        {
            while (gameState != GameState.GameOver)
            {
                try
                {
                    this.point = Move(this.point, this.currentDirection);
                    this.Draw();
                    isSprite = !isSprite;
                    Wait();
                    //Debug.WriteLine($"{Color} - gameState {gameState}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        protected override void Wait()
        {
            if (ghostState == GhostState.Normal)
            {
                handler.Wait(speed);
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

        protected override Point Move(Point startPoint, Direction dir)
        {
            if (gameState == GameState.GameOn)
            {
                if (!IsCollision(NextPoint(startPoint, dir), startPoint))
                {
                    return NextPoint(startPoint, dir);
                }

                Random rnd = new Random();

                List<Direction> nextPossibleDir = PossibleDirections(startPoint, dir);

                if (nextPossibleDir.Count != 0)
                {
                    int i = rnd.Next(0, nextPossibleDir.Count);
                    dir = nextPossibleDir[i];
                }

                this.point = NextPoint(startPoint, dir);
                this.currentDirection = dir;
                return this.point;
            }
            return startPoint;
        }

        private List<Direction> PossibleDirections(Point p, Direction currDir)
        {
            List<Direction> nextPossibleDir = new List<Direction>();
            for (Direction dir = Direction.Up; dir <= Direction.Right; dir++)
            {
                if (!IsCollision(NextPoint(p, dir), p) && dir != currDir && Math.Abs(dir - currDir) != 2)
                {
                    nextPossibleDir.Add(dir);
                }
            }
            return nextPossibleDir;
        }

        private bool IsOutOfBox(Point p)
        {
            //MessageBox.Show($"IsOutOfBox is called by {Color.ToString()}");
            List<Point> commonPoints = DetermineBody(p).Intersect(boxList.Select(u => u)).ToList();
            //Debug.WriteLine($"IsOutOfBox is called by {Color.ToString()} - is {commonPoints.Count}");
            return commonPoints.Count == 0;
        }
        protected override bool IsCollision(Point curr, Point prev)
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

        protected override Point NextPoint(Point p, Direction d)
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