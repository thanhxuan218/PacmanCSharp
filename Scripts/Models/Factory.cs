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
        public int AddEntity(Entity entity)
        {
            return 0;
        }

        // Method for creating an entity with specified type
        public Entity CreateEntity(EntityType entityType)
        {
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
    }
}
