using Assets.C_.common.common;

namespace Assets.C_.player.bag
{
    public interface IPlayerToolbar
    {
        /// <summary>
        /// 获取第index格的PileOfItem
        /// </summary>
        PileOfItem GetPileOfItem(int index);


        /// <summary>
        /// 获取所有，拿到的是副本
        /// </summary>
        PileOfItem[] GetAll();

        /// <summary>
        /// 从指定索引位置弹出指定数量的物品
        /// </summary>
        /// <param name="index">物品所在Toolbar的索引位置</param>
        /// <param name="num">要弹出的物品数量</param>
        /// <returns>弹出的PileOfItem</returns>
        PileOfItem Pop(int index, int num);

        /// <summary>
        /// 删除指定的物品堆
        /// </summary>
        /// <param name="item">要删除的物品堆</param>
        void Delete(PileOfItem item);

        /// <summary>
        /// 在指定索引位置添加物品堆
        /// </summary>
        /// <param name="index">要添加物品的索引位置</param>
        /// <param name="item">要添加的物品堆</param>
        void Add(int index, PileOfItem item);

        /// <summary>
        /// 统计指定物品ID的总数量
        /// </summary>
        /// <param name="itemId">要统计的物品ID</param>
        /// <returns>该物品的总数量</returns>
        int Count(int itemId);

        /// <summary>
        /// 切换到指定索引的工具栏
        /// </summary>
        /// <param name="index">要切换到的工具栏索引</param>
        void ChangeToolbar(int index);

        /// <summary>
        /// 获取当前工具栏的索引
        /// </summary>
        /// <returns>当前工具栏的索引值</returns>
        int GetNowToolbarIndex();

        /// <summary>
        /// 获取当前工具栏的物品堆信息
        /// </summary>
        /// <returns>当前工具栏的物品堆</returns>
        PileOfItem GetNowToolbarPileOfItem();
    }
}