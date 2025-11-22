using System;

namespace Assets.C_.common
{
    public enum WeaponType : int
    {
        melee,
        ranged
    }

    public static class WeaponTypeExtensions
    {
        public static WeaponType GetById(int id)
        {
            if (Enum.IsDefined(typeof(WeaponType), id))
                return (WeaponType)id;
            throw new ArgumentOutOfRangeException(nameof(id), $"Invalid WeaponType ID: {id}");
        }

        public static WeaponType GetByName(string name)
        {
            if (Enum.TryParse(name, ignoreCase: true, out WeaponType itemType))
                return itemType;
            throw new ArgumentException($"Invalid WeaponType name: {name}", nameof(name));
        }
    }
}