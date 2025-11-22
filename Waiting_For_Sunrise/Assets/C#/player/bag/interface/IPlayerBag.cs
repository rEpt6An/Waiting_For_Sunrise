using Assets.C_.common.common;
using System.Collections.Generic;

namespace Assets.C_.player.bag
{
    public interface IPlayerBag
    {
        /// <summary>
        /// 获取玩家工具栏实例
        /// </summary>
        /// <returns>玩家工具栏接口实例</returns>
        IPlayerToolbar GetToolbar();

        /// <summary>
        /// 获取玩家背包实例
        /// </summary>
        /// <returns>玩家背包接口实例</returns>
        IPlayerInventory GetInventory();

        /// <summary>
        /// 将物品从背包移动到指定索引的工具栏位置
        /// </summary>
        /// <param name="pileOfItem">要移动的物品堆</param>
        /// <param name="targetToolbarIndex">目标工具栏索引位置</param>
        void MoveToToolbar(PileOfItem pileOfItem, int targetToolbarIndex);

        /// <summary>
        /// 将工具栏指定位置的物品移动到背包
        /// </summary>
        /// <param name="index">工具栏中的物品索引位置</param>
        /// <param name="num">要移动的物品数量</param>
        void MoveToInventory(int index, int num);

        /// <summary>
        /// 向玩家添加物品（自动分配到背包）
        /// </summary>
        /// <param name="pileOfItem">要添加的物品堆</param>
        void GiveItem(PileOfItem pileOfItem);

        /// <summary>
        /// 从玩家处删除物品（从背包或工具栏中移除，先背包）
        /// </summary>
        /// <param name="pileOfItem">要删除的物品堆</param>
        void DeleteItem(PileOfItem pileOfItem);
    }
}