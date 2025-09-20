using System.Collections.Generic;


namespace Assets.C_.common
{
    public class Item: AbstractJsonReadable<List<ItemDo>, List<Item>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Rarity Rarity { get; set; }
        public string Description { get; set; }
        public Icon Icon { get; set; } = null;

        protected override List<Item> Convert(List<ItemDo> jsonDos)
        {
            List<Item> list = new();
            foreach (ItemDo jsonDo in jsonDos) {
                list.Add(ItemConverter.Do2E(jsonDo));
            }
            return list;
        }
    }
}

