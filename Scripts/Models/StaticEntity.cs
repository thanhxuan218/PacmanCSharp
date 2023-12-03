using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;
using PacmanWindowForms.Script.Models;
using PacmanWindowForms.Script.Views;
using System.Runtime.CompilerServices;

namespace PacmanWindowForms.Script.Models
{
    public class StaticEntity
    {
        private List<Point> dotList = null;
        private List<Point> bonusList = null;
        private List<Point> wallList = null;
        private List<Point> boxList = null;
        private List<Point> boxDoorList = null;
        private readonly PacmanBoard board;
        private Task handler;
        public StaticEntity(PacmanBoard b)
        {
            this.board = b;
        }

        public void Initialize()
        {
            dotList = MapLoader.Instance.DotList();
            bonusList = MapLoader.Instance.BonusList();
            wallList = MapLoader.Instance.WallList();
            boxList = MapLoader.Instance.BoxList();
            boxDoorList = MapLoader.Instance.BoxDoorList();
        }

        private int bonusStateCounter = 0;

        public void StaticEntityStart()
        {
            handler = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        //MessageBox.Show("StaticEntityStart() is called");
                        Draw();
                        handler.Wait(100);
                        bonusStateCounter++;
                        if (bonusStateCounter == 4)
                        {
                            bonusStateCounter = 0;
                        }
                    }
                    catch (Exception ex) 
                    { MessageBox.Show("StaticEntityStart" + ex.ToString()); }
                }
            });
        }

       

        public void DrawDots()
        {
            lock(dotList)
            {
                foreach (Point p in dotList)
                {
                    board.DrawDot(p, Color.White);
                }
            }
        }

        public void DrawBonus()
        {
            lock (this)
            {
                foreach (Point p in bonusList)
                {
                    board.DrawBonus(p, Color.White, this.bonusStateCounter);
                }
            }
        }

        private bool isDrawWalls = false;
        public void DrawWalls()
        {
            foreach (Point p in wallList)
            {
                board.DrawRect(p, Color.RoyalBlue);
            }
        }

        public void DrawBoxes()
        {
            foreach (Point p in boxList)
            {
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