using System.Collections.Generic;
using UnityEngine;
namespace Assets.C_.common
{ 
    public class RegisterCenter
    {
        public static readonly List<AbstractRegister> REGISTERS= new();

        public RegisterCenter() { }

        private static void AddRegister()
        {
            REGISTERS.Add(new IconRegister());
            REGISTERS.Add(new ItemRegister());
            REGISTERS.Add(new EquipmentRegister());
        }

        public static void RegisterAll()
        {
            AddRegister();
            foreach (AbstractRegister register in REGISTERS) { 
                register.Register();
            }
        }
    }
}