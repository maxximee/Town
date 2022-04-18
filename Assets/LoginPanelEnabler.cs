using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanelEnabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Manager.isFirstStart()) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
