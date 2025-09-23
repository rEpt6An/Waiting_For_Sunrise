using Assets.C_.common.common;
using Assets.C_.player.bag;

namespace Assets.C_.player.player
{
    public class PlayerAsset: IPlayerAsset
    {
        public int Money {  get; private set; }
        public IPlayerBag PlayerBag {  get; private set; }

        public PlayerAsset()
        {
            Money = 0;
            PlayerBag = new PlayerBag();
        }

        public Re ChangeMoney(int amount)
        {
            int m = Money;
            m += amount;
            if (m < 0)
            {
                return Re.Fail("Ǯ��������0");
            }
            Money = m;
            return Re.Ok();
        }

        public IPlayerBag GetPlayerBag()
        {
            return PlayerBag;
        }
    }
}