using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerRoulette : MonoBehaviour
{
    [SerializeField] private Image ButtonOverlayImage;
    [SerializeField] private Ability[] abilities;

    Dictionary<AbilityProbability, List<int>> abilityProbabilities = new Dictionary<AbilityProbability, List<int>>();

    private void Awake()
    {
        abilityProbabilities.Add(AbilityProbability.often, new List<int>());
        abilityProbabilities.Add(AbilityProbability.rare, new List<int>());
        abilityProbabilities.Add(AbilityProbability.extraRare, new List<int>());

        for (int i = 0; i < abilities.Length; i++)
        {
            abilityProbabilities[abilities[i].probability].Add(i);
        }
    }

    public void StartRoulette()
    {
        GetRandomAbility();
    }

    private Ability GetRandomAbility()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        int amountOfAbilities = 0;
        switch (rand)
        {
            case int n when (n <= 70):
                amountOfAbilities = abilityProbabilities[AbilityProbability.often].Count;
                return abilities[abilityProbabilities[AbilityProbability.often][UnityEngine.Random.Range(0, amountOfAbilities)]];
            case int n when (n < 90):
                amountOfAbilities = abilityProbabilities[AbilityProbability.rare].Count;
                return abilities[abilityProbabilities[AbilityProbability.rare][UnityEngine.Random.Range(0, amountOfAbilities)]];
            default:
                amountOfAbilities = abilityProbabilities[AbilityProbability.extraRare].Count;
                return abilities[abilityProbabilities[AbilityProbability.extraRare][UnityEngine.Random.Range(0, amountOfAbilities)]];
        }
    }


}
