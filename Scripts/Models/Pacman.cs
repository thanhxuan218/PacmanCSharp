using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PacmanWindowForms.Script.Models;
using PacmanWindowForms.Script.Views;

namespace PacmanWindowForms.Script.Models
{
    public class PacmanCharacter : CharacterBase
    {
        private Direction nextDirection = Direction.None;
        private List<Point> wallList = null;
        private List<Point> boxDoorList = null;
        // TODO: Hanle GameBoard
        private readonly PacmanBoard board;

        public Point GetPoint()
        {
            return point;
        }

        public PacmanCharacter(PacmanBoard b)
        {
            this.board = b;
        }

        public override void Initialize()
        {
            boxDoorList = MapLoader.Instance.BoxDoorList();
            wallList = MapLoader.Instance.WallList();
            wallList.OrderBy(p => p.X).ThenBy(p => p.Y);
            point = new Point(26, 39);
            currentDirection = Direction.None;
            nextDirection = Direction.None;
            body = DetermineBody(point);
            core = DetermineCore(point);
            gameState = GameState.GamePaused;
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
            handler = new Task(() =>
            {
                while (gameState != GameState.GameOver)
                {
                    try
                    {
                        RemovePacman(this.point);
                        this.point = Move(this.point, currentDirection);
                        Debug.WriteLine($"Pacman move to {this.point.ToString()}");
                        Draw();
                        Wait();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            });
            handler.Start();
        }

        protected override void Wait()
        {
            handler.Wait(speed);
        }

        public void RemovePacman(Point point)
        {
            board.ClearPacMan(point);
        }
        bool isMoving = false;
        public override void Draw()
        {
            board.DrawPacMan(point, Color.Yellow, currentDirection);
        }
        protected override Point Move(Point startPoint, Direction direction)
        {
            if (gameState != GameState.GameOn)
            {
                Debug.WriteLine($"Pacman Keep current Point");
                return startPoint;
            }
            Point next;
            bool passed = false;

            for (Direction d = Direction.Up; d <= Direction.Right; d++)
            {
                next = NextPoint(startPoint, d);
                bool isColliding = !IsCollision(next, startPoint);
                if (isColliding && direction == nextDirection && nextDirection != Direction.None)
                {
                    currentDirection = nextDirection;
                    nextDirection = Direction.None;
                    return next;
                }
                else if (isColliding && direction == d)
                {
                    passed = true;
                }
            }
            
            if (passed)
            {
                return NextPoint(startPoint, direction);
            }

            nextDirection = Direction.None;
            return startPoint;
        }
        public void SetDirection(Direction d)
        {
            if (!IsCollision(NextPoint(point, d), this.point))
            {
                currentDirection = d;
                nextDirection = Direction.None;
            }
            else
            {
                    nextDirection = d;
            }

        }

        public override void Reset()
        {
            point = new Point(26, 39);
        }

        protected override bool IsCollision(Point curr, Point prev)
        {
            List<Point> mergedList = new List<Point>();
            mergedList = boxDoorList.Union(wallList).ToList();
            List<Point> commonPoints = DetermineBody(curr).Intersect(mergedList.Select(u => u)).ToList();

            return commonPoints.Count != 0;
        }

        protected override Point NextPoint(Point p, Direction dir)
        {
            Point next = p;
            switch (dir)
            {
                case Direction.Up:
                    next.Y--;
                    break;
                case Direction.Down:
                    next.Y++;
                    break;
                case Direction.Right:
                    next.X++;
                    break;
                case Direction.Left:
                    next.X--;
                    break;
                case Direction.None:

                    break;
            }
            return next;
        }
    }
}
