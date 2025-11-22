using Assets.C_.common.common;
using System.Collections.Generic;

namespace Assets.C_.player.bag
{
    public interface IPlayerInventory
    {
        /// <summary>
        /// 获取玩家背包中所有物品堆的副本列表
        /// </summary>
        /// <remarks>
        /// 返回的是物品堆的副本，以避免外部代码意外修改原始数据
        /// </remarks>
        /// <returns>物品堆的副本列表</returns>
        List<PileOfItem> GetAll();

        /// <summary>
        /// 统计背包中指定物品ID的总数量
        /// </summary>
        /// <param name="itemId">要统计的物品ID</param>
        /// <returns>该物品在背包中的总数量</returns>
        int Count(int itemId);

        /// <summary>
        /// 向背包中添加一个物品堆
        /// </summary>
        /// <param name="item">要添加的物品堆</param>
        void Add(PileOfItem item);

        /// <summary>
        /// 从背包中删除一个物品堆
        /// </summary>
        /// <param name="item">要删除的物品堆</param>
        void Delete(PileOfItem item);
    }
}