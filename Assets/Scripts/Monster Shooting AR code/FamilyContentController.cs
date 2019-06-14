using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;



public class FamilyContentController : MonoBehaviour
{
    public GameObject familyContentObject;
    [SerializeField]
    private List<GameObject> familyNames = new List<GameObject>();
    public List<Sprite> Sprites;

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
        if (-7 <= familyRT.localPosition.y && 1070 >= familyRT.localPosition.y)
        {
            d.captionText.text = "Dextrin";
            d.captionImage.sprite = Sprites[0];

        }
        else if (1070 < familyRT.localPosition.y && 3000 >= familyRT.localPosition.y)
        {
            d.captionText.text = "OSE";
            d.captionImage.sprite = Sprites[1];
        }
        else if (3000 < familyRT.localPosition.y && 6400 >= familyRT.localPosition.y)
        {
            d.captionText.text = "Cane";
            d.captionImage.sprite = Sprites[2];
        }
        else if (6400 < familyRT.localPosition.y && 10000 >= familyRT.localPosition.y)
        {
            d.captionText.text = "Syrup";
            d.captionImage.sprite = Sprites[3];
        }
        else if (10000 < familyRT.localPosition.y && 15000 >= familyRT.localPosition.y)
        {
            d.captionText.text = "Concentrate";
            d.captionImage.sprite = Sprites[4];
        }
        else if (15000 < familyRT.localPosition.y && 19000 >= familyRT.localPosition.y)
        {
            d.captionText.text = "Obvious";
            d.captionImage.sprite = Sprites[5];
        }
        else if (19000 < familyRT.localPosition.y)
        {
            d.captionText.text = "Strange";
            d.captionImage.sprite = Sprites[6];
        }
    }

    void Update() {
        TitleControl();
        //Debug.Log(familyRT.localPosition.y);
    }
}
