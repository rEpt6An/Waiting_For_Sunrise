
using Assets.C_.player.player;

namespace Assets.C_.item
{
    public class EquipmentEvent
    {
        // true=װ����false=ж��
        public bool IsEquipped { get; private set; }
        public int ItemId { get; private set; }

        public EquipmentEvent(bool isEquipped, int itemId)
        {
            IsEquipped = isEquipped;
            ItemId = itemId;
        }
    }
}