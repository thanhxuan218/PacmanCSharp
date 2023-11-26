using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanWindowForms.Scripts.Models
{
    // Class for implementing the factory pattern
    // The factory pattern is used to create entities
    public class EntityFactory
    {
        private static EntityFactory instance = null;
        private static readonly object padlock = new object();
        private EntityFactory() { }
        public static EntityFactory Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EntityFactory();
                    }
                    return instance;
                }
            }
        }

        // Dictionary for storing entities with key as entity type + "postfix"
        private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();

        // Method for adding an entity to the dictionary if it doesn't exist
        // return 0 on success, other is error code
        // 1 - entity already exists
        // 2 - entity is null
        public int AddEntity(Entity entity, string postfix)
        {
            if (entity == null)
            {
                return 2;
            }

           string key = entity.entityType.ToString() + postfix;
            
            if (entities.ContainsKey(key))
            {
                return 1;
            }

            entities.Add(key, entity);

            return 0;
        }

        // Method for creating an entity with specified type
        public Entity CreateEntity(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Pacman:
                    return new Pacman(new Point(0, 0), 0, 0, Direction.None, 0, 0);
                    case EntityType.Ghost:
                        return new Ghost(GhostColor.None, new Point(0, 0), 0, 0);
                    case EntityType.Wall:
                        return new Wall(0, 0);
                    case EntityType.Dot:
                        return new Dot(0, 0);
                    case EntityType.Border:
                        return new Border(0, 0);
                case EntityType.Energy:
                    return new Energy(0, 0);
                default:
                    return null;
            }
            return null;
        }

        public Entity GetEntity(EntityType entityType, string postfix)
        {
            string key = entityType.ToString() + postfix;
            if (entities.ContainsKey(entityType.ToString() + postfix))
            {
                return entities[entityType.ToString() + postfix];
            }
            return null;
        }

        public void NotifyGameStateChanged(GameState gameState)
        {
            foreach (KeyValuePair<string, Entity> entry in entities)
            {
                // TODO: Notify game state changed for each entity
                Logger.Log("Notify game state changed for :" + entry.Key);
            }
        }

        public void Reset()
        {
            foreach (KeyValuePair<string, Entity> entry in entities)
            {
                // TODO: Reset each entity
                Logger.Log("Reset entity: " + entry.Key);
            }
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, Entity> entry in entities)
            {
                Logger.Log("Draw entity: " + entry.Key);
                entry.Value.Draw();
            }
        }

        public void DynamicEntityStartMoving()
        {
            // Start moving for dynamic entities
            foreach (KeyValuePair<string, Entity> entry in entities)
            {
                if (entry.Value.entityType == EntityType.Pacman || entry.Value.entityType == EntityType.Ghost)
                {
                    ((DynamicEntity)entry.Value).Move();
                }
            }
        }
    }
}
