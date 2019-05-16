using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFireBullet : MonoBehaviour {

    public GameObject _particles;
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster" && !hasHit) {
            GameObject temp = Instantiate(_particles, gameObject.transform.position, Quaternion.identity);
            Destroy(temp, 1);
            print("Monster Hit");
            hasHit = true;
            Destroy(gameObject);            
        }
    }
}
