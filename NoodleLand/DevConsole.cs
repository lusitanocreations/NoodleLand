using System;
using System.Collections;
using System.Collections.Generic;
using NoodleLand.Farming;
using NoodleLand.Serialization;
using UnityEngine;



public class DevConsole : MonoBehaviour
{
    public string commandName;

    private Dictionary<string, Action> commands = new Dictionary<string, Action>();

    public bool press;

    private void OnValidate()
    {
        if (press)
        {
            press = false;
            if (commands.TryGetValue(commandName, out Action action))
            {
                action?.Invoke();
            }

        }
    }

    private void Awake()
    {
        commands["savegame"] = () =>
        {
            FindObjectOfType<GameSavingSystem>().SaveGame();
        };
        commands["loadgame"] = () =>
        {
            FindObjectOfType<GameSavingSystem>().LoadGame();
        };
    }
    
}
