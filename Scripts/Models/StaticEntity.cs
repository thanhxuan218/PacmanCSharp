using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;
using PacmanWindowsForm.Script.Models;

namespace PacmanWindowForms.Script.Models
{
    public class StaticEntity
    {
        private List<Point> dotList = null;
        private List<Point> bonusList = null;
        private List<Point> wallList = null;
        private List<Point> boxList = null;
        private readonly PacmanBoard board;
        private Task handler;
        StaticEntity(PacmanBoard b)
        {
            this.board = b;
        }

        public void Initialize()
        {
            dotList = GameBoard.Instance.DotList();
            bonusList = GameBoard.Instance.BonusList();
            wallList = GameBoard.Instance.WallList();
            boxList = GameBoard.Instance.BoxList();
        }

        private int bonusStateCounter = 0;
        private int bonusState = 0;

        public void StaticEntityStart()
        {
            handler = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        DoorPaint();
                        WallPaint();
                        Thread.Sleep(100);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }
            });
            handler.Start();
        }

        public void DrawDots()
        {
            foreach (Point p in dotList)
            {
                board.DrawDot(p);
            }
        }

        public void DrawBonus()
        {
            lock (this.bonusState)
            {
                foreach (Point p in bonusList)
                {
                    board.DrawBonus(p, Color.White, this.bonusState);
                }
            }
        }

        public void DrawWalls()
        {
            foreach (Point p in wallList)
            {
                board.DrawWall(p, Color.RoyalBlue);
            }
        }

        public void DrawBoxes()
        {
            foreach (Point p in boxList)
            {
                board.DrawBox(p);
            }
        }

        public void DrawDoor()
        {
            foreach (Point p in boxDoorList)
            {
                board.DrawDoor(p, Color.SlateBlue);
            }
        }

        public void BonusClear()
        {
            if (bonusList.Count > 0)
            {
                foreach (Point p in bonusList)
                {
                    board.ClearBonus(p);
                }
            }
        }

        public void Draw()
        {
            DrawDots();
            DrawBonus();
            DrawWalls();
            DrawBoxes();
            DrawDoor();
        }
    }
}