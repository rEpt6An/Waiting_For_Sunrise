using System.Collections.Generic;

namespace Assets.C_.shop
{
    public class GoodsGetConfig
    {
        public int Luck {  get; set; }
        public int Day { get; set; }
        public List<int> LockedGoodIndexs { get; set; }

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