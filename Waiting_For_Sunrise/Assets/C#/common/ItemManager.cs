using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.C_.common
{
    public class ItemManager : IItemManager
    {
        private static readonly List<Item> ITEMS = new();

        public void Init(List<Item> items)
        {
            ITEMS.AddRange(items);
        }

        public Item GetItem(int id)
        {
            Item item = ITEMS[id];
            if (item == null)
            {
                throw new ItemException("�޷�ͨ��id���ҵ�����Ʒ��id��" + id);
            }
            return item;
        }
    }
}