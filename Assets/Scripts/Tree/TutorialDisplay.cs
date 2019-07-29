using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDisplay : MonoBehaviour {

    public GameObject mask, tree, canvas;
    private int tapCount = 0;
    private bool isPressed = false;
    private List<string> tutorialStagePics;
    public static readonly string[] dialog = new string[] { "Start by aiming a food or beverage barcode at the center of the square.",
                                                      "You will collect a Sugar Monster for each added sugar in the product!",
                                                      "Check out your collection of Sugar Monsters in the SugarDex!",
                                                      "All of the food you have scanned will be stored in the FoodDex!"};

    // Use this for initialization
    void Start () {
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        tree.GetComponent<RectTransform>().localPosition = new Vector2(0, 0 - canvasHeight / 3 + canvasHeight / 10);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || 
              Input.GetButtonDown("Fire1")) && isPressed)
        {
            ButtonClick();
        }

    }
   
    public void ButtonClick() {
        var tipText = tree.GetComponentInChildren<Text>().text;
        switch (tapCount)
        {

            case 5:
                tapCount = -1;
                tree.GetComponentInChildren<Text>().text = "";
                ToggleTutorialObjects(false);
                isPressed = false;
                break;
            case 4:
                PositionTutorialMask(new Vector2(0, -205), new Vector2(20, 1820), "DexMask", dialog[3]);
                ToggleTutorialObjects(true);
                break;
            case 3:
                PositionTutorialMask(new Vector2(0, -205), new Vector2(20, 1820), "FoodMask", dialog[2]);
                ToggleTutorialObjects(true);
                break;

            case 2:
                PositionTutorialMask(new Vector2(0, -150), new Vector2(160, 2150), "TutorialCard", dialog[1]);
                ToggleTutorialObjects(true);
                break;

            case 0:
                PositionTutorialMask(new Vector2(0, -310), new Vector2(160, 2084), "0-3", dialog[0]);
                ToggleTutorialObjects(true);
                isPressed = true;
                break;

            default:
                break;
        }
        tapCount++;
    }
    private void PositionTutorialMask(Vector2 anchoredPosition, Vector2 sizeDelta, string maskName, string content)
    {
        RectTransform maskRect = mask.GetComponent<RectTransform>();
        mask.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        mask.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
        mask.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
        mask.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        mask.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        mask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + maskName);
        tree.GetComponentInChildren<Text>().text = content;
    }

    private void ToggleTutorialObjects(bool b)  //Enable and disable mask and tree
    {
        mask.gameObject.SetActive(b);
        tree.gameObject.SetActive(b);
    }

    /// <summary>
    /// Display different tutorial based on the stage 
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="sugarName"></param>
    public void DisplayTutorial(int stage, string sugarName)
    {
        TutorialController.initMask();
        GameObject magicTree = GameObject.Find("Magic Tree"), tutorialMask = GameObject.Find("Tutorial Mask");
        Debug.Log(stage);
        //first stage
        if (stage == 0)
        {                    
            magicTree.GetComponentInChildren<Text>().text = "Hi friend, I am the Magic Tree. I am here to help you grow healthier.";
            GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics = new List<string>() { "0-1", "0-2", "0-3" };
            
        }
        //second stage
        else if(stage == 1)
        {
            
            GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics = new List<string>() { "1-1", "1-2" };

            if (sugarName == "No Added Sugar")
            {
                magicTree.GetComponentInChildren<Text>().text = "Yay! Looks like you found a healthy food with 0 Sugar Monsters!";
            }
            else if (sugarName == "Not Found")
            {
                magicTree.GetComponentInChildren<Text>().text = "I’m still growing so check back again in the future!";
            }
            else
            {
                magicTree.GetComponentInChildren<Text>().text = "Wow! Looks like you found a Sugar Monster!";
            }
            stage++;
            PlayerPrefs.SetInt("TutorialStage", stage);
        }

        else if(stage == 2)
        {
           
            magicTree.GetComponentInChildren<Text>().text = "Check out your collection of Sugar Monsters in the SugarDex!";
            GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics = new List<string>() { "2-1" };

            tutorialMask.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            tutorialMask.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
            tutorialMask.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
            tutorialMask.GetComponent<RectTransform>().anchoredPosition = new Vector2(2, -15);
            tutorialMask.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 2000);

            stage++;   //Increase the stage of tutorial
            PlayerPrefs.SetInt("TutorialStage", stage);
        }
        tutorialMask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics[0]);
    }

}
