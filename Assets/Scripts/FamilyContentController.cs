using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class FamilyContentController : MonoBehaviour {

    public GameObject familyContentObject;
    private List<string> familyNames;
    
    void Start()
    {

    }
    public void FamilyContentPosition()
    {
        familyNames = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().fms;
        GameObject tle = GameObject.Find(this.name + " Monsters Title");
        float titleHeight = tle.GetComponent<RectTransform>().rect.height;
        Vector2 newPosition = new Vector2(familyContentObject.GetComponent<RectTransform>().localPosition.x, Math.Abs(tle.GetComponent<RectTransform>().localPosition.y) - titleHeight);
        familyContentObject.GetComponent<RectTransform>().localPosition = newPosition;
    }
}
