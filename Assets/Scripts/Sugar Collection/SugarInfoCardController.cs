/**
 * SugarInfoCardController.cs is attached to the SugarInfoCard Game Object and is responsible for
 * displaying the pop-up info card that appears when a sugar monster is clicked in the SugarDex.
 * Created by Aidan Lee on 6/13/19
 */
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SugarInfoCardController : MonoBehaviour {

    private static SugarInfoCardController instance;
    public static SugarInfoCardController Instance { get { return instance; } }

    public GameObject CardImg; // image of the background of the card
    public GameObject MonsterImg; // image of the monster being displayed
    public GameObject CloseBtn; // button that closes the card
    public GameObject SugarNameText; // Name of the sugar displayed above
    public GameObject DescriptionText; // Description of a Sugar taken from the SugarDescriptions database
    public GameObject WhereToFindText; // A sample food group to find any given sugar

    string name;
    string description;
    string whereToFind;
    private void Awake() {
        if (instance != null) Destroy(this);
        else instance = this;
    }
    // Use this for initialization
    void Start () {
        SugarNameText.GetComponent<TextMeshProUGUI>().text = name;
        DescriptionText.GetComponent<TextMeshProUGUI>().text = description;
        WhereToFindText.GetComponent<TextMeshProUGUI>().text = whereToFind;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Sets the content for the monster name given
    /// </summary>
    /// <param name="name">Name of the monster to display</param>
    public void SetContent(string monName) {
        name = monName;
        MonsterImg.GetComponent<Image>().sprite = FindAddedSugar.Instance.GetMonsterDesign(name);
    }
}
