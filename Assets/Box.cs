using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using System;
using MoreMountains.Feedbacks;
using MoreMountains.HighroadEngine;
public class Box : MonoBehaviour
{

    [SerializeField] MMF_Player feedbacks;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] MeshRenderer meshQMark;

    [SerializeField] RaceManager raceManager;

    [SerializeField] PowerRoulette powerRoulette;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<MoreMountains.HighroadEngine.AirCarController>(out var controller))
        {      
            mesh.enabled = false;
            meshQMark.enabled = false; 
            GetComponent<Collider>().enabled = false;
            // TODO doing this so the AI can shoot as soon as they hit a box
            controller.canFire = true;
            StartCoroutine(DestroyAfter2Sec());
            if (other.gameObject.name.Equals(raceManager.currentPlayer)) {
                feedbacks.PlayFeedbacks();
                Ability ability = powerRoulette.StartRoulette();
                controller.setAbility(ability);
            }
        }
        
    }

    IEnumerator  DestroyAfter2Sec() {
        yield return new WaitForSeconds(2);
        Destroy(gameObject); 
    }

}
