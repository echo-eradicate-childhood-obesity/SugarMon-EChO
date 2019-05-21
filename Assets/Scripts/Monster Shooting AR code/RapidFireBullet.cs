/*
 * This file was created by Mark Botaish on May 16th, 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFireBullet : MonoBehaviour {

    public GameObject _particles;
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        /*
         * ALL MONSTERS SHOULD HAVE THE TAG "MONSTER"
         * If you hit a monster and you have not hit anything before, then spawn a particle. 
         * Is was possible to hit more than one colliders at once; therefore a bool is 
         * needed to check if it has been hit yet
         */
        if (other.tag == "Monster" && !hasHit) {
            GameObject temp = Instantiate(_particles, gameObject.transform.position, Quaternion.identity);
            Destroy(temp, 1); //Destroy particles after 1 second
            hasHit = true;
            Destroy(gameObject);             
        }
    }
}
