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
    /// <summary>
    /// Sets the content for the monster name given by accessing the information from Sugar Repository.txt file 
    /// stored in a 2D list in FindAddedSugar called dbList
    /// </summary>
    /// <param monName="monIndex">The number of a monster (displayed in the top right in the sugar dex)</param>
    public void SetContent(int monIndex) {
        SugarNameText.GetComponent<TextMeshProUGUI>().text = fas.dbList[monIndex][fas.nameIndex];
        WhereToFindText.GetComponent<TextMeshProUGUI>().text = fas.dbList[monIndex][fas.hiddenIndex];

        MonsterImg.GetComponent<Image>().sprite = fas.GetMonsterDesign(fas.dbList[monIndex][fas.nameIndex]);
        if (!fas.MonsterFound(fas.dbList[monIndex][fas.nameIndex])) { // if the monster has not yet been found
            MonsterImg.GetComponent<Image>().color = Color.black; // set the monster image to a silhouette
            DescriptionText.GetComponent<TextMeshProUGUI>().text = "";
            /**
             * TODO:
             * Move monster to the top middle of the screen
             * Center&Move Where to Find Me? Label
             * Center&Move WhereToFindText
             */

        }
        else {
            MonsterImg.GetComponent<Image>().color = Color.white; // reset the image to having original colors
            DescriptionText.GetComponent<TextMeshProUGUI>().text = fas.dbList[monIndex][fas.descriptionIndex];
            /**
             * TODO:
             * Move monster to original location
             * Leftside text&Move Where to Find Me? Label
             * Leftside text&Move WhereToFindText
             */
        }
    }
}
