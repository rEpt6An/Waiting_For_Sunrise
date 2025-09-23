using Assets.C_.common.common;
using System.Collections.Generic;

namespace Assets.C_.player.bag
{
    public class PlayerInventory: IPlayerInventory
    {
        public List<PileOfItem> Inventory { get; private set; } = new();
        private readonly object _lock = new object();

        public void Add(PileOfItem item)
        {
            lock (_lock)
            {
                int index = FindItem(item.ItemId);
                if (index == -1)
                {
                    Inventory.Add(item);
                }
                else
                {
                    Inventory[index].Count += item.Count;
                }
            }
        }

        public void Delete(PileOfItem item)
        {
            lock (_lock)
            {
                int index = FindItem(item.ItemId);
                if (index == -1)
                {
                    throw new BagException("无法删除背包内不存在的物品，id："+ item.ItemId);
                }
                else
                {
                    int count = Inventory[index].Count;
                    count -= item.Count;
                    if (count < 0)
                    {
                        throw new BagException("删除时物品数量不足，id：" + item.ItemId);
                    } else
                    {
                        Inventory[index].Count = count;
                    }
                }
            }
        }

        public List<PileOfItem> GetAll()
        {
            lock (_lock)
            {
                List<PileOfItem> copy = new List<PileOfItem>();
                foreach (PileOfItem item in Inventory)
                {
                    copy.Add(item);
                }
                return copy;
            }
        }

        private int FindItem(int itemId)
        {
            int index = -1;
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (itemId == Inventory[i].ItemId)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public int Count(int itemId)
        {
            int index = FindItem(itemId);
            if (index == -1) return 0;
            return Inventory[index].Count;
        }
    }
}