using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SugarCollection : MonoBehaviour {

    //singleton ref
    UIManager um;

    [HideInInspector]
    public int foundMonsterNumber;

    public GameObject sugarDiskImage;                //Sugar dex background
    public GameObject mainCam;
    private Vector3 diskPosition;
    public GameObject canvas;

    private List<string> sugarFromMain, newSugars;
    public List<string> allCollectedSugars;
    private int numCount;
    public List<string> scannedAddedSugars;

    [HideInInspector]
    public bool sugarDexOpen = false;
    
    void Start () {

        Transform sugarDiskImage = GameObject.Find("Canvas").transform.Find("CollectedSugarCanvas");
        diskPosition = sugarDiskImage.transform.localPosition;
        foundMonsterNumber = 0;
        newSugars = new List<string>();

        um = UIManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenSugarDisk()
    {

        sugarDexOpen = true;
        UpdateDexData();
    }
    public void CloseSugarDisk()
    {
        sugarDexOpen = false;
        mainCam.GetComponent<SimpleDemo>().enabled = true;
        if (mainCam.GetComponent<SimpleDemo>().tutorialStage != 0) mainCam.GetComponent<SimpleDemo>().StartScan();
        GameObject.Find("SugarCollection").transform.Find("RedDot").gameObject.SetActive(false);
        sugarDiskImage.gameObject.SetActive(false);

        um.DisAllUp("Notification");
    }
    public void UpdateDexData()
    {
        mainCam.GetComponent<SimpleDemo>().enabled = false;

        newSugars.Clear();
        sugarDiskImage.transform.localPosition = diskPosition;
        canvas.transform.Find("CollectedSugarCanvas").gameObject.SetActive(true);
        GameObject.Find("CollectedSugarCanvas").transform.Find("TopBar/Found Count").GetComponent<Text>().text = "Found: " + canvas.GetComponent<FindAddedSugar>().allScanned.Count;

        scannedAddedSugars = canvas.GetComponent<FindAddedSugar>().scannedAddedSugars;
        sugarFromMain = canvas.GetComponent<FindAddedSugar>().allScanned;

        //Get new types of sugar in ingredient but sugar dex
        foreach (string ni in sugarFromMain)
        {
            if (!allCollectedSugars.Contains(ni) && ni.ToLower() != "no added sugar")
            {
                newSugars.Add(ni);
                allCollectedSugars.Add(ni.ToLower());
            }
        }
        UpdateSugarDex(canvas.GetComponent<FindAddedSugar>().dbList, newSugars);

    }

    /// <summary>
    /// Reset all found types of sugar and tutorial stage for testing
    /// </summary>
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }


    /// <summary>
    /// Change the image of found sugar in sugar dex
    /// </summary>
    /// <param name="dbList">the list of product list</param>
    /// <param name="newSugars">types of sugar not in sugar dex</param>
    public void UpdateSugarDex(List<List<string>> dbList, List<string> newSugars)
    {
        foreach (List<string> s in dbList)
        {
            foreach (string ss in newSugars)
            {
                if (s[canvas.GetComponent<FindAddedSugar>().nameIndex].ToLower() == ss.ToLower())
                {
                    var sc = GameObject.Find(s[canvas.GetComponent<FindAddedSugar>().deckNumIndex]);
                    var sci = sc.transform.Find("Image");

                    if (sc != null)
                    {
                        sc.name = ss;
                        List<string> sugarWords = ss.Split(' ').ToList();

                        //if the sugar name is too long, split to more words
                        if (sugarWords[0].ToCharArray().Count() > 12)
                        {
                            string text = char.ToUpper(ss[0]) + ss.Substring(1);
                            text = text.Insert(12, "- ");
                            sc.transform.Find("Name").GetComponent<Text>().text = text;

                        }
                        else sc.transform.Find("Name").GetComponent<Text>().text = char.ToUpper(ss[0]) + ss.Substring(1);

                        sci.GetComponentInChildren<Text>().text = "";
                        sci.GetChild(0).gameObject.SetActive(false);

                        //placing and resizing the monster image in sugardex
                        sci.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                        sci.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);
                        sci.GetComponent<RectTransform>().localScale = new Vector2(1.5f, 1.5f);

                        sci.GetComponent<Image>().color = Color.white;
                    }
                }
            }

        }
    }
}
