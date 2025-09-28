namespace Assets.C_.shop
{
    public class BuyRequest
    {
        public int IndexOfItemInShop { get; private set; }

        public BuyRequest(int indexOfItemInShop)
        {
            IndexOfItemInShop = indexOfItemInShop;
        }
    }
}