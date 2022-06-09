using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float activeTime;
    public float amount;

    public AbilityType type;

    public Sprite abilityIcon;

    public AbilityProbability probability;

    public AbilityState state;
  

    public virtual void Activate(Transform parentTransform) {}


}


