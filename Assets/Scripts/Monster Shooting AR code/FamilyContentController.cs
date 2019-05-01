using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;



public class FamilyContentController : MonoBehaviour {

    //for the dropdown version
   

    public GameObject familyContentObject;
    private List<string> familyNames;
    
    void Start()
    {

    }
    public void FamilyContentPosition()
    {

        var test = gameObject.GetComponent<TMP_Dropdown>();
        var thisopts = test.options;
        var thisval = test.value;
        var thisname = thisopts[thisval].text.ToString();
        familyNames = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().fms;
        //GameObject tle = GameObject.Find(this.name + " Monsters Title");
        GameObject tle = GameObject.Find(thisname + " Monsters Title");
        float titleHeight = tle.GetComponent<RectTransform>().rect.height;
        Vector2 newPosition = new Vector2(familyContentObject.GetComponent<RectTransform>().localPosition.x, Math.Abs(tle.GetComponent<RectTransform>().localPosition.y) - titleHeight);
        familyContentObject.GetComponent<RectTransform>().localPosition = newPosition;
    }
}
