using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growleaves : MonoBehaviour {


    public float maxGrowth = 1;
    public float speedGrowth = 0.0001f;
    Vector3 tempScale;
    public Tree TestTreeObject;
    public GameObject targetObject;

    private bool hideObject;
    // Use this for initialization

    void Start()
    {
        hideObject = true;
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
       
        yield return new WaitForSeconds(5);
        hideObject = false;
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
        }
    }
 
// Update is called once per frame
void Update()
{

    if (hideObject)
        {
            targetObject.SetActive(false);
        }
        else if (!hideObject)
        {
            targetObject.SetActive(true);
        }
}
}

