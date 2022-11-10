using NoodleLand.Data.Achievements;
using UnityEngine;

namespace NoodleLand.Data.Databases
{
    
    [CreateAssetMenu(menuName = "NoodleLand/Data/Databases/New Achievement Database", fileName = "AchievemnetDatabase")]

    public class AchievementDatabase : Database<Achievement>
    {
        
        public const string FolderPath = "Assets/Data/Databases/AchievementDatabase.asset";

    }
}