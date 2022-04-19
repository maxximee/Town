using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using System;

public class Box : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void OnTriggerEnter(Collider other)
    {
        Debug.Log("box collected by " + other.gameObject.name);
        if (other.gameObject.TryGetComponent<MoreMountains.HighroadEngine.AirCarController>(out var controller))
        {
            controller.setCanFire(true);           
            Destroy(gameObject);
            if (other.gameObject.name.Equals("Player #1")) {
                var powerUiObject = GameObject.FindGameObjectWithTag("Power").GetComponent<Image>();
                Color c = powerUiObject.color;
                c.a = 1;
                powerUiObject.color = c;
                await WaitTwoSecondAsync();
                c.a = 0;
                powerUiObject.color = c;
            }
        }
        
    }

    private async Task WaitTwoSecondAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        Debug.Log("Finished waiting.");
    }
}
