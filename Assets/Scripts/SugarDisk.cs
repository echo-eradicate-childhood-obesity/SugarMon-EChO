using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SugarDisk : MonoBehaviour {

    public GameObject canvas, familyBackground, mainCam, redDot;

    //singleton ref
    UIManager um;

    [HideInInspector]
    public int foundMonsterNumber;

    private Vector3 diskPosition;
    private GameObject[] allTypesOfSugars;

    private List<string> sugarFromMain, newSugars;
    public List<string> allCollectedSugars;

    public List<string> monsterColor;
    //change to local
    //private Transform sci;
    // Use this for initialization

    void Start () {

        diskPosition = familyBackground.transform.localPosition;
        foundMonsterNumber = 0;
        newSugars = new List<string>();

        um = UIManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenSugarDisk()
    {
        mainCam.GetComponent<SimpleDemo>().enabled = false;
        newSugars.Clear();
        familyBackground.transform.localPosition = diskPosition;
        familyBackground.gameObject.SetActive(true);
        familyBackground.transform.Find("TopBar/Found Count").GetComponent<Text>().text = "Found: " + canvas.GetComponent<FindAddedSugar>().allScanned.Count;


        sugarFromMain = canvas.GetComponent<FindAddedSugar>().allScanned;

        foreach (string ni in sugarFromMain)
        {
            if (!allCollectedSugars.Contains(ni) && ni.ToLower() != "no added sugar")
            {
                newSugars.Add(ni);
                allCollectedSugars.Add(ni.ToLower());
  
            }
        }

        foreach (List<string> s in canvas.GetComponent<FindAddedSugar>().dbList)
        {
            foreach(string ss in newSugars)
            {
                if (s[canvas.GetComponent<FindAddedSugar>().nameIndex].ToLower() == ss.ToLower())
                {
                    var sc = GameObject.Find(s[canvas.GetComponent<FindAddedSugar>().deckNumIndex]);
                    var sci = sc.transform.Find("Image");

                    if (sc != null)
                    {
                        sc.name = ss;
                        List<string> sugarWords = ss.Split(' ').ToList();
                        if (sugarWords[0].ToCharArray().Count() > 12)
                        {
                            string text = char.ToUpper(ss[0]) + ss.Substring(1);
                            text = text.Insert(12, "- ");
                            sc.transform.Find("Name").GetComponent<Text>().text = text;

                        }
                        else sc.transform.Find("Name").GetComponent<Text>().text = char.ToUpper(ss[0]) + ss.Substring(1);

                        sc.transform.Find("Image").GetComponentInChildren<Text>().text = "";
                        
                        
                        //placing and resizing the monster image in sugardex
                        sci.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
                        sci.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                        sci.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);
                        sci.GetComponent<RectTransform>().localScale = new Vector2(1.5f, 1.5f);

                        sci.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + s[canvas.GetComponent<FindAddedSugar>().familyIndex] + "/" + sc.name);

                    }
                }
            }
            
        }
    }
    public void CloseSugarDisk()
    {
        mainCam.GetComponent<SimpleDemo>().enabled = true;
        redDot.gameObject.SetActive(false);
        familyBackground.gameObject.SetActive(false);

        um.DisAllUp("Notification");
    }


    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
    
}
