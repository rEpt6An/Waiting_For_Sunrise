using UnityEngine;

namespace Assets.C_.common
{
    public class ItemConverter
    {
        public static Item Do2E(ItemDo itemDO)
        {
            //打印传入的 itemDO 信息
            //UnityEngine.Debug.Log($"[ItemConverter] Converting ItemDo: ID={itemDO.Id}, Name={itemDO.Name}, Describe= {itemDO.Description}, IconId={itemDO.IconId}");

            IconManager iconManager = IconManager.Instance;


            // 尝试获取图标并打印详细信息
            Icon icon = null;
            try
            {
                icon = iconManager.Get(itemDO.IconId);
                if (icon == null)
                {
                    // UnityEngine.Debug.LogError($"[ItemConverter] Failed to get icon.Image! IconId={itemDO.IconId} returned null.");
                }
                else
                {
                  //  UnityEngine.Debug.Log($"[ItemConverter] Successfully got icon: {icon.Id} (ID={itemDO.IconId})");
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"[ItemConverter] Exception while getting icon (IconId={itemDO.IconId}): {e.Message}");
                throw; // 重新抛出异常以便上层捕获
            }

            // 创建 Item 对象
            Item item = new(
                itemDO.Id,
                itemDO.Name,
                RarityExtensions.GetById(itemDO.Rarity),
                ItemTypeExtensions.GetById(itemDO.ItemType),
                itemDO.Description,
                itemDO.Price,
                icon
            );

            //UnityEngine.Debug.Log($"[ItemConverter] Successfully converted ItemDo to Item: {item.Name} (ID={item.Id}) Description = {item.Description}");
            return item;
        }
    }
}