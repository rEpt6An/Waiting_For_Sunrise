using Assets.C_.common.common;
using System.Collections.Generic;

namespace Assets.C_.player.bag
{
    public interface IPlayerBag
    {
        IPlayerToolbar GetToolbar();
        IPlayerInventory GetInventory();
        void MoveToToolbar(PileOfItem pileOfItem, int targetToolbarIndex);
        void MoveToInventory(int index, int num);
        void GiveItem(PileOfItem pileOfItem);
        void DeleteItem(PileOfItem pileOfItem);
    }
}