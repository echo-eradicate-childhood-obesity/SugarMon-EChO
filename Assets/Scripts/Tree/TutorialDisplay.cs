using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDisplay : MonoBehaviour {

    public GameObject mask, tree, canvas;
    private int tapCount = 0;
    private bool isPressed = false;
    private readonly string[] dialog = new string[] { "Start by aiming a food or beverage barcode at the center of the square.",
                                                      "Click the checkmark to add the new Sugar Monster to your SugarDex!",
                                                      "Check out your collection of Sugar Monsters in the SugarDex!"};

    // Use this for initialization
    void Start () {
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        tree.GetComponent<RectTransform>().localPosition = new Vector2(0, 0 - canvasHeight / 3 + canvasHeight / 10);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || 
              Input.GetKeyDown(KeyCode.A)) && isPressed)
        {
            ButtonClick();
        }

    }
   
    public void ButtonClick() {
        var tipText = tree.GetComponentInChildren<Text>().text;
        switch (tapCount)
        {

            case 4:
                tapCount = -1;
                tree.GetComponentInChildren<Text>().text = "";
                ToggleTutorialObjects(false);
                isPressed = false;
                break;

            case 3:
                PositionTutorialMask(new Vector2(0, -205), new Vector2(20, 1820), "DexMask", dialog[2]);
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

}
