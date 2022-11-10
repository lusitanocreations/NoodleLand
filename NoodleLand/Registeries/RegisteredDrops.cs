using System;
using System.Collections;
using NoodleLand.Data.Databases;
using NoodleLand.Data.Drops;
using UnityEngine.Serialization;

namespace NoodleLand.Registeries
{
   public class RegisteredDrops : RegisteredDatabase<DropDatabase,DropData>
   {
      
           
      //Bad Singleton fix after
      public static RegisteredDrops Instance { get; private set; }

      protected override void Awake()
      {
         base.Awake();
         Instance = this;
      }

   }
}