using NoodleLand.Data.Achievements;
using NoodleLand.Data.Databases;

namespace NoodleLand.Registeries
{
    public class RegisteredAchievements : RegisteredDatabase<AchievementDatabase,Achievement>
    {
        public static RegisteredAchievements Instance { get; private set; }


        public Achievement TheStart => Get("TheStart");

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
    }
}