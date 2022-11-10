using NoodleLand.Data.Entities;
using UnityEngine;

namespace NoodleLand.Data.Databases
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Databases/New Entity Database", fileName = "EntityDatabase")]
    public class EntityDatabase : Database<EntityData>
    {
        public const string FolderPath = "Assets/Data/Databases/EntityDatabase.asset";

      
    }
}