using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FireballAbility : Ability
{
    public GameObject fireball;
    public float fireballSize = 1f;
    public float fireballSpeed = 1500f;
    public float fireballReach = 100f;
    public Transform initPosition;


    protected RaycastHit hit;

    public override void Activate(Transform parentTranform)
    {
        Vector3 fwd = parentTranform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(parentTranform.position, fwd, out hit, fireballReach)) //Finds the point where you click with the mouse
        {
            GameObject projectile = Instantiate(fireball, initPosition.position, Quaternion.identity) as GameObject; //Spawns the selected projectile
            projectile.tag = "Projectile";
            projectile.name = name + "-Projectile";
            projectile.transform.LookAt(hit.point); //Sets the projectiles rotation to look at the point clicked
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * fireballSpeed); //Set the speed of the projectile by applying force to the rigidbody
        }
    }
}
