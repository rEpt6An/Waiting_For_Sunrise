using Assets.C_.common.common;
using System.Collections.Generic;

namespace Assets.C_.player.bag
{
    public interface IPlayerInventory
    {
        // 拿到的是副本，避免不小心修改
        List<PileOfItem> GetAll();
        int Count(int itemId);
        void Add(PileOfItem item);
        void Delete(PileOfItem item);
    }
}