using Assets.C_.common;
using Assets.C_.shop;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static void Init()
    {
        RegisterCenter.RegisterAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        TestShop.TestFush();
    }
        // Update is called once per frame
        void Update()
        {

        }
}
