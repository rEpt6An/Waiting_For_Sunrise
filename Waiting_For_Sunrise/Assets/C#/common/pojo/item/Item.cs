using System.Collections.Generic;


namespace Assets.C_.common
{
    public class Item
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Rarity Rarity { get; private set; }
        public ItemType ItemType { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        public Icon Icon { get; set; } = null;

        public Item(int id, string name, Rarity rarity, ItemType itemType, string description, int price, Icon icon)
        {
            Id = id;
            Name = name;
            Rarity = rarity;
            ItemType = itemType;
            Description = description;
            Price = price;
            Icon = icon;
        }
    }
}

