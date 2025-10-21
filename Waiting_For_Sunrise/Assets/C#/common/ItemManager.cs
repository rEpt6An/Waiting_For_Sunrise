
using System.Collections.Generic;

namespace Assets.C_.common
{
    public class ItemManager : AbstractJsonReadable<List<ItemDo>, List<Item>>, IResourceManager<FileResource, Item>
    {
        // �±�����idһ��������
        private static readonly List<Item> ITEMS = new();
        private static readonly object _lock = new();

        public static ItemManager Instance { get; private set; }

        private ItemManager()
        {

        }

        public static ItemManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new ItemManager();
            }
            return Instance;
        }
        
        public void Load(FileResource fileResource)
        {
            lock (_lock)
            {
                ITEMS.Clear();
                ITEMS.AddRange(Instance.Read(fileResource));
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

        public List<Item> GetAll()
        {
            List<Item> copy = new List<Item>();
            copy.AddRange(ITEMS);
            return copy;
        }
    }
}