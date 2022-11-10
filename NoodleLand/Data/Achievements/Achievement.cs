using NoodleLand.Data.Databases;
using NoodleLand.Data.Entities;
using UnityEditor;
using UnityEngine;

namespace NoodleLand.Data.Achievements
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Achievements/New Achievement", fileName = "Achievement", order = 0)]
    public class Achievement : ScriptableObject,IData
    {
        
        [SerializeField] private string achvTag;
        [SerializeField] private string achievementName;
        [SerializeField]private Sprite image;
        [SerializeField] private Color iconMultiplayer;

        public string AchievementName => achievementName;
        public Sprite Image => image;
        public Color ColorMultiplier => iconMultiplayer;
        
        
        private void Awake()
        {
            RegisterSelf();
            if(iconMultiplayer.a == 0) iconMultiplayer = Color.white;
            
        }

        private void RegisterSelf()
        {
            var objs = AssetDatabase.LoadAllAssetsAtPath(AchievementDatabase.FolderPath);

            if (objs != null && objs.Length != 0)
            {
                foreach (var o in objs)
                {
                    if (o is AchievementDatabase achievementDatabase)
                    {
                        if(! achievementDatabase.Has(this))
                            achievementDatabase.Register(this);
                        return;
                    }    
                   
                }
            }
        }


        public string GetTag()
        {
            return achvTag;
        }
    }
}