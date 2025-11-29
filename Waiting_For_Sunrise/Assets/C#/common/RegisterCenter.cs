using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Assets.C_.common
{ 
    public class RegisterCenter
    {
        public RegisterCenter() { }

        public static void RegisterAll()
        {
            IconRegister iconRegister = new IconRegister();
            ItemRegister itemRegister = new ItemRegister();
            EquipmentRegister equipmentRegister = new EquipmentRegister();

            iconRegister.Register();
            itemRegister.Register();
            equipmentRegister.Register();
        }
    }
}