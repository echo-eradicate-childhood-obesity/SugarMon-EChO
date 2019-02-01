using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

    public List<string> tutorialStagePics;

    [HideInInspector]
    public int pic;
    private GameObject tree;
    // Use this for initialization
    void Start () {
        tree = GameObject.Find("Magic Tree");
	}
	
	// Update is called once per frame
	void Update () {

        //Touch touch = Input.GetTouch(0);

        //if (Input.GetKeyDown(KeyCode.A) || touch.phase == TouchPhase.Ended)
        //if(Input.GetKeyDown(KeyCode.A))
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetKeyDown(KeyCode.A))
        {
            //First stage of tutorial
            if (GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage == 0)
            {
                if (pic == tutorialStagePics.Count)
                {
                    pic = 0;
                    GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage++;
                    Debug.Log("Stage Num: " + GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage);
                    PlayerPrefs.SetInt("TutorialStage", GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage);
                    Debug.Log("Destroy");
                    GameObject.Find("Main Camera").GetComponent<SimpleDemo>().Invoke("ClickStart", 1f);
                    Destroy(gameObject);
                    Destroy(tree);
                }
                else
                {
                    this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + tutorialStagePics[pic]);
                    if (tutorialStagePics[pic] == "0-2")
                    {
                        tree.GetComponentInChildren<Text>().text = "Together, we can find all 247 Sugar Monsters!";
                    }
                    else if (tutorialStagePics[pic] == "0-3")
                    {
                        tree.GetComponentInChildren<Text>().text = "Start by aiming a food or beverage barcode at the center of the square.";

                        //Modify mask position
                        this.GetComponent<RectTransform>().sizeDelta = new Vector2(1040, 2080);
                        this.GetComponent<RectTransform>().localPosition = new Vector2(0, -65);
                    }
                }
                pic++;
            }
            //Second stage of tutorial
            else if (GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage == 1)
            {
                if (pic == tutorialStagePics.Count)
                {
                    pic = 0;
                    GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage++;
                    Debug.Log("Stage Num: " + GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage);
                    PlayerPrefs.SetInt("TutorialStage", GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage);
                    Debug.Log("Destroy");
                    Destroy(gameObject);
                    Destroy(tree);
                    GameObject.Find("Canvas").GetComponent<FindAddedSugar>().DisplayMonsters();
                }
                else
                {
                    this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + tutorialStagePics[pic]);
                    if(GameObject.Find("No Added Sugar") != null || GameObject.Find("Not Found") != null)
                    {
                        tree.GetComponentInChildren<Text>().text = "Click the checkmark to continue.";
                    }
                    else
                    {
                        tree.GetComponentInChildren<Text>().text = "Click the checkmark to add it to your SugarDex!";
                    }
                    this.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 2071);
                    this.GetComponent<RectTransform>().localPosition = new Vector2(0, 60);
                }
                pic++;
            }
            //Third stage of tutorial
            else if (GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage == 2)
            {
                if (pic == tutorialStagePics.Count)
                {
                    pic = 0;
                    GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage++;
                    Debug.Log("Stage Num: " + GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage);
                    PlayerPrefs.SetInt("TutorialStage", GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage);
                    Debug.Log("Destroy");
                    Destroy(gameObject);
                    Destroy(GameObject.Find("Tutorial Dex"));
                    Destroy(tree);
                }
            }
        }

    }
    public static void initMask()
    {
        GameObject tutorialMask = Instantiate(Resources.Load("Prefabs/TutorialMask"), GameObject.Find("Canvas").transform) as GameObject;
        GameObject magicTree = Instantiate(Resources.Load("Prefabs/Magic Tree"), GameObject.Find("Canvas").transform) as GameObject;
        tutorialMask.name = "Tutorial Mask";
        magicTree.name = "Magic Tree";
        GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().pic = 1;
        magicTree.GetComponent<RectTransform>().localPosition = new Vector2(0, 0 - GameObject.Find("Canvas").GetComponent<RectTransform>().rect.height / 4);
    } 
}
