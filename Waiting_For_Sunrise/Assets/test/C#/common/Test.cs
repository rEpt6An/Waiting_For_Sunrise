using Assets.C_.common;
using Assets.C_.shop;

using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RegisterCenter.RegisterAll();

        IShop shop = Shop.GetInstance();
        GoodsDto goodsDto = shop.GetGoodsForSale();
        foreach (int id in goodsDto.GoodIds)
        {
            Debug.Log(id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
