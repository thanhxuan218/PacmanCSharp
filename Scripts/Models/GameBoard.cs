using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacmanWindowForms.Scripts.Models;
using PacmanWindowForms.Scripts.Views;

namespace PacmanWindowForms.Scripts.Models
{

    public class EntityParams
    {
        public Point position = new Point(0, 0);
        public int width = 0;
        public int height = 0;
        public int speed = 0;
        public Direction direction = Direction.None;
        public int lives = 0;

        public override string ToString()
        {
            return "Position: " + position.ToString() + " Width: " + width + " Height: " + height + " Speed: " + speed + " Direction: " + direction.ToString() + " Lives: " + lives;
        }
    }

    public class GameBoard
    {
        // For Singleton Pattern implementation
        private static GameBoard gameBoard = null;
        private GameBoard() { }
        public static GameBoard Instance
        {
            get
            {
                if (gameBoard == null)
                {
                    gameBoard = new GameBoard();
                }
                return gameBoard;
            }
        }

        // GameBoard Properties
        private Dictionary<EntityType, List<Point>> entityLocs = new Dictionary<EntityType, List<Point>>();
        private GameBoardLoader loader = new GameBoardLoader();

        // Load the game board
        public int onInit(int level)
        {
            Logger.Log("GameBoard onInit");
            return loader.LoadMap(level, ref entityLocs);
        }

        public int RemoveEntityAt(EntityType type, Point loc)
        {
            lock (this)
            {
                // Logger.Log($"onRequest Remove entity {type} at {loc}");
                if (entityLocs.ContainsKey(type))
                {
                    // Logger.Log($"Remove entity {type} at {loc}");
                    entityLocs[type].Remove(loc);
                    return 0;
                }
                return -1;
            }
        }
        public int AddEntityAt(EntityType type, Point loc)
        {
            lock (this)
            {
                // Logger.Log($"onRequest Add entity {type} at {loc}");
                if (entityLocs.ContainsKey(type))
                {
                    // Logger.Log($"Add entity {type} at {loc}");
                    entityLocs[type].Add(loc);
                    return 0;
                }
                return -1;
            }
        }

        public void CleanUp()
        {
            entityLocs.Clear();
            loader.ClearMap();
        }


        // Getters and Setters
        public List<Point> GetEntityLocs(EntityType type)
        {
            if (entityLocs.ContainsKey(type))
            {
                return entityLocs[type];
            }
            return null;
        }

        public EntityType EntityTypeAt(Point loc)
        {
            foreach (KeyValuePair<EntityType, List<Point>> entry in entityLocs)
            {
                if (entry.Value.Contains(loc))
                {
                    return entry.Key;
                }
            }
            return EntityType.None;
        }

        public int GetMapWidth()
        {
            return loader.GetMapWidth();
        }

        public int GetMapHeight()
        {
            return loader.GetMapHeight();
        }

        public bool IsPossibleLocation(Point loc)
        {
            if (loc.X < 0 || loc.X >= GetMapWidth() || loc.Y < 0 || loc.Y >= GetMapHeight())
            {
                return false;
            }

            if (EntityTypeAt(loc) == EntityType.Wall)
            {
                return false;
            }

            return true;
        }

        public Point NextLocation(Point loc, Direction direction)
        {
            Point nextLoc;
            switch (direction)
            {
                case Direction.Up:
                    // Logger.Log("Direction: Up");
                    nextLoc = new Point(loc.X, loc.Y - 1);
                    break;
                case Direction.Down:
                    // Logger.Log("Direction: Down");
                    nextLoc = new Point(loc.X, loc.Y + 1);
                    break;
                case Direction.Left:
                    // Logger.Log("Direction: Left");
                    nextLoc = new Point(loc.X - 1, loc.Y);
                    break;
                case Direction.Right:
                    // Logger.Log("Direction: Right");
                    nextLoc = new Point(loc.X + 1, loc.Y);
                    break;
                default:
                    // Logger.Log("Direction: None");
                    nextLoc = new Point(loc.X, loc.Y);
                    break;
            }
            // Logger.Log("NextLocation: " + nextLoc.ToString());
            return nextLoc;
        }

        public EntityParams GetPacmanParams()
        {
            return loader.GetPacmanParams();
        }

        public Dictionary<GhostColor, EntityParams> GetGhostParams()
        {
            return loader.GetGhostParams();
        }

        // Debug method
        /// <summary>
        ///   Print the map to the console
        ///   
        /// </summary>
        public void PrintMap()
        {
            lock (this)
            {
                for (int row = 0; row < GetMapHeight(); row++)
                {
                    for (int col = 0; col < GetMapWidth(); col++)
                    {
                        Point loc = new Point(col, row);
                        EntityType type = EntityTypeAt(loc);
                        switch (type)
                        {
                            case EntityType.Wall:
                                Console.Write("0");
                                break;
                            case EntityType.Dot:
                                Console.Write("2");
                                break;
                            case EntityType.Energy:
                                Console.Write("8");
                                break;
                            case EntityType.Pacman:
                                Console.Write("p");
                                break;
                            case EntityType.Ghost:
                                Console.Write("g");
                                break;
                            default:
                                Console.Write(" ");
                                break;
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
    }

    internal class GameBoardLoader
    {
        const string MAP_FILE_NAME_PATTERN = "map_{0}.txt";
        const string CFG_FILE_NAME_PATTERN = "cfg_{0}.txt";

        public int mapWidth;
        public int mapHeight;
        private EntityParams pacmanParams = null;
        private Dictionary<GhostColor, EntityParams> ghostParams = null;
        public int LoadMap(int level, ref Dictionary<EntityType, List<Point>> containers)
        {
            int retCode = 0;

            string mapFileName = Utilities.GetFullPathMap(string.Format(MAP_FILE_NAME_PATTERN, level));
            string mapConfigFileName = Utilities.GetFullPathMap(string.Format(CFG_FILE_NAME_PATTERN, level));

            if (!IsMapFileValid(mapFileName, mapConfigFileName))
            {
                return -1;
            }

            retCode = LoadStaticEntities(mapFileName, containers);

            if (retCode == 0)
            {
                retCode = LoadDynamicEntities(mapConfigFileName, containers);
            }

            return retCode;
        }

        private int LoadStaticEntities(string mapFileName, Dictionary<EntityType, List<Point>> containers)
        {
            int retCode = 0;

            string[] lines = System.IO.File.ReadAllLines(mapFileName);

            if (lines.Length == 0)
            {
                Logger.Log("Map file is empty");
                return -1;
            }

            this.mapWidth = lines[0].Length;    // Column count
            this.mapHeight = lines.Length;      // Row count

            for (int row = 0; row < this.mapHeight; row++)
            {
                string line = lines[row];

                if (line.Length != this.mapWidth)
                {
                    Logger.Log("Map file is invalid");
                    return -1;
                }

                for (int col = 0; col < this.mapWidth; col++)
                {
                    char c = line[col];
                    EntityType type = CharToEntityType(c);
                    if (type != EntityType.None)
                    {
                        Point loc = new Point(col, row);
                        if (containers.ContainsKey(type))
                        {
                            containers[type].Add(loc);
                        }
                        else
                        {
                            containers.Add(type, new List<Point>() { loc });
                        }
                    }
                }
            }
            Logger.Log("Map file is loaded successfully");
            return retCode;
        }

        private int LoadDynamicEntities(string mapConfigFileName, Dictionary<EntityType, List<Point>> containers)
        {
            int retCode = 0;
            string[] lines = System.IO.File.ReadAllLines(mapConfigFileName);

            if (lines.Length == 0)
            {
                Logger.Log("Map config file is empty");
                return -2;
            }

            foreach (string line in lines)
            {
                string[] tokens = line.Split(' ');
                char c = tokens[0][0].ToString().ToLower()[0];
                EntityType type = CharToEntityType(c);
                if (type == EntityType.None)
                {
                    Logger.Log("Map config file is invalid - Unknown entity type");
                    return -1;
                }

                int x = int.Parse(tokens[1]);
                int y = int.Parse(tokens[2]);
                int width = int.Parse(tokens[3]);
                int height = int.Parse(tokens[4]);
                int speed = int.Parse(tokens[5]);
                int lives = int.Parse(tokens[6]);
                Direction direction = (Direction)Enum.Parse(typeof(Direction), tokens[7]);

                EntityParams entityParams = new EntityParams()
                {
                    position = new Point(x, y),
                    width = width,
                    height = height,
                    speed = speed,
                    direction = direction,
                    lives = lives
                };

                if (type == EntityType.Pacman)
                {
                    this.pacmanParams = entityParams;
                }
                else if (type == EntityType.Ghost)
                {
                    if (this.ghostParams == null)
                    {
                        this.ghostParams = new Dictionary<GhostColor, EntityParams>();
                    }
                    GhostColor color = (GhostColor)Enum.Parse(typeof(GhostColor), tokens[7]);
                    if (this.ghostParams.ContainsKey(color))
                    {
                        Logger.Log("Map config file is invalid - Duplicate ghost color: " + color.ToString());
                        return -1;
                    }
                    this.ghostParams.Add(color, entityParams);
                }

                Point loc = new Point(x, y);
                if (containers.ContainsKey(type))
                {
                    containers[type].Add(loc);
                }
                else
                {
                    containers.Add(type, new List<Point>() { loc });
                }
            }
            Logger.Log("Map config file is loaded successfully");
            Logger.Log("Pacman params: " + this.pacmanParams.ToString());

            foreach (KeyValuePair<GhostColor, EntityParams> entry in this.ghostParams)
            {
                Logger.Log("Ghost params: " + entry.Value.ToString());
            }


            return retCode;
        }

        private bool IsMapFileValid(string mapFileName, string mapConfigFileName)
        {
            if (string.IsNullOrEmpty(mapFileName) || string.IsNullOrEmpty(mapConfigFileName))
            {
                return false;
            }

            if (System.IO.File.Exists(mapFileName) && System.IO.File.Exists(mapConfigFileName))
            {
                return true;
            }

            return false;
        }

        private EntityType CharToEntityType(char c)
        {
            switch (c)
            {
                case '0':
                    return EntityType.Wall;
                case '1':
                    return EntityType.None;
                case '2':
                    return EntityType.Dot;
                case '8':
                    return EntityType.Energy;
                case 'p':
                    return EntityType.Pacman;
                case 'g':
                    return EntityType.Ghost;
                default:
                    return EntityType.None;
            }
        }
        public int GetMapWidth()
        {
            return this.mapWidth;
        }
        public int GetMapHeight()
        {
            return this.mapHeight;
        }

        public EntityParams GetPacmanParams()
        {
            return this.pacmanParams;
        }

        public Dictionary<GhostColor, EntityParams> GetGhostParams()
        {
            return this.ghostParams;
        }

        public int ClearMap()
        {
            this.mapWidth = 0;
            this.mapHeight = 0;
            this.pacmanParams = null;
            this.ghostParams = null;
            return 0;
        }

        // Debug method
        public void PrintMap(Dictionary<EntityType, List<Point>> containers)
        {
            foreach (KeyValuePair<EntityType, List<Point>> entry in containers)
            {
                Logger.Log("EntityType: " + entry.Key);
                foreach (Point loc in entry.Value)
                {
                    Logger.Log("Point: " + loc);
                }
            }
        }
    }
}



