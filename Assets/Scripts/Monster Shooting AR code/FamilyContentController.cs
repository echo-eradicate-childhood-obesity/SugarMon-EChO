using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;



public class FamilyContentController : MonoBehaviour {
    public GameObject familyContentObject;
    [SerializeField]
    private List<GameObject> familyNames = new List<GameObject>();
    public List<Sprite> Sprites;

    RectTransform familyRT;
    void Start() {
        familyRT = familyContentObject.GetComponent<RectTransform>();
    }
    public void FamilyContentPosition() {

        var test = gameObject.GetComponent<TMP_Dropdown>();
        var thisopts = test.options;
        var thisval = test.value;
        var thisname = thisopts[thisval].text.ToString();
        
        test.options[thisval].text = (Regex.Replace(thisname, "(\\(.*\\))", "")).Trim();  //Remove "New" after the family name
        test.options[thisval].image = Sprites[thisval];  //Reset the family image

        foreach (GameObject go in familyNames) {
            if (go.name.Substring(0, 2) == thisname.Substring(0, 2)) {
                GameObject tle = GameObject.Find(go.name + " Monsters Title");
                float titleHeight = tle.GetComponent<RectTransform>().rect.height;
                Vector2 newPosition = new Vector2(familyContentObject.GetComponent<RectTransform>().localPosition.x, Math.Abs(tle.GetComponent<RectTransform>().localPosition.y) - titleHeight);
                familyContentObject.GetComponent<RectTransform>().localPosition = newPosition;
            }
        }
    }
    
    private void TitleControl() {
       
        var dropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>().options;
        var d = transform.GetComponent<TMP_Dropdown>();
        if (-7 <= familyRT.localPosition.y && 1070 >= familyRT.localPosition.y) {
            d.captionText.text = "Dextrin";
            d.captionImage.sprite = Sprites[0];
            dropdown[0].text = "Dextrin";
            dropdown[0].image = Sprites[0];

        }
        else if (1070 < familyRT.localPosition.y && 3000 >= familyRT.localPosition.y) {
            d.captionText.text = "OSE";
            d.captionImage.sprite = Sprites[1];
            dropdown[1].text = "OSE";
            dropdown[1].image = Sprites[1];

        }
        else if (3000 < familyRT.localPosition.y && 6400 >= familyRT.localPosition.y) {
            d.captionText.text = "Cane";
            d.captionImage.sprite = Sprites[2];
            dropdown[2].text = "Cane";
            dropdown[2].image = Sprites[2];
        }
        else if (6400 < familyRT.localPosition.y && 10000 >= familyRT.localPosition.y) {
            d.captionText.text = "Syrup";
            d.captionImage.sprite = Sprites[3];
            dropdown[3].text = "Syrup";
            dropdown[3].image = Sprites[3];
        }
        else if (10000 < familyRT.localPosition.y && 15000 >= familyRT.localPosition.y) {
            d.captionText.text = "Concentrate";
            d.captionImage.sprite = Sprites[4];
            dropdown[4].text = "Concentrate";
            dropdown[4].image = Sprites[4];
        }
        else if (15000 < familyRT.localPosition.y && 19000 >= familyRT.localPosition.y) {
            d.captionText.text = "Obvious";
            d.captionImage.sprite = Sprites[5];
            dropdown[5].text = "Obvious";
            dropdown[5].image = Sprites[5];
        }
        else if (19000 < familyRT.localPosition.y) {
            d.captionText.text = "Strange";
            d.captionImage.sprite = Sprites[6];
            dropdown[6].text = "Strange";
            dropdown[6].image = Sprites[6];
        }
    }
    void Update() {
        TitleControl();
    }
}