using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms.PropertyGridInternal;
using System.Reflection;
using System.Windows.Forms;

namespace PacmanWindowForms.Script.Models
{
    public class MapLoader
    {
        private MapLoader() { }

        private static MapLoader _instance;

        public static MapLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MapLoader();
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
        private Dictionary<int, string> maps = new Dictionary<int, string>()
        {
            { 1, Properties.Resources.map },

        };

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

        private bool isLoading = false;
        public void LoadBoard(int level = 1, char loadType = LoadTypeAll)
        {

            if (isLoaded == true)
            {
                return;
            }

            isLoading = true;



            if (mapBuffer.Length == 0)
            {
                mapBuffer = maps[level];
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

            // MessageBox.Show($"Load map with size {this.boardWidth}x{this.BoardHeight}");

            isLoaded = true;
            isLoading = false;

        }

        public List<Point> DotList()
        {
            while(isLoading == true)
            {

            }

            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeDots);
            }
            return  dotList;
        }

        public  List<Point> BonusList()
        {
            while (isLoading == true)
            {

            }
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeBonus);
            }
            return  bonusList;
        }

        public  List<Point> WallList()
        {
            while (isLoading == true)
            {

            }
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeWalls);
            }
            return  wallList;
        }

        public  List<Point> BoxList()
        {
            while (isLoading == true)
            {

            }
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeBoxes);
            }
            return  boxList;
        }

        public  List<Point> BoxDoorList()
        {
            while (isLoading == true)
            {

            }
            if (!isLoaded)
            {
                LoadBoard(loadType: LoadTypeDoors);

                MessageBox.Show("BoxDoorList" + boxDoorList.Count);
            }
            return  boxDoorList;
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