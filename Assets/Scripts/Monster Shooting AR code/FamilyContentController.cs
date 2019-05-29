using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;



public class FamilyContentController : MonoBehaviour
{

    //for the dropdown version


    public GameObject familyContentObject;
    [SerializeField]
    private List<GameObject> familyNames = new List<GameObject>();
    public List<Sprite> Sprites;
    private TMP_Dropdown dropdownComponent;

    RectTransform familyRT;
    void Start()
    {
        familyRT = familyContentObject.GetComponent<RectTransform>();
        dropdownComponent = gameObject.GetComponent<TMP_Dropdown>();
    }
    public void FamilyContentPosition()
    {

        dropdownComponent = gameObject.GetComponent<TMP_Dropdown>();
        var thisopts = dropdownComponent.options;
        var thisval = dropdownComponent.value;
        var thisname = thisopts[thisval].text.ToString();
        
        //Reset selected item when clicked
        dropdownComponent.options[thisval].text = Regex.Replace(thisname, " (\\(.*\\))", "");
        dropdownComponent.options[thisval].image = Sprites[thisval];

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
    }
    private void TitleControl()
    {
        if (-7 <= familyRT.localPosition.y && 1070 >= familyRT.localPosition.y)
        {
            ChangeSpriteAndTextOfDropdownItem("Dextrin", 0);
        }
        else if (1070 < familyRT.localPosition.y && 3000 >= familyRT.localPosition.y)
        {
            ChangeSpriteAndTextOfDropdownItem("OSE", 1);
        }
        else if (3000 < familyRT.localPosition.y && 6400 >= familyRT.localPosition.y)
        {
            ChangeSpriteAndTextOfDropdownItem("Cane", 2);
        }
        else if (6400 < familyRT.localPosition.y && 10000 >= familyRT.localPosition.y)
        {
            ChangeSpriteAndTextOfDropdownItem("Syrup", 3);
        }
        else if (10000 < familyRT.localPosition.y && 15000 >= familyRT.localPosition.y)
        {
            ChangeSpriteAndTextOfDropdownItem("Concentrate", 4);
        }
        else if (15000 < familyRT.localPosition.y && 19000 >= familyRT.localPosition.y)
        {
            ChangeSpriteAndTextOfDropdownItem("Obvious", 5);
        }
        else if (19000 < familyRT.localPosition.y)
        {
            ChangeSpriteAndTextOfDropdownItem("Strange", 6);
        }
    }

    private void ChangeSpriteAndTextOfDropdownItem(string name, int index)
    {
        dropdownComponent.captionText.text = name;
        dropdownComponent.captionImage.sprite = Sprites[index];
        dropdownComponent.value = index;
    }

    void Update()
    {
        TitleControl();
    }

}
