using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SwapAbility : Ability
{

    public float swapRaySpeed = 2200f;
    public int amount = 10;
    public override void Activate(Transform parentTranform)
    {
        Debug.Log("swap!");
    }
}
