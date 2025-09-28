using Assets.C_.common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.C_.shop
{
    public class ItemIdLottery : IItemIdLottery
    {
        public int GetItemId(int luck, int day)
        {
            List<Item> allItem = ItemManager.Instance.GetAll();
            List<int> powerInt = new List<int>();
            foreach (Item item in allItem)
            {
                powerInt.AddRange(Enumerable.Repeat(item.Id, GetPower(item.Rarity, luck, day)));
            }
            return powerInt.IndexOf(GetRandomIndex(allItem.Count));
        }

        private int GetPower(Rarity rarity, int luck, int day) {
            int power = (luck*1 + day*1 + 1) * ((int)rarity + 1)*1;
            return power;
        }

        private int GetRandomIndex(int length)
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, length);
            return randomNumber;
        }
    }
}