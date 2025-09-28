
using Assets.C_.common;
using Assets.C_.common.common;
using Assets.C_.player.player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.C_.shop
{
    public class Shop : IShop
    {
        private static readonly int ITEM_COUNT_IN_SHOP = 4;
        private static readonly List<int> ItemIdForSale = new List<int>();

        public static Shop Instance { get; private set; } = null;

        public static Shop GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Shop();
            }
            return Instance;
        }

        private Shop()
        {
            ItemIdForSale.AddRange(Enumerable.Repeat(-1, ITEM_COUNT_IN_SHOP));
            Flush(new GoodsGetConfig(0, 0));
        }

        public void Flush(GoodsGetConfig goodsGetConfig) {
            IItemIdLottery itemIdLottery = new ItemIdLottery();
            for (int i = 0; i < ItemIdForSale.Count; i++)
            {
                if (goodsGetConfig.LockedGoodIndexs.Contains(i))
                {
                    continue;
                }
                int itemId = itemIdLottery.GetItemId(goodsGetConfig.Luck, goodsGetConfig.Day);
                ItemIdForSale[i] = itemId;
            }
        }

        public GoodsDto GetGoodsForSale()
        {
            return new GoodsDto(ItemIdForSale);
        }
        public Re Buy(IPlayerAsset playerAsset, BuyRequest buyRequest)
        {
            if (buyRequest.IndexOfItemInShop >= ITEM_COUNT_IN_SHOP) {
                throw new ArgumentException("所购买物品所在栏位超出商店大小");
            }

            int itemId = ItemIdForSale[buyRequest.IndexOfItemInShop];
            Item item = ItemManager.Instance.Get(itemId);

            Re moneyRe = playerAsset.ChangeMoney(item.Price * -1);
            if (!moneyRe.IsSuccess()) {
                return moneyRe;
            }
            playerAsset.GetPlayerBag().GiveItem(new player.bag.PileOfItem(itemId, 1));
            return Re.Ok();
        }
    }
}