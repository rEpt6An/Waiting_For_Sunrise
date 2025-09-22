using System;

namespace Assets.C_.common
{
    public enum Rarity: int
    {
        Write,
        Green,
        Blue,
        Purple,
        Red
    }


    public static class RarityExtensions
    {
        public static Rarity GetById(int id)
        {
            if (Enum.IsDefined(typeof(Rarity), id))
                return (Rarity)id;
            throw new ArgumentOutOfRangeException(nameof(id), $"Invalid Rarity ID: {id}");
        }

        public static Rarity GetByName(string name)
        {
            if (Enum.TryParse(name, ignoreCase: true, out Rarity rarity))
                return rarity;
            throw new ArgumentException($"Invalid Rarity name: {name}", nameof(name));
        }
    }
}