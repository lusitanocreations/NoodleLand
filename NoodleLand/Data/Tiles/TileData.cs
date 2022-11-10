using UnityEngine;
using UnityEngine.Tilemaps;

namespace NoodleLand.Data.Tiles
{
    [CreateAssetMenu(menuName = "NoodleLand/Data/Tiles/Basic Tile", fileName = "Tile", order = 0)]
    public class TileData : ScriptableObject
    {
      [SerializeField] private TileBase tileBase;
      [SerializeField] private string tileName;


      
      public bool canBeMadeIntoFarmPlot;

      public string TileName => tileName;
      public TileBase TileBase => tileBase;
    }
}