using Assets.C_.common;
using System.Collections.Generic;

namespace Assets.C_.shop
{
    public record GoodsDto
    {
        public List<int> GoodIds { get; private set; }

        public GoodsDto(List<int> itemIds) { 
            GoodIds = itemIds;
        }
    }
}