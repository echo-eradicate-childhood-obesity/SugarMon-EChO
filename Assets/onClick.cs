﻿/*
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class onClick : MonoBehaviour {
    Dropdown m_Dropdown;
    public Text m_Text;
    private List<string> familyNames;

    // Use this for initialization
    void Start() {
        //Fetch the Dropdown GameObject
        m_Dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });
        m_Text.text = "First Value : " + m_Dropdown.value;

        public voidDropdownValueChanged(Dropdown change)
        {
            m_Text.text = "New Value : " + change.value;
            familyNames = GameObject.Find("Canvas").GetComponent<FindAddedSugar>().fms;
            GameObject tle = GameObject.Find(this.name + " Monsters Title");
            float titleHeight = tle.GetComponent<RectTransform>().rect.height;
            Vector2 newPosition = new Vector2(m_Dropdown.GetComponent<RectTransform>().localPosition.x, Math.Abs(tle.GetComponent<RectTransform>().localPosition.y) - titleHeight);
            m_Dropdown.GetComponent<RectTransform>().localPosition = newPosition;
        }
    }
}
*/

/*


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
*/