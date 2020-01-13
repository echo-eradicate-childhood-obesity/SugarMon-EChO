/*
 * This file was created by Mark Botaish on May 16th, 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileScript : MonoBehaviour {

    public GameObject _particalSystem;

    private float _speed;
    private Vector3 _dir;

    private bool hasHit = false;

    public void Start()
    {
        Destroy(gameObject, 3); //Destroy the yourself after 3 seconds 
    }

    /*
     * Sets up the direction and speed of the projectile. 
     * <This function should get called anytime an object that has this script is spawned>
     */
    public void SetProjectile(Vector3 dir, float speed = 50.0f)
    {
        _speed = speed;
        _dir = dir.normalized; //Normalize the vector just in case

        gameObject.GetComponent<Rigidbody>().velocity = _dir * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
         * ALL MONSTERS SHOULD HAVE THE TAG "MONSTER"
         * If you hit a monster and you have not hit anything before, then spawn a particle. 
         * Is was possible to hit more than one colliders at once; therefore a bool is 
         * needed to check if it has been hit yet
         */
        if(collision.gameObject.tag == "Monster" && !hasHit)
        {
            GameObject part = Instantiate(_particalSystem, collision.transform.position, Quaternion.identity);
            Destroy(part, 1); //Destory the particle after 1 seconds
            Destroy(gameObject);
            hasHit = true;
        }
       
    }
}
