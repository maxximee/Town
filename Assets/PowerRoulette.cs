using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.HighroadEngine;
using Random = UnityEngine.Random;
using TMPro;

public class PowerRoulette : MonoBehaviour
{
    [SerializeField] private Image ButtonOverlayImage;
    [SerializeField] private Sprite DefaultButtonSprite;
    [SerializeField] private Ability[] abilities;

    [SerializeField] private Animator itemAnimator;

    [SerializeField] private Image ItemHolderImage;
    [SerializeField] private TextMeshProUGUI AltButtonText;
    [SerializeField] private GameObject ItemHolder;

    Dictionary<AbilityProbability, List<int>> abilityProbabilities = new Dictionary<AbilityProbability, List<int>>();

    private void Awake()
    {
        abilityProbabilities.Add(AbilityProbability.often, new List<int>());
        abilityProbabilities.Add(AbilityProbability.rare, new List<int>());
        abilityProbabilities.Add(AbilityProbability.extraRare, new List<int>());
        AirCarController.OnAbilityDoneEvent += onAbilityDone;
        AirCarController.AbilityRemaningEvent += onAbilityUsed;

        for (int i = 0; i < abilities.Length; i++)
        {
            abilityProbabilities[abilities[i].probability].Add(i);
        }

    }

    private void onAbilityDone()
    {
        ButtonOverlayImage.sprite = DefaultButtonSprite;
    }

    private void onAbilityUsed(int remainingCharge)
    {
        if (remainingCharge == 0) {
            AltButtonText.text = "";
        } else {
            AltButtonText.text = remainingCharge.ToString();
        }
    }

    public Ability StartRoulette()
    {
        ItemHolder.SetActive(true);
        Ability ability = GetRandomAbility();
        StartCoroutine(Wait6Sec());
        StartCoroutine(SpinItemRoutine(ability));
        return ability;
    }

    private Ability GetRandomAbility()
    {
        int rand = UnityEngine.Random.Range(0, 100);
        int amountOfAbilities = 0;

        // TOOD just for testing, giving all abilities same probability
        amountOfAbilities = abilityProbabilities[AbilityProbability.often].Count;
        return abilities[abilityProbabilities[AbilityProbability.often][UnityEngine.Random.Range(0, amountOfAbilities)]];
        // also check if we have any abilities in that category at all, ex: if we don't have "often" ability, move to next rarity
        // switch (rand)
        // {
        //     case int n when (n <= 60):
        //         amountOfAbilities = abilityProbabilities[AbilityProbability.often].Count;
        //         return abilities[abilityProbabilities[AbilityProbability.often][UnityEngine.Random.Range(0, amountOfAbilities)]];
        //     case int n when (n < 80):
        //         amountOfAbilities = abilityProbabilities[AbilityProbability.rare].Count;
        //         return abilities[abilityProbabilities[AbilityProbability.rare][UnityEngine.Random.Range(0, amountOfAbilities)]];
        //     default:
        //         amountOfAbilities = abilityProbabilities[AbilityProbability.extraRare].Count;
        //         return abilities[abilityProbabilities[AbilityProbability.extraRare][UnityEngine.Random.Range(0, amountOfAbilities)]];
        // }
    }


    private IEnumerator Wait6Sec()
    {
        yield return new WaitForSeconds(6);
        ItemHolder.SetActive(false);
    }

    private IEnumerator SpinItemRoutine(Ability ability)
    {
        itemAnimator.SetBool("Ticking", true);
        float dur = 3;
        float spd = Random.Range(9f, 11f); // variation, for flavor.
        float x = 0;
        while (x < dur)
        {
            x += Time.deltaTime;

            itemAnimator.speed = (spd - 1) / (dur * dur) * (x - dur) * (x - dur) + 1;
            yield return null;
        }

        itemAnimator.SetBool("Ticking", false);
        ItemHolderImage.sprite = ability.abilityIcon;
        ButtonOverlayImage.sprite = ability.abilityIcon;
    }
}
