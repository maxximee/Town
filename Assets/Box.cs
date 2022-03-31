using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("box collected by " + other.gameObject.name);
        if (other.gameObject.GetComponent<MoreMountains.HighroadEngine.AirCarController>() != null)
        {
            other.gameObject.GetComponent<MoreMountains.HighroadEngine.AirCarController>().setCanFire(true);
            Destroy(gameObject);
        }
        
    }
}
