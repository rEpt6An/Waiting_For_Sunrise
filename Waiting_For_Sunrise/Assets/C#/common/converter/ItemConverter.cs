namespace Assets.C_.common
{
    public class ItemConverter
    {
        public static Item Do2E(ItemDo itemDO)
        {
            IconManager iconManager = IconManager.Instance;
            Icon icon = iconManager.Get(itemDO.IconId);
            Item item = new(itemDO.Id, itemDO.Name, RarityExtensions.GetById(itemDO.Rarity), ItemTypeExtensions.GetById(itemDO.ItemType), itemDO.Description, itemDO.Price, icon);
            return item;
        }
    }
}