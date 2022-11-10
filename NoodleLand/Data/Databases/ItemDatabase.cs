using NoodleLand.Data.Items;
using UnityEngine;

namespace NoodleLand.Data.Databases
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Databases/New Item Database", fileName = "New ItemDatabase")]
    public class ItemDatabase : Database<BaseItemData>
    {
        public const string FolderPath = "Assets/Data/Databases/ItemDatabase.asset";

    }
}