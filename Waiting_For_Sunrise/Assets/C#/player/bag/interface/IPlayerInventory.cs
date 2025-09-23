using Assets.C_.common.common;
using System.Collections.Generic;

namespace Assets.C_.player.bag
{
    public interface IPlayerInventory
    {
        // �õ����Ǹ��������ⲻС���޸�
        List<PileOfItem> GetAll();
        int Count(int itemId);
        void Add(PileOfItem item);
        void Delete(PileOfItem item);
    }
}