using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerClickHandler
{

    
	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Destroy(GameObject.Find("Syrup"));
        GameObject.Destroy(GameObject.Find("SyrupGB"));
    }

    void Update () {

    }
    
}
