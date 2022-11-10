using NoodleLand.Data.Drops;
using UnityEngine;

namespace NoodleLand.Data.Databases
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Databases/new DropDatabase", fileName = "DropDatabase", order = 0)]
    public class DropDatabase : Database<DropData>
    {
        public const string FolderPath = "Assets/Data/Databases/DropDatabase.asset";

    }
}