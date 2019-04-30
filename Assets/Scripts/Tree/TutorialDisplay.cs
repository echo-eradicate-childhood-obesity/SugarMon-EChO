using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDisplay : MonoBehaviour {
    private int tapCount = 0;
    private Transform mask, tree;
    private bool isPressed = false;

    // Use this for initialization
    void Start () {
        mask = GameObject.Find("Canvas").transform.Find("Mask");
        tree = GameObject.Find("Canvas").transform.Find("Tree Bubble");
        tree.GetComponent<RectTransform>().localPosition = new Vector2(0, 0 - GameObject.Find("Canvas").GetComponent<RectTransform>().rect.height / 3 + GameObject.Find("Canvas").GetComponent<RectTransform>().rect.height / 10);
        
    }
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown(KeyCode.A) && isPressed)
        //if(Input.GetTouch(0).phase == TouchPhase.Ended && isPressed)
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetKeyDown(KeyCode.A))&& isPressed)
        {
            ButtonClick();
        }

    }
   
    public void ButtonClick() {
        //bool deviceIsIphoneX = UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX;

        var tipText = tree.GetComponentInChildren<Text>().text;
        switch (tapCount)
        {

            case 4:
                tapCount = -1;
                tree.GetComponentInChildren<Text>().text = "";
                mask.gameObject.SetActive(false);
                tree.gameObject.SetActive(false);
                isPressed = false;
                break;

            case 3:
                mask.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                mask.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                mask.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                mask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                mask.GetComponent<RectTransform>().offsetMax = new Vector2(10, 0);
                mask.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 1950);
                mask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/2-1");
                tree.GetComponentInChildren<Text>().text = "Check out your collection of Sugar Monsters in the SugarDex!";

                mask.gameObject.SetActive(true);
                tree.gameObject.SetActive(true);
                break;

            case 2:
                mask.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                mask.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                mask.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                mask.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
                mask.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 2150);
                mask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/TutorialCard");
                tree.GetComponentInChildren<Text>().text = "Click the checkmark to add the new Sugar Monster to your SugarDex!";

                mask.gameObject.SetActive(true);
                tree.gameObject.SetActive(true);
                break;

            case 0:
                mask.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                mask.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                mask.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                mask.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -68);
                mask.GetComponent<RectTransform>().sizeDelta = new Vector2(1050, 2084);
                mask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/0-3");
                tree.GetComponentInChildren<Text>().text = "Start by aiming a food or beverage barcode at the center of the square.";

                mask.gameObject.SetActive(true);
                tree.gameObject.SetActive(true);
                isPressed = true;
                break;

            default:
                break;
        }
        tapCount++;
    }
}
