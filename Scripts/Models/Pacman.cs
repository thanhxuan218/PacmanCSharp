using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PacmanWindowForms.Script.Models;

namespace PacmanWindowForms
{
    public class Pacman : CharacterBase
    {
        private Direction nextDirection = Direction.Stop;
        private List<Point> wallList = null;
        private List<Point> boxDoorList = null;
        // TODO: Hanle GameBoard
        private readonly PacmanBoard board;

        public Point GetPoint()
        {
            return point;
        }

        public PacmanCharacter(frmPacmanGame frm, PacmanBoard b)
        {
            this.gameForm = frm;
            this.board = b;
        }

        public void Initialize()
        {
            boxDoorList = GameBoard.Instance.BoxDoorList();
            wallList = GameBoard.Instance.WallList();
            wallList.OrderBy(p => p.X).ThenBy(p => p.Y);
            point = new Point(26, 39);
            currentDirection = Direction.Stop;
            nextDirection = Direction.Stop;
            body = DetermineBody(point);
            core = DetermineCore(point);
            gameState = GameState.GameOver;
        }

        public void Stop()
        {
            gameState = GameState.GameOver;
        }


        public Point[] DetermineBody(Point p)
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

        public Point[] DetermineCore(Point p)
        {
            Point[] core = new Point[4];
            core[0] = new Point(p.X + 2, p.Y + 2);
            core[1] = new Point(p.X + 2, p.Y + 3);
            core[2] = new Point(p.X + 3, p.Y + 3);
            core[3] = new Point(p.X + 3, p.Y + 2);
            return core;
        }
        public void Run()
        {
            gameState = GameState.GameRun;
            handler = new Task(() =>
            {
                while (gameState != GameState.GameOver)
                {
                    try
                    {
                        RemovePacman(this.point);
                        this.point = Move(this.point);
                        Draw();
                        handler.Wait(speed);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            });
            handler.Start();
        }

        public void RemovePacman(Point point)
        {
            board.ClearPacMan(point);
        }

        public override void Draw()
        {
            board.DrawPacMan(point, Color.Yellow, currentDirection);
        }
        private Point Move(Point startPoint)
        {
            if (gameState != GameState.GameRun)
            {
                return startPoint;
            }
            Point next;

            for (Direction d = Direction.Up; d <= Direction.Right; d++)
            {
                next = NextPoint(startPoint, d);
                if (!IsCollision(next, startPoint))
                {
                    if (d == nextDirection)
                    {
                        currentDirection = nextDirection;
                        nextDirection = Direction.Stop;
                        return nextP;
                    }
                    else if (d == currentDirection)
                    {
                        return nextP;
                    }
                }
            }
            return startPoint;
        }
        public void SetDirection(Direction d)
        {
            if (!IsCollision(this.point, NextPoint(point, d)))
            {
                currentDirection = d;
                nextDirection = Direction.Stop;
            }
            else
            {
                nextDirection = d;
            }
        }

        public void Reset()
        {
            point = new Point(26, 39);
        }

        private override bool IsCollision(Point curr, Point prev)
        {
            List<Point> mergedList = new List<Point>();
            mergedList = boxDoorList.Union(wallList).ToList();
            List<Point> commonPoints = DetermineBody(curr).Intersect(mergedList.Select(u => u)).ToList();
            return commonPoints.Count == 0;
        }

        private override Point NextPoint(Point p, Direction dir)
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
                case Direction.Stop:

                    break;
            }
            return next;
        }
    }
}
