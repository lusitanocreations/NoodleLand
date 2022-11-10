using NoodleLand.Data.Databases;
using NoodleLand.Entities;
using UnityEditor;
using UnityEngine;

namespace NoodleLand.Data.Entities
{
    public interface IData
    {
        public string GetTag();
    }
    [CreateAssetMenu(menuName = "NoodleLand/Data/Entities/Entities/New Entity Data", fileName = "New EntityData", order = 0)]
    public class EntityData : ScriptableObject, IData
    {
        [SerializeField] public Entity _entity;
        [SerializeField] private string entityTag;


        public string EntityTag => entityTag;
        public Entity Entity => _entity;

        private void Awake()
        {
            RegisterSelf();
        }

        private void RegisterSelf()
        {
            var objs = AssetDatabase.LoadAllAssetsAtPath(EntityDatabase.FolderPath);

            if (objs != null && objs.Length != 0)
            {
                foreach (var o in objs)
                {
                    if (o is EntityDatabase entityDatabase)
                    {
                        if(! entityDatabase.Has(this))
                            entityDatabase.Register(this);
                        return;
                    }    
                   
                }
            }
        }

        public string GetTag()
        {
            return entityTag;
        }
    }
}