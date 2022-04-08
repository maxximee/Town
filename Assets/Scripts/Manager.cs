using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    private static string selectedDragon = "0";


    void Awake()
    {
        
    }

    public static void setSelectedDragon(string dragonNumber)
    {
        selectedDragon = dragonNumber ;
        Debug.Log("selected dragon set to : " + selectedDragon);
    }

    public static string getSelectedDragon()
    {
        return selectedDragon;
    }
}

