using System.Collections.Generic;
using UnityEngine;
namespace Assets.C_.common
{ 
    public class RegisterCenter
    {
        public static readonly List<IRegister> REGISTERS= new();

        public RegisterCenter() { }

        private static void AddRegister()
        {
            REGISTERS.Add(new IconRegister());
            REGISTERS.Add(new ItemRegister());
        }

        public static void RegisterAll()
        {
            AddRegister();
            foreach (IRegister register in REGISTERS) { 
                register.Register();
            }
        }
    }
}