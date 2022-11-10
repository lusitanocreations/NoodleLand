using NoodleLand.Events;

namespace NoodleLand.Interfaces
{
    public interface IMarkerOn
    {
        public void OnMarkerEnter();
        public void OnMarkerLeave();
    }

    public interface IWorldInteractable
    {
        public void OnInteract(OnInteractEnterEvent onInteractEnterEvent);
        
        public void OnLeave(OnInteractLeaveEvent onInteractLeaveEvent);


    }
    
    
}