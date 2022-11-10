using TMPro;
using UnityEngine;

namespace NoodleLand.UI.SaveAndLoad
{
    public class LoadOptionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI saveName;
        [SerializeField] private TextMeshProUGUI timeSaved;


        public void Set(string saveName, string timeSaved)
        {
            this.saveName.text = saveName;
            this.timeSaved.text = timeSaved;
        }
    }
}