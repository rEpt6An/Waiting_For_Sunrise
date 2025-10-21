
using Assets.C_.player.player;

namespace Assets.C_.item
{
    public class EquipmentEvent
    {
        // true=װ����false=ж��
        public bool IsEquipped { get; private set; }
        public int ItemId { get; private set; }
        public IPlayerState PlayerState { get; private set; }

        public EquipmentEvent(bool isEquipped, int itemId, IPlayerState playerState)
        {
            IsEquipped = isEquipped;
            ItemId = itemId;
            PlayerState = playerState;
        }
    }
}