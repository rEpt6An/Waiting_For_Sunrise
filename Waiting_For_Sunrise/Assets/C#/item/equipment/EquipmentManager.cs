using System.Collections.Generic;

namespace Assets.C_.item
{
    public class EquipmentManager
    {
        public static readonly Dictionary<int, IEquipment> EQUIPMENTS = new();

        private static EquipmentManager Instance = null;

        private EquipmentManager() {
            AddToEQUIPMENTS(new Lamp());
        }

        private static void AddToEQUIPMENTS(IEquipment equipment)
        {
            EQUIPMENTS.Add(equipment.Id, equipment);
        }

        public static EquipmentManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new EquipmentManager();
            }
            return Instance;
        }
    }
}