using PacmanWindowForms;
using PacmanWindowForms.Forms;
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
    public abstract class CharacterBase
    {
        protected Point point;
        protected Direction currentDirection;
        protected Point[] body = new Point[12];
        protected Point[] core;
        protected Task handler;
        public int speed = 70;
        public GameState gameState = GameState.GameOver;
        protected readonly GameBoard gameBoard;
        private bool isSprite = true;
        public abstract void Initialize();
        public abstract void Reset();
        public abstract Point[] DetermineBody(Point p);
        public abstract Point[] DetermineCore(Point p);
        public Point GetPoint()
        {
            return point;
        }
        public Point[] GetCore()
        {
            this.core = DetermineCore(point);
            return core;
        }
        public void Run()
        {
            this.gameState = GameState.GameOn;
            handler = new Task(MainTask);
            handler.Start();
        }
        private abstract void MainTask();
        private abstract void Wait();
        private abstract void Move(Point p, Direction dir);
        private abstract void Stop();
        private abstract bool IsCollision(Point curr, Point prev);
        private abstract Point NextPoint(Point p, Direction dir);
        public abstract void Draw();
    }
}