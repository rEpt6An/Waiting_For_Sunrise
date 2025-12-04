

using Assets.C_.item;
using Assets.C_.common;
namespace Assets.C_.player.bag
{
    public class PlayerBag: IPlayerBag
    {
        public IPlayerToolbar PlayerToolbar { get; private set; }
        public IPlayerInventory PlayerInventory { get; private set; }

        public PlayerBag()
        {
            PlayerToolbar = new PlayerToolbar();
            PlayerInventory = new PlayerInventory();
        }

        public IPlayerInventory GetInventory()
        {
            return PlayerInventory;
        }

        public IPlayerToolbar GetToolbar()
        {
            return PlayerToolbar;
        }

        /// 优先从库存中删除
        public void DeleteItem(PileOfItem pileOfItem)
        {   
            lock (this)
            {
                int itemId = pileOfItem.ItemId;
                int neededNum = pileOfItem.Count;
                int countInInventory = PlayerInventory.Count(itemId);
                int countInToolbar = PlayerToolbar.Count(itemId);

                if (countInInventory + countInToolbar < neededNum)
                {
                    throw new BagException("物品数量不足");
                }
                if (countInInventory >= neededNum)
                {
                    PlayerInventory.Delete(new PileOfItem(itemId, neededNum));
                }
                else
                {
                    PlayerInventory.Delete(new PileOfItem(itemId, countInInventory));
                    PlayerToolbar.Delete(new PileOfItem(itemId, neededNum - countInInventory));
                    EquipmentEventHandler.UnEquip(itemId);
                }
            }
        }

        public void GiveItem(PileOfItem pileOfItem)
        {
            lock (this)
            {
                PlayerInventory.Add(pileOfItem);
                if (ItemManager.Instance.Get(pileOfItem.ItemId).ItemType == ItemTypeExtensions.GetById(0)) {
                    EquipmentEventHandler.Equip(pileOfItem.ItemId);
                }
            }
        }

        public void MoveToInventory(int index, int num)
        {
            lock (this)
            {
                PileOfItem popedItem = PlayerToolbar.Pop(index, num);
                PlayerInventory.Add(popedItem);
            }
        }

        public void MoveToToolbar(PileOfItem pileOfItem, int targetToolbarIndex)
        {
            lock (this)
            {
                PlayerInventory.Delete(pileOfItem);
                PlayerToolbar.Add(targetToolbarIndex, pileOfItem);
            }
        }
    }
}