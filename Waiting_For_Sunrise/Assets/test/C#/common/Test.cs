using Assets.C_.common;
using Assets.C_.shop;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RegisterCenter.RegisterAll();
        string o = "";

        IShop shop = Shop.GetInstance();
        GoodsDto goodsDto = shop.GetGoodsForSale();
        foreach (int id in goodsDto.GoodIds)
        {
            o = o + "id:" + id + ",";
        }
        Debug.Log(o);

        o = "";

        List<int> ignore = new List<int>();
        ignore.Add(1);
        shop.Flush(new GoodsGetConfig(99, 22, ignore));

        GoodsDto goodsDto2 = shop.GetGoodsForSale();


        foreach (int id in goodsDto2.GoodIds)
        {
            {
                o = o + "id:" + id + ",";
            }

        }
        Debug.Log(o);
    }
        // Update is called once per frame
        void Update()
        {

        }
}
