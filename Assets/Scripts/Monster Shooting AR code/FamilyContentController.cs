using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;



public class FamilyContentController : MonoBehaviour {

    //for the dropdown version
   

    public GameObject familyContentObject;
    [SerializeField]
    private List<GameObject> familyNames=new List<GameObject>();
   
    void Start()
    {


    }
    public void FamilyContentPosition()
    {

        var test = gameObject.GetComponent<TMP_Dropdown>();
        var thisopts = test.options;
        var thisval = test.value;
        var thisname = thisopts[thisval].text.ToString();
        //familyNames = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().fms;
        //GameObject tle = GameObject.Find(this.name + " Monsters Title");
        foreach(GameObject go in familyNames)
        {
            if (go.name.Substring(0, 2) == thisname.Substring(0, 2))
            {
                GameObject tle = GameObject.Find(go.name + " Monsters Title");
                float titleHeight = tle.GetComponent<RectTransform>().rect.height;
                Vector2 newPosition = new Vector2(familyContentObject.GetComponent<RectTransform>().localPosition.x, Math.Abs(tle.GetComponent<RectTransform>().localPosition.y) - titleHeight);
                familyContentObject.GetComponent<RectTransform>().localPosition = newPosition;
            }
        }
        //GameObject tle = GameObject.Find(thisname + " Monsters Title");
        //float titleHeight = tle.GetComponent<RectTransform>().rect.height;
        //Vector2 newPosition = new Vector2(familyContentObject.GetComponent<RectTransform>().localPosition.x, Math.Abs(tle.GetComponent<RectTransform>().localPosition.y) - titleHeight);
        //familyContentObject.GetComponent<RectTransform>().localPosition = newPosition;

    }
 
}
