using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PacmanWindowForms.Scripts.Models;
using PacmanWindowForms;
using PacmanWindowForms.Scripts.Views;

namespace PacmanWindowForms.Scripts.Controllers
{
    public class MapController
    {
        // private fields
        private static MapController instance = null;
        const string MAP_FILE_NAME_PATTERN = "map_{0}.txt";
        const string CFG_FILE_NAME_PATTERN = "cfg_{0}.txt";

        string mapFileName = "";
        string mapConfigFileName = "";

        int mapWidth = 0;
        int mapHeight = 0;

        private MapController() { }

        public static MapController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapController();
                }
                return instance;
            }
        }

        public int onLoadMap(int level)
        {

            // File is stored in Resource/Maps folder

            this.mapFileName = Utilities.GetFullPathMap(string.Format(MAP_FILE_NAME_PATTERN, level));
            this.mapConfigFileName = Utilities.GetFullPathMap(string.Format(CFG_FILE_NAME_PATTERN, level));

            // Hard code for testing

            if (string.IsNullOrEmpty(mapFileName) || string.IsNullOrEmpty(mapConfigFileName) ||
                !File.Exists(mapFileName) || !File.Exists(mapConfigFileName))
            {
                Logger.Log($"Invalid map file [{mapFileName}] or config file [{mapConfigFileName}]");
                return -1;
            }

            // Reset all entities
            this.ResetEntities();

            // Load new data
            // this.LoadStaticEntities();
            // this.LoadDynamicEntities();

            return 0;
        }
        // privte methods
        private EntityType GetEntityType(char c)
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

        // This method is used to load all static entities from the map file
        // return 0 on success, other is error code
        private int LoadStaticEntities()
        {
            // Load static entities
            // Static entities's information is stored in a text file
            // File name follow pattern "map" + level + ".txt"
            // The map is a 2D array of characters
            // With each character, we can determine the type of the entity
            // "0" - Dot
            // "1" - Wall
            // "2" - Dot
            // "3" - Energy

            // Read all lines from the map file
            string[] lines = System.IO.File.ReadAllLines(mapFileName);
            string staticPostfix = "";

            if (lines.Length == 0)
            {
                Logger.Log("Invalid map file");
                return -1;
            }

            this.mapWidth = lines[0].Length;
            this.mapHeight = lines.Length;

            for (int i = 0; i < this.mapHeight; i++)
            {
                string line = lines[i];
                if (line.Length != this.mapWidth)
                {
                    Logger.Log("Map file is miss matching!!! Please check again line " + i);
                    // return -1;
                }
                for (int j = 0; j < this.mapWidth; j++)
                {
                    char c = line[j];
                    EntityType entityType = this.GetEntityType(c);
                    if (entityType != EntityType.None)
                    {
                        // Get the entity from the factory
                        Entity entity = EntityFactory.Instance.GetEntity(entityType, staticPostfix);

                        // Incase of null, create a new entity and add it to the factory
                        if (entity == null)
                        {
                            // Create a new entity
                            entity = EntityFactory.Instance.CreateEntity(entityType);
                            EntityFactory.Instance.AddEntity(entity, staticPostfix);
                        }

                        if (entity == null)
                        {
                            Logger.Log("Invalid entity type for static entity: " + entityType);
                            return -1;
                        }
                        else
                        {
                            // Logger.Log("Valid entity type for static entity: " + entityType);
                            // Set the position for the entity
                            entity.AddPoint(new Point(j, i));
                        }
                    }
                }
            }
            Logger.Log("Load static entities successfully");
            return 0;
        }

        private GhostColor GetGhostColor(string color)
        {
            if (color.ToLower() == "red")
            {
                return GhostColor.Red;
            }
            else if (color.ToLower() == "blue")
            {
                return GhostColor.Blue;
            }
            else if (color.ToLower() == "pink")
            {
                return GhostColor.Pink;
            }
            else if (color.ToLower() == "yellow")
            {
                return GhostColor.Yellow;
            }
            else
            {
                return GhostColor.None;
            }
        }

        // This method is used to load all dynamic entities from the map config file
        // 
        private int LoadDynamicEntities()
        {
            // Format of Config file must follow this pattern
            // 
            // <entity type> <x> <y> <speed> <lives> <optional>
            // Example:
            // Pacman 1 1 1 3
            // Ghost 1 1 1 9999 Red
            // Ghost 1 1 1 9999 Blue
            // Ghost 1 1 1 9999 Pink
            // Ghost 1 1 1 9999 Orange

            // Read all lines from the map config file
            string[] lines = System.IO.File.ReadAllLines(mapConfigFileName);
            // For pacman dynamic postfix is ""
            // For ghost dynamic postfix is ghostColor: UPPER CASE
            string dynamicPostfix = "";
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] tokens = line.Split(' ');

                string identify = tokens[0];
                if (identify.ToLower() != "pacman" && identify.ToLower() != "ghost")
                {
                    Logger.Log($"Invalid entity type: [{identify}]");
                    return -1;
                }

                int x = int.Parse(tokens[1]);
                int y = int.Parse(tokens[2]);
                int speed = int.Parse(tokens[3]);
                int lives = int.Parse(tokens[4]);
                EntityType entityType = EntityType.None;

                if (identify.ToLower() == "ghost") // Ghost
                {
                    entityType = EntityType.Ghost;
                    dynamicPostfix = tokens[5].ToUpper();
                }
                else // Pacman
                {
                    entityType = EntityType.Pacman;
                    dynamicPostfix = "";
                }
                // Get pacman or ghost from the factory
                Entity entity = EntityFactory.Instance.GetEntity(entityType, dynamicPostfix);
                if (entity == null)
                {
                    // Create a new entity
                    entity = EntityFactory.Instance.CreateEntity(entityType);
                    EntityFactory.Instance.AddEntity(entity, dynamicPostfix);
                }

                // Set the position, speed and lives for the entity
                ((DynamicEntity)entity).SetPosition(new Point(x, y));
                ((DynamicEntity)entity).SetLives(lives);
                ((DynamicEntity)entity).SetSpeed(speed);
                if (entityType == EntityType.Ghost)
                {
                    ((Ghost)entity).SetGhostColor(this.GetGhostColor(dynamicPostfix));
                }
                Logger.Log($"Load entity {entityType} at ({x}, {y}) with speed {speed} and lives {lives}");
                // Add the entity to the map
            }
            Logger.Log("Load dynamic entities successfully");
            return 0;
        }


        // This method is used to reset all entities to their initial state
        private void ResetEntities()
        {
            EntityFactory.Instance.Reset();
        }
        public void onDestroyMap() { }
        public int GetBoardHeight()
        {
            return this.mapHeight;
        }

        public int GetBoardWidth()
        {
            return this.mapWidth;
        }

        List<Point> wallPoints = new List<Point>();
        List<Point> dotPoints = new List<Point>();
        List<Point> energyPoints = new List<Point>();
        List<Point> pacmanPoints = new List<Point>();
        List<Point> ghostPoints = new List<Point>();

        public EntityType IsWall(Point point)
        {
            if (wallPoints.Contains(point))
            {
                return EntityType.Wall;
            }
            return EntityType.None;
        }

        public EntityType IsDot(Point point)
        {
            if (dotPoints.Contains(point))
            {
                return EntityType.Dot;
            }
            return EntityType.None;
        }

        public EntityType IsEnergy(Point point)
        {
            if (energyPoints.Contains(point))
            {
                return EntityType.Energy;
            }
            return EntityType.None;
        }

        public EntityType IsPacman(Point point)
        {
            if (pacmanPoints.Contains(point))
            {
                return EntityType.Pacman;
            }
            return EntityType.None;
        }

        public EntityType IsGhost(Point point)
        {
            if (ghostPoints.Contains(point))
            {
                return EntityType.Ghost;
            }
            return EntityType.None;
        }

        public void RemoveDot(Point point)
        {
            if (dotPoints.Contains(point))
            {
                dotPoints.Remove(point);
            }
        }

        public void RemoveEnergy(Point point)
        {
            if (energyPoints.Contains(point))
            {
                energyPoints.Remove(point);
            }
        }
        public void RemovePacman(Point point)
        {
            if (pacmanPoints.Contains(point))
            {
                pacmanPoints.Remove(point);
            }
        }

    }
}
