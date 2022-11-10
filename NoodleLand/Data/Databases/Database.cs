using System;
using System.Collections.Generic;
using NoodleLand.Data.Achievements;
using NoodleLand.Data.Drops;
using NoodleLand.Data.Entities;
using NoodleLand.Data.Items;
using NoodleLand.Data.Items.Fuel;
using UnityEditor;
using UnityEngine;

namespace NoodleLand.Data.Databases
{

 
   public class Database<T> : ScriptableObject
   {
      public List<T> values = new List<T>();
      
      public void Register(T t)
      {
         if(values.Contains(t)) return;
         Debug.Log($"{t} was sucessfully added to database of {typeof(T)}");
         values.Add(t);
      }

      public bool Has(T t)
      {
         return values.Contains(t);

      }
      

      public void Unregister(T t)
      {
         values.Remove(t);
      }
    
   }
   
   class DatabasePostDeleteProessor : AssetModificationProcessor
   {
      private static void Process<T>(string deletedAssetPath,string databasePatb) where T: UnityEngine.Object 
      {
         if (AssetDatabase.GetMainAssetTypeAtPath(deletedAssetPath) == typeof(T))
         {
            T a = AssetDatabase.LoadAssetAtPath<T>(deletedAssetPath);
            if (a != null)
            {
               var objs = AssetDatabase.LoadAllAssetsAtPath(databasePatb);
               foreach (var o in objs)
               {
                  if (o is Database<T> database)
                  {
                     Debug.Log("Sucessfully Unregistered from Database");
                     database.Unregister(a);
                  }
                        
               }
            }

         }
      }
       
      public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
      {
         Process<DropData>(path,DropDatabase.FolderPath);
         Process<EntityData>(path,EntityDatabase.FolderPath);
         Process<BaseItemData>(path,ItemDatabase.FolderPath);
         Process<Achievement>(path,AchievementDatabase.FolderPath);
         
         // if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(DropData))
         // {
         //    DropData a = AssetDatabase.LoadAssetAtPath<DropData>(path);
         //    if (a != null)
         //    {
         //       var objs = AssetDatabase.LoadAllAssetsAtPath(DropDatabase.FolderPath);
         //       foreach (var o in objs)
         //       {
         //          if (o is DropDatabase dropDatabase)
         //          {
         //             dropDatabase.Unregister(a);
         //          }
         //                
         //       }
         //    }
         //
         // }
         return AssetDeleteResult.DidNotDelete;
      }
      
   }
}
