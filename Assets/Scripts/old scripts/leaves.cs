using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaves : MonoBehaviour {

    public float maxGrowth = 1;
    public float speedGrowth = 0.1f;
    Vector3 tempScale;
    public Tree TestTreeObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "can" || other.gameObject.name == "fert" )
        {
            tempScale = transform.localScale;
            if (transform.localScale.x < maxGrowth)
            {
                tempScale.x += speedGrowth;
                tempScale.y += speedGrowth * 2;
                tempScale.z += speedGrowth;
                transform.localScale = tempScale;
            }
        }
    }
 

    // Update is called once per frame
    void Update()
    {

       



      
    }
}
