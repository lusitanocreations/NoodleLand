using System.Collections;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using NoodleLand.Serialization.BDS;
using NoodleLand.UI.SaveAndLoad;
using UnityEngine;

public class LoadGameUIController : MonoBehaviour
{
    [SerializeField] private Transform content;

    [SerializeField] private LoadOptionUI model;


    private List<LoadOptionUI> _loadOptions = new List<LoadOptionUI>();

    public void Refresh()
    {
        Clear();
        string js = PlayerPrefs.GetString("SavedGames");
        LDSDictionary ldsDictionary = JsonConvert.DeserializeObject<LDSDictionary>(js);
        if(ldsDictionary == null) return;
        
        List<string> names = ldsDictionary.GetStringList("names");
        List<string> times = ldsDictionary.GetStringList("times");
        
        for (var i = 0; i < names.Count; i++)
        {
            AddLoadOption(names[i],times[i]);
            
        }
    }

    private void AddLoadOption(string text, string time)
    {
        LoadOptionUI optionUI = Instantiate(model);
        optionUI.Set(text,time);
        _loadOptions.Add(optionUI);
    }

    private void Clear()
    {
        for (var i = 0; i < _loadOptions.Count; i++)
        {
            Destroy(_loadOptions[i].gameObject);
            
        }
    }
    public void Add(string loadName, string time)
    {
        
    }
}
