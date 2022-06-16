using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomRouletteAnimResources : MonoBehaviour
{
    [SerializeField] private Image rouletteImage;
    [SerializeField] private Sprite[] rouletteImageList;

    public void PlaySpinTickSound()
    {
        //AudioManager.Play("tickItemUI", AudioManager.MixerTarget.UI);
    }

    public void GetRandomIcon()
    {
        rouletteImage.sprite = RandomElement(rouletteImageList);
    }



    public int RandomIndex(Sprite[] array)
    {
        return Random.Range(0, array.Length);
    }

    public Sprite RandomElement(Sprite[] array)
    {
        return array[RandomIndex(array)];
    }
}
