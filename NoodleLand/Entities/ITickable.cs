using NoodleLand.Entities.Living.Player;
using NoodleLand.Farming;

namespace NoodleLand.Entities
{
    public interface ITickable
    {
        public void OnTick(World world);
    }
}