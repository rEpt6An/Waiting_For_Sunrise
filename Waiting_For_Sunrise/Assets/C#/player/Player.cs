using Assets.C_.bus;
using Assets.C_.player.player;
using Assets.C_.item;

namespace Assets.C_.player
{
    public class Player: AbstractAttackable
    {
        private static readonly Player Instance = new();

        public static Player GetInstance() { return Instance; }

        public IPlayerState PlayerState { get; private set; }

        public IPlayerAsset PlayerAsset { get; private set; }

        public Player()
        {
            PlayerState = new PlayerState();
            PlayerAsset = new PlayerAsset();
        }

        

        private static void PublishPlayerGetDamageEvent(Damage actualDamage, Damage orignDamage)
        {
            EventBus.Publish(new PlayerGetDamageEvent(actualDamage, orignDamage));
        }
    }
}