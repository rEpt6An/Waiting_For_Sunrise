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

        public static void Equip(int itemId)
        {
            EventBus.Publish(new EquipmentEvent(true, itemId));
        }

        public static void UnEquip(int itemId)
        {
            EventBus.Publish(new EquipmentEvent(false, itemId));
        }

        private static void Handle(EquipmentEvent eventData)
        {
            EquipmentManager equipmentManager = EquipmentManager.GetInstance();
            IEquipment equipment = equipmentManager.Get(eventData.ItemId);
            if (equipment != null)
            {
                throw new NoSuchEquipmentException();
            }
            if (eventData.IsEquipped)
            {
                equipment.Execute(PlayerState.Instance);
            } else
            {
                equipment.ExecuteRemove(PlayerState.Instance);
            }
        }
    }
}