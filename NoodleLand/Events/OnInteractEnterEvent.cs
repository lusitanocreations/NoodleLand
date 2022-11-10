using Lusitano.Input;
using NoodleLand.Entities.Living.Player;

namespace NoodleLand.Events
{
    public class OnInteractEnterEvent
    {
        public PlayerEntity player { get; }
        public AnalogType analogUsed;

        public OnInteractEnterEvent(PlayerEntity playerEntity,AnalogType analogTypeUsed)
        {
            analogUsed = analogTypeUsed;
            
            this.player = playerEntity;

        }
        
    }
    
    public class OnInteractLeaveEvent
    {
        public PlayerEntity player { get; }
       
        public OnInteractLeaveEvent(PlayerEntity playerEntity)
        {
           
            this.player = playerEntity;

        }
        
    }
}