using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms.PropertyGridInternal;
using System.Reflection;
using PacmanWinForms;
using System.Windows.Forms;

namespace PacmanWindowsForm.Script.Models
{
    public class GameBoard
    {
        private GameBoard() { }

        private static GameBoard _instance;

        public static GameBoard Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameBoard();
                }

                return _instance;
            }
        }
        private List<Point> dotList = new List<Point>();
        private List<Point> bonusList = new List<Point>();
        private List<Point> wallList = new List<Point>();
        private List<Point> boxList = new List<Point>();
        private List<Point> boxDoorList = new List<Point>();
        private bool isLoaded = false;
        private static string mapBuffer = new string(new char[0]);
        private const string MapFilePath = @"D:\00.Temp\Pacman_Ref\PacmanWinForms\Resources\map.txt";
        private const char LoadTypeAll = 'a';
        private const char LoadTypeDots = 'o';
        private const char LoadTypeWalls = 'x';
        private const char LoadTypeBoxes = 'X';
        private const char LoadTypeDoors = 'D';
        private const char LoadTypeBonus = 'b';
        private int boardWidth = 0;
        private int boardHeight = 0;
        public int BoardWidth
        {
            get
            {
                if (boardWidth == 0)
                {
                    boardWidth = mapBuffer.Split('\n')[0].Length;
                }
                return boardWidth;
            }
        }

        public int BoardHeight
        {
            get
            {
                if (boardHeight == 0)
                {
                    boardHeight = mapBuffer.Split('\n').Length;
                }
                return boardHeight;
            }
        }

        public void LoadBoard(int level = 1, char loadType = LoadTypeAll)
        {
            if (mapBuffer.Length == 0)
            {
                mapBuffer = File.ReadAllText(MapFilePath);
            }

            bool isUpdate = (loadType == LoadTypeAll);

            int y = 0;
            foreach (string line in mapBuffer.Split('\n'))
            {
                int x = 0;
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'o':
                            if (isUpdate || loadType == LoadTypeDots)
                                dotList.Add(new Point(x, y));
                            break;
                        case 'x':
                            if (isUpdate || loadType == LoadTypeWalls)
                                wallList.Add(new Point(x, y));
                            break;
                        case 'X':
                            if (isUpdate || loadType == LoadTypeBoxes)
                                boxList.Add(new Point(x, y));
                            break;
                        case 'D':
                            if (isUpdate || loadType == LoadTypeDoors)
                            {
                                boxDoorList.Add(new Point(x, y));
                                boxList.Add(new Point(x, y));
                            }
                            break;
                        case 'b':
                            if (isUpdate || loadType == LoadTypeBonus)
                                bonusList.Add(new Point(x, y));
                            break;
                        case 'B':
                            if (isUpdate || loadType == LoadTypeBoxes || loadType == LoadTypeWalls)
                            {
                                wallList.Add(new Point(x, y));
                                boxList.Add(new Point(x, y));
                            }
                            break;
                    }
                    x++;
                }
                y++;
            }

            this.boardWidth = mapBuffer.Split('\n')[0].Length;
            this.boardHeight = mapBuffer.Split('\n').Length;

            isLoaded = true;
        }

        public ref List<Point> DotList()
        {
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeDots);
            }
            return ref dotList;
        }

        public ref List<Point> BonusList()
        {
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeBonus);
            }
            return ref bonusList;
        }

        public ref List<Point> WallList()
        {
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeWalls);
            }
            return ref wallList;
        }

        public ref List<Point> BoxList()
        {
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeBoxes);
            }
            return ref boxList;
        }

        public ref List<Point> BoxDoorList()
        {
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeDoors);

                MessageBox.Show("BoxDoorList" + boxDoorList.Count);
            }
            return ref boxDoorList;
        }

        public void Reset()
        {
            isLoaded = false;

            dotList.Clear();
            wallList.Clear();
            boxDoorList.Clear();
            boxList.Clear();
            bonusList.Clear();
        }
    }
}