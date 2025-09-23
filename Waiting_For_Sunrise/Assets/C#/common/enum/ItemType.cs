using System;

namespace Assets.C_.common
{
    public enum ItemType: int
    {
        equipment,
        weapon,
        consumable
    }

    public static class ItemTypeExtensions
    {
        public static ItemType GetById(int id)
        {
            if (Enum.IsDefined(typeof(ItemType), id))
                return (ItemType)id;
            throw new ArgumentOutOfRangeException(nameof(id), $"Invalid ItemType ID: {id}");
        }

        public static ItemType GetByName(string name)
        {
            if (Enum.TryParse(name, ignoreCase: true, out ItemType itemType))
                return itemType;
            throw new ArgumentException($"Invalid ItemType name: {name}", nameof(name));
        }
    }
}