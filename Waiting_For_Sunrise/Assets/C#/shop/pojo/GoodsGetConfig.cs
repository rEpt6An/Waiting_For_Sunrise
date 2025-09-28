using System.Collections.Generic;

namespace Assets.C_.shop
{
    public class GoodsGetConfig
    {
        public int Luck {  get; private set; }
        public int Day { get; private set; }
        public List<int> LockedGoodIndexs { get; private set; }

        public GoodsGetConfig(int luck, int day, List<int> lockedGoodIndexs)
        {
            this.Luck = luck;
            this.Day = day;
            this.LockedGoodIndexs = lockedGoodIndexs;
        }

        public GoodsGetConfig(int luck, int day)
        {
            this.Luck = luck;
            this.Day = day;
            this.LockedGoodIndexs = new List<int>();
        }
    }
}