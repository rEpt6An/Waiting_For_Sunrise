using Assets.C_.common;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;


namespace Assets.C_.item
{
    public class EquipmentManager : AbstractJsonReadable<List<EquipmentDo>, List<Equipment>>, IResourceManager<FileResource, Equipment>
    {
        public static readonly Dictionary<int, Equipment> EQUIPMENTS = new();

        private static readonly object _lock = new();
        private static EquipmentManager Instance = null;

        private EquipmentManager() {
        }

        public static EquipmentManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new EquipmentManager();
            }
            return Instance;
        }

        protected override List<Equipment> Convert(List<EquipmentDo> jsonDo)
        {
            List<Equipment> list = new();
            foreach (EquipmentDo ob in jsonDo)
            {
                list.Add(EquipmentConverter.Do2E(ob));
            }
            return list;
        }

        public Equipment Get(int id)
        {
            Equipment equipment = EQUIPMENTS[id];
            return equipment;
        }

        public List<Equipment> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Load(FileResource fileResource)
        {
            lock (_lock)
            {
                EQUIPMENTS.Clear();
                List<Equipment> equipments = Instance.Read(fileResource);
                foreach (Equipment e in equipments) {
                    AddToEQUIPMENTS(e);
                    //¥Ú”°e.id
                    //UnityEngine.Debug.Log($"Equipment ID: {e.Id}");
                }
                

            }
        }

        private static void AddToEQUIPMENTS(Equipment equipment)
        {
            EQUIPMENTS.Add(equipment.Id, equipment);
        }
    }
}