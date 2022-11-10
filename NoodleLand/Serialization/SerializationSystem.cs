using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoodleLand.Serialization
{
    [Obsolete]
    public interface ISerializedObject
    {
        public void OnSave();
        
        public void OnLoad();
    }
    [Obsolete]
    public interface ISerializedSector
    {
        public List<ISerializedObject> ObjectsSeriailized { get; }
    }
    [Obsolete]
    public  class SerializationSystem : MonoBehaviour
    {

        private static List<ISerializedSector> _sectors = new List<ISerializedSector>();

        public static void RegisterSector(ISerializedSector sector)
        {
            _sectors.Add(sector);
            
        }
        [Obsolete]

        public static void SaveGame()
        {
            foreach (var serializedSector in _sectors)
            {
                foreach (var serializedObject in serializedSector.ObjectsSeriailized)
                {
                    serializedObject.OnSave();
                    
                }
            }
        }
        [Obsolete]
        
        public static void LoadGame()
        {
            foreach (var serializedSector in _sectors)
            {
                foreach (var serializedObject in serializedSector.ObjectsSeriailized)
                {
                    serializedObject.OnLoad();
                    
                }
            }
        }

    }
}