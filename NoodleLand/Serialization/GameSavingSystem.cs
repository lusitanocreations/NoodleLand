using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoodleLand.Serialization
{

    public interface IOnGameSaveAndLoad
    {
        public void OnLoadGame();
        public void OnSaveGame();

    }
    public class GameSavingSystem : MonoBehaviour
    {
        private List<IOnGameSaveAndLoad> _onGameSaveAndLoads = new List<IOnGameSaveAndLoad>();
        private static event Action<IOnGameSaveAndLoad> onGameSaveAndLoadEvent;

        private void Awake()
        {

            onGameSaveAndLoadEvent += InternalRegister;
        }

        public void LoadGame()
        {
            foreach (var onGameSaveAndLoad in _onGameSaveAndLoads)
            {
                onGameSaveAndLoad.OnLoadGame();
                
            }
        }

        public void SaveGame()
        {
            foreach (var onGameSaveAndLoad in _onGameSaveAndLoads)
            {
                onGameSaveAndLoad.OnSaveGame();
                
            }
        }
        private void OnDestroy()
        {
            onGameSaveAndLoadEvent -= InternalRegister;
        }

        private void InternalRegister(IOnGameSaveAndLoad onGameSaveAndLoad)
        {
            _onGameSaveAndLoads.Add(onGameSaveAndLoad);
        }
        public static void Register(IOnGameSaveAndLoad obj)
        {

            onGameSaveAndLoadEvent?.Invoke(obj);
            
        }

    }
}