using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  All in game enums are declared here.
/// </summary>
namespace AirCraftCombat.CoreData
{
    public enum BulletType
    {
        Player = 0,
        Enemy = 1
    }

    public enum PrefabType
    {
        Small_1,
        Small_2,
        Medium_1,
        Medium_2,
        Boss
    }

    public enum PowerUp
    {
        None = -1,
        Shield = 0,
        Spread = 1
    }

    public enum Formation
    {
        Type_1 = 0,
        Type_2 = 1,
        Type_3 = 2,
        Type_4 = 3,
        Type_5 = 4
    }

}
