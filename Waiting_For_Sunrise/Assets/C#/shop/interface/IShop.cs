using Assets.C_.common.common;
using Assets.C_.player.player;

namespace Assets.C_.shop
{
    public interface IShop
    {
        GoodsDto GetGoodsForSale();

        void Flush(GoodsGetConfig goodsGetConfig);

        Re Buy(IPlayerAsset playerAsset, BuyRequest buyRequest);
    }
}