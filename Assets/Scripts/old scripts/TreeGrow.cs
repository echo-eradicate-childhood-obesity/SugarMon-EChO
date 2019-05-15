using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrow : MonoBehaviour {

    public float maxGrowth = 1;
    public float speedGrowth = 0.1f;
    Vector3 tempScale;
    public Tree TestTreeObject;
    public static bool ready = false;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
       
       
       // else
       // {
       //     ready = true;
       // }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "can")
        {
            tempScale = transform.localScale;
            if (transform.localScale.x < maxGrowth)
            {
                tempScale.x += speedGrowth;
                tempScale.y += speedGrowth * 2;
                tempScale.z += speedGrowth;
                transform.localScale = tempScale;
            }
            else
            {
                ready = true;
            }
        }

    }

}


