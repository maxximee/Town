using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SwapAbility : Ability
{
    public override void Activate(Transform parentTranform)
    {
      Debug.Log("swap!");
    }
}
