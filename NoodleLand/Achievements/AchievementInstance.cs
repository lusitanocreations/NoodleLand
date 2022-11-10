using NoodleLand.Data.Achievements;
using NoodleLand.Serialization.BDS;
using UnityEngine;

namespace NoodleLand.Achievements
{
    [System.Serializable]
    public sealed class AchievementInstance
    {
        public Achievement Achievement;

        public bool HasCompleted { get; set; }

        public void OnSave(LDSDictionary data)
        {
            data.Set("hasCompleted",HasCompleted);

            
        }

        public void OnLoad(LDSDictionary data)
        {
            var a= data.Get<bool>("hasCompleted");
            HasCompleted = a.Value;
        }
        public void CompleteAchievement()
        {
            HasCompleted = true;
            Debug.Log($"{Achievement.name} has been completed!");
        }
        
        public AchievementInstance(Achievement achievement)
        {
            this.Achievement = achievement;
        }
    }
}