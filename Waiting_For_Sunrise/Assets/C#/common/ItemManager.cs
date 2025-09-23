
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;


namespace Assets.C_.common
{
    public class ItemManager : AbstractJsonReadable<List<ItemDo>, List<Item>>, IResourceManager<FileResource, Item>
    {
        // 下标必须和id一样！！！
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
                throw new ResourceNotFoundException("无法通过id查找到该物品，id：" + id);
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