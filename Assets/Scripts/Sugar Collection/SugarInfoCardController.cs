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

    private FindAddedSugar fas;

    public GameObject CardImg; // image of the background of the card
    public GameObject MonsterImg; // image of the monster being displayed
    public GameObject CloseBtn; // button that closes the card
    public GameObject SugarNameText; // Name of the sugar displayed above
    public GameObject DescriptionText; // Description of a Sugar taken from the SugarDescriptions database
    public GameObject WhereToFindText; // A sample food group to find any given sugar

    private void Awake() {
        if (instance != null) Destroy(this);
        else instance = this;
        fas = FindAddedSugar.Instance;
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Sets the content for the monster name given
    /// </summary>
    /// <param monName="monName">Name of the monster to display</param>
    public void SetContent(string monName) {
        SugarNameText.GetComponent<TextMeshProUGUI>().text = monName;
        DescriptionText.GetComponent<TextMeshProUGUI>().text = GetDescription(monName);
        WhereToFindText.GetComponent<TextMeshProUGUI>().text = GetWhereToFind(monName);
        MonsterImg.GetComponent<Image>().sprite = fas.GetMonsterDesign(monName);
        if (!fas.MonsterFound(monName)) // if the monster has not yet been found
            MonsterImg.GetComponent<Image>().color = Color.black; // set his image to a silhouette
        else
            MonsterImg.GetComponent<Image>().color = Color.white; // reset the image to having original colors
    }
    /// <summary>
    /// Sets the content for the monster name given
    /// </summary>
    /// <param monName="monName">Name of the monster to display</param>
    private string GetDescription(string monName) {
        string description = "";
        return description;
    }
    /// <summary>
    /// Sets the content for the monster name given
    /// </summary>
    /// <param monName="monName">Name of the monster to display</param>
    private string GetWhereToFind(string monName) {
        string whereToFind = "";
        return whereToFind;
    }
}
