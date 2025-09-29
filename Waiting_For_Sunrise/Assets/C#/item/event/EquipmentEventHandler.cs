using Assets.C_.bus;
using Assets.C_.player.player;

namespace Assets.C_.item
{
    public class EquipmentEventHandler
    {

        // 确保初始化
        private static readonly EquipmentEventHandler _instance = new EquipmentEventHandler();


        static EquipmentEventHandler()
        {
            EventBus.Subscribe<EquipmentEvent>(Handle);
        }

        public static void Equip(int itemId, IPlayerState playerState)
        {
            EventBus.Publish(new EquipmentEvent(true, itemId, playerState));
        }

        public static void UnEquip(int itemId, IPlayerState playerState)
        {
            EventBus.Publish(new EquipmentEvent(false, itemId, playerState));
        }

        private static void Handle(EquipmentEvent eventData)
        {
            IEquipment equipment = EquipmentManager.EQUIPMENTS[eventData.ItemId];
            if (equipment != null)
            {
                throw new NoSuchEquipmentException();
            }
            if (eventData.IsEquipped)
            {
                equipment.Execute(eventData.PlayerState);
            } else
            {
                equipment.ExecuteRemove(eventData.PlayerState);
            }
            
        }
    }
}