using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{

    public float dashSpeed = 1500f;    


    public override void Activate(Transform parentTranform)
    {
      Debug.Log("dashing!");
    }
}
