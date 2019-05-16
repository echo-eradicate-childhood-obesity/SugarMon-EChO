using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFireBullet : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster") {
            print("Monster Hit");
            Destroy(gameObject);
        }
    }
}
