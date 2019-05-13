using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;



public class FamilyContentController : MonoBehaviour
{

    //for the dropdown version


    public GameObject familyContentObject;
    [SerializeField]
    private List<GameObject> familyNames = new List<GameObject>();

    RectTransform familyRT;
    void Start()
    {
        familyRT = familyContentObject.GetComponent<RectTransform>();

    }
    public void FamilyContentPosition()
    {

        var test = gameObject.GetComponent<TMP_Dropdown>();
        var thisopts = test.options;
        var thisval = test.value;
        var thisname = thisopts[thisval].text.ToString();
        //familyNames = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().fms;
        //GameObject tle = GameObject.Find(this.name + " Monsters Title");
        foreach (GameObject go in familyNames)
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
    private void TitleControl()
    {
        //if()
        var d = transform.GetComponent<TMP_Dropdown>();
        if (-7 <= familyRT.localPosition.y && 1090 >= familyRT.localPosition.y)
          {
              d.captionText.text = "Cane";
          }
          else if (1090 < familyRT.localPosition.y && 2160 >= familyRT.localPosition.y)
          {
              d.captionText.text = "Dextrin";
          }
           else if (2160 < familyRT.localPosition.y && 3800 >= familyRT.localPosition.y)
          {
              d.captionText.text = "OSE";
          }
           else if (3800 < familyRT.localPosition.y && 8400 >= familyRT.localPosition.y)
          {
              d.captionText.text = "Concentrate";
          }
           else if (8400 < familyRT.localPosition.y && 13700 >= familyRT.localPosition.y)
          {
              d.captionText.text = "Syrup";
          }
           else if (13700 < familyRT.localPosition.y && 17900 >= familyRT.localPosition.y)
          {
              d.captionText.text = "Sugar";
          }
            else if (17900 < familyRT.localPosition.y)
          {
              d.captionText.text = "Other";
          }
    }

    void Update()
    {
        TitleControl();
        Debug.Log(familyRT.localPosition.y);
    }

}
