using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using NoodleLand.Data.Entities;
using NoodleLand.Entities;
using NoodleLand.Entities.GridEntities;
using NoodleLand.MessageHandling.World;
using NoodleLand.Registeries;
using NoodleLand.Serialization;
using NoodleLand.Serialization.BDS;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NoodleLand.Farming
{
    public interface IGridObject
    {
        public GameObject Get();
        public Vector2 Offset();
        public void OnSpawn();
    }

    
    public class World : MonoBehaviour, IOnGameSaveAndLoad
    {
        [Header("World Properties")]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Vector2Int GridSize;
        [SerializeField] private Vector2Int offset;
        [SerializeField]public Vector2Int chunks;
        private IGridObject[,] _objects;
        private List<Entity> allEntities = new List<Entity>();
        private List<ITickable> tickables = new List<ITickable>();
        private GridEntity _entity;
        
        private void OnDrawGizmos()
        {
            for (int chunkX = 0; chunkX < chunks.x; chunkX++)
            {

                for (int chunkY = 0; chunkY < chunks.y; chunkY++)
                {
                    for (int i = 0; i < GridSize.x; i++)
                    {
                        for (int j = 0; j < GridSize.y; j++)
                        {
                            Gizmos.DrawWireCube(new Vector2(i ,j ) + offset, new Vector3(1,1,0));
                        }
                    }
                }
            }
        
        }

    


        private void CleanGame()
        {
            for (var x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    if (_objects[x, y] != null)
                    {
                        RemoveEntity(_objects[x, y].Get().GetComponent<Entity>());
                    }
                }
                
            }
        }

        IEnumerator Tick()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.05f);

                for (var i = 0; i < tickables.Count; i++)
                {
                    tickables[i].OnTick(this);
                    
                }
            }
            
        }

        public void RemoveTickableObject(ITickable tickable)
        {
            tickables.Remove(tickable);

        }
        
        IEnumerator RandomSpawn()
        {
                  bool hasSelected = false;
                    while (true)
                    {
                        while (!hasSelected)
                        {
                            Debug.Log("no selected");
                            int rnX = UnityEngine.Random.Range(0, GridSize.x);
                            int rnY = UnityEngine.Random.Range(0, GridSize.y);

                            Vector2Int p0 = new Vector2Int(rnX, rnY) + offset;

                            TileBase t0 = tilemap.GetTile((Vector3Int) p0);
                            
                            if (t0 != null && _objects[rnX, rnY] == null)
                            {
                                hasSelected = true;
                                
                                AddEntity(Instantiate(_entity) , p0);

                            }

                            yield return null;

                        }
                        yield return new WaitForSeconds(5f);
                        hasSelected = false;

                    }
              
        }

        public bool CanPlaceAt(Vector2 worldPoint)
        {
            Vector2 fixOffset = worldPoint - offset;
            Vector2Int roundedVector = new Vector2Int(Mathf.RoundToInt(fixOffset.x), Mathf.RoundToInt(fixOffset.y));
          
            if (_objects[roundedVector.x, roundedVector.y] == null)
            {
                return true;

            }

            return false;
        }



        public WorldPlaceMessage AddEntity<T>(T ent, Vector2 worldPoint) where T: Entity
        {
            if (ent is IGridObject o)
            {
                
            Vector2 fixOffset = worldPoint - offset;
            Vector2Int roundedVector = new Vector2Int(Mathf.RoundToInt(fixOffset.x), Mathf.RoundToInt(fixOffset.y));

            if (CanPlaceAt(worldPoint))
            {
                allEntities.Add(ent);
                _objects[roundedVector.x, roundedVector.y] = o;
                GameObject k = o.Get();
                Vector2 position = k.transform.position;
                position = new Vector2(Mathf.RoundToInt(worldPoint.x), Mathf.RoundToInt(worldPoint.y));
                position +=  o.Offset();
                k.transform.position = position;
                
                Debug.Log("Grid Entity Added");
                o.OnSpawn();

                ITickable tickable = o.Get().GetComponent<ITickable>();
                if (tickable != null)
                {
                    tickables.Add(tickable);
                    
                }
               
                return WorldPlaceMessage.PlacedSuccessfully;

            }
            }
            else
            {
                Debug.Log(" Entity Added");
                ent.transform.position = worldPoint;
                allEntities.Add(ent);
                return WorldPlaceMessage.PlacedSuccessfully;
            }
            return WorldPlaceMessage.SomethingHappened;
           
        }
        public WorldPlaceMessage RemoveEntity(Entity entity)
        {

            IGridObject gridObject = entity.GetComponent<IGridObject>();

            if (gridObject != null)
            {
                Vector2 fixOffset = (Vector2) gridObject.Get().transform.position - offset;
           
           
                Vector2Int roundedVector = new Vector2Int(Mathf.RoundToInt(fixOffset.x), Mathf.RoundToInt(fixOffset.y));

                _objects[roundedVector.x, roundedVector.y] = null;
            
                ITickable tickable = gridObject.Get().GetComponent<ITickable>();
                if (tickable != null)
                {
                    tickables.Remove(tickable);
                    
                }
                
                if (gridObject.Get().activeSelf)
                {
                    gridObject.Get().SetActive(false);
                    Destroy(gridObject.Get(),4f);
                   
                }

            }

            allEntities.Remove(entity);

            return WorldPlaceMessage.RemovedWell;
           
        }
        private void Awake()
        {
            Debug.Log("Grid On");
            _objects = new IGridObject[GridSize.x, GridSize.y];
            GameSavingSystem.Register(this);
          

        }

      
        private void Start()
        {
            StartCoroutine(Tick());
            StartCoroutine(Kappa());


        }


        private float timeTook;

        IEnumerator Kappa()
        {
            
            yield return new WaitForSeconds(20f);


            Debug.Log("20 SECONDS PASSED");
        }

        public void LoadGame()
        {
            
            CleanGame();
            
            string entJson = PlayerPrefs.GetString("entities");

            List<LDSDictionary> deserializeObject =  JsonConvert.DeserializeObject<List<LDSDictionary>>(entJson);
            
            for (var i = 0; i < deserializeObject.Count; i++)
            {
                string id = deserializeObject[i].GetString("id");
                var entityData = FindObjectOfType<RegisteredEntities>().Get(id);
                if (entityData == null)
                {
                    Debug.Log($"Missing Entity {id}");
                   continue;
                }
                var a0 = Instantiate(entityData._entity);
                a0.RemoveForceRegister();
                a0.LoadEntity(deserializeObject[i]);
                timeTook += 1;


            }
       

        }
        public void SaveGame()
        {
            List<LDSDictionary> binaryDataSaves = new List<LDSDictionary>();
            
            foreach (var allEntity in allEntities)
            {
                Debug.Log(allEntity.EntityTag);
                LDSDictionary b0 = new LDSDictionary();
                allEntity.SaveEntity(b0);
                binaryDataSaves.Add(b0);
                timeTook += Time.deltaTime;

            }
            Debug.Log("Saved Entities");
            string saved = JsonConvert.SerializeObject(binaryDataSaves);
            PlayerPrefs.DeleteKey("entities");
            PlayerPrefs.SetString("entities",saved);
           
        }

        public void OnLoadGame()
        {
           LoadGame();
        }

        public void OnSaveGame()
        {
          SaveGame();
        }
    }
}
