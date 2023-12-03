using PacmanWindowForms;
using PacmanWindowForms.Forms;
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
        public GameState _gameState = GameState.GamePaused;
        public GameState gameState
        { get { return _gameState; }
          set { _gameState = value; } 
        }
        protected readonly MapLoader gameBoard;
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
            handler = new Task(MainTask);
            handler.Start();
        }
        protected abstract void MainTask();
        protected abstract void Wait();
        protected abstract Point Move(Point p, Direction dir);
        protected abstract bool IsCollision(Point curr, Point prev);
        protected abstract Point NextPoint(Point p, Direction dir);
        public abstract void Stop();
        public abstract void Draw();
    }
}