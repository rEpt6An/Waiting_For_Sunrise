namespace Assets.C_.common
{
    public class ItemConverter
    {
        public static Item Do2E(ItemDo itemDO)
        {
            Item item = new()
            {
                Id = itemDO.Id,
                Name = itemDO.Name,
                Description = itemDO.Description,
                Rarity = RarityExtensions.GetById(itemDO.Rarity)
            };
            IconManager iconManager = new();
            item.Icon = iconManager.GetIcon(itemDO.IconId);
            return item;
        }
    }
}