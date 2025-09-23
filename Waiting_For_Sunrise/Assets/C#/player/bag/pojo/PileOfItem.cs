namespace Assets.C_.player.bag
{
    public class PileOfItem
    {
        public int ItemId { get; set; }
        public int Count { get; set; }

        public PileOfItem(int itemId, int count) 
        { 
            ItemId = itemId;
            Count = count;
        }
    }
}