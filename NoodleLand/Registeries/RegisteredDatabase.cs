using System;
using System.Collections.Generic;
using NoodleLand.Data.Databases;
using NoodleLand.Data.Entities;
using UnityEngine;

namespace NoodleLand.Registeries
{
    public abstract class RegisteredDatabase<T,K>: MonoBehaviour where T : Database<K> where  K: IData
    {
        [SerializeField] private T database;
        private Dictionary<string, K> _tagToDrops = new Dictionary<string,K>();

        public int Count => database.values.Count;


        public T Database => database;
        
        protected virtual void Awake()
        {
            InitializeDatabase();
            
        }

        private void InitializeDatabase()
        {
            for (var i = 0; i < database.values.Count; i++)
            {
                K data = database.values[i];

                Debug.Log($"{data.GetTag()} has been registered into game");
                _tagToDrops[data.GetTag()] = data;

            }
        }

        public K Get(int index)
        {
            return database.values[index];
        }

        public K Get(string tag)
        {
            if(_tagToDrops.TryGetValue(tag,out K data))
            {
                return data;
            }

            return default;
        }
    }
}