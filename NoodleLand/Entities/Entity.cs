using NoodleLand.Farming;
using NoodleLand.Serialization.BDS;
using UnityEngine;

namespace NoodleLand.Entities
{
    public class Entity : MonoBehaviour
    { 
        [SerializeField] private string entityTag;
        [SerializeField] private bool forceRegister;
        public World World { get; private set; }


        public void RemoveForceRegister()
        {
            forceRegister = false;
        }
        protected virtual void Awake()
        {
            World = FindObjectOfType<World>();
         
        }

        protected virtual void Start()
        {
            if (forceRegister)
            {
                World.AddEntity(this, transform.position);
            }
            
            
        }

        public string EntityTag => entityTag;
        public  void SaveEntity(LDSDictionary ldsDictionary)
        {
            ldsDictionary.SetVector3("pos",transform.position);
            ldsDictionary.SetString("id",entityTag);
            CustomSaveEntity(ldsDictionary);

        }

        protected virtual void CustomSaveEntity(LDSDictionary ldsDictionary)
        {
            
        }
        protected virtual void CustomLoadEntity(LDSDictionary ldsDictionary)
        {
            
        }
        public  void LoadEntity(LDSDictionary ldsDictionary)
        {
            transform.position = ldsDictionary.GetVector3("pos");
            Debug.Log($"{entityTag} __ World"); 
            World.AddEntity(this, transform.position);
            CustomLoadEntity(ldsDictionary);
        }
    }
}