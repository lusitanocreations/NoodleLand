using System;
using NoodleLand.Data.Databases;
using NoodleLand.Data.Items;

namespace NoodleLand.Registeries
{
    public class RegisteredItems : RegisteredDatabase<ItemDatabase, BaseItemData>
    {
        
        //Bad Singleton fix after
        public static RegisteredItems Instance { get; private set; }

        protected virtual void Awake()
        {
            base.Awake();
            Instance = this;
        }


        // Tags
        public BaseItemData Flint => Get("Flint");
        public BaseItemData Wood => Get("Wood");
        public BaseItemData Rock => Get("Rock");
    }
}