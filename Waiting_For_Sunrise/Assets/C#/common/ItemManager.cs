
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;


namespace Assets.C_.common
{
    public class ItemManager : AbstractJsonReadable<List<ItemDo>, List<Item>>, IResourceManager<FileResource, Item>
    {
        // �±�����idһ��������
        private static readonly List<Item> ITEMS = new();
        private static readonly object _lock = new();

        public void Load(FileResource fileResource)
        {
            lock (_lock)
            {
                ItemManager itemManager = new();
                ITEMS.Clear();
                ITEMS.AddRange(itemManager.Read(fileResource));
            }
        }

        public Item Get(int id)
        {
            Item item = ITEMS[id];
            if (item == null)
            {
                throw new ResourceNotFoundException("�޷�ͨ��id���ҵ�����Ʒ��id��" + id);
            }
            return item;
        }

        protected override List<Item> Convert(List<ItemDo> jsonDos)
        {
            List<Item> list = new();
            foreach (ItemDo jsonDo in jsonDos)
            {
                list.Add(ItemConverter.Do2E(jsonDo));
            }
            return list;
        }
    }
}