using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("retrieving player's selected dragon" + Manager.getSelectedDragon());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
