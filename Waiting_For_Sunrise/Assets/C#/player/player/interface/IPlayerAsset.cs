using Assets.C_.common.common;
using Assets.C_.player.bag;

namespace Assets.C_.player.player
{
    public interface IPlayerAsset
    {
        Re ChangeMoney(int amount);

        int GetMoney();

        IPlayerBag GetPlayerBag();
    }
}