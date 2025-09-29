
using Assets.C_.player.player;

namespace Assets.C_.item
{
    public class Lamp : IEquipment
    {
        private static readonly int ID = 1;

        public int Id => ID;

        public void Execute(IPlayerState playState)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteRemove(IPlayerState playState)
        {
            throw new System.NotImplementedException();
        }
    }
}