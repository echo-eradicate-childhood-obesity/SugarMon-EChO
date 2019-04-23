using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SugarDisk : MonoBehaviour {

    private GameObject foundSugar;

    //singleton ref
    UIManager um;

    [HideInInspector]
    public int foundMonsterNumber;

    public NumbersOfEachSugar sugarCardData;
    public GameObject summonSystem;
    public GameObject sugarDiskImage;
    private Vector3 diskPosition;
    private GameObject cv;
    private GameObject[] allTypesOfSugars;
    public GameObject addButtonOnSugarCard;


    private List<string> sugarFromMain, newSugars;
    public List<string> allCollectedSugars;
    private int numCount;
    public List<string> scannedAddedSugars;

    private List<string> newMonsterFamilyDesign = new List<string>() { "Dextrin Monsters", "Cane Monsters" };

    public List<string> monsterColor;
    //change to local
    //private Transform sci;
    // Use this for initialization

    void Start () {

        Transform sugarDiskImage = GameObject.Find("Canvas").transform.Find("FamilyBackground");
        diskPosition = sugarDiskImage.transform.localPosition;
        foundMonsterNumber = 0;
        foundSugar = GameObject.Find("Canvas");
        cv = GameObject.Find("Canvas");
        newSugars = new List<string>();

        um = UIManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenSugarDisk()
    {
        UpdateDexData();

        //if Open from Summon System
        if (summonSystem.activeInHierarchy)
        {
            addButtonOnSugarCard.gameObject.SetActive(true);
        }
        else
        {
            UpdateCounterOfEachSugar();
        }
        
    }
    public void CloseSugarDisk()
    {
        GameObject.Find("Main Camera").GetComponent<SimpleDemo>().enabled = true;
        GameObject.Find("SugarDisk").transform.Find("RedDot").gameObject.SetActive(false);
        sugarDiskImage.gameObject.SetActive(false);
        addButtonOnSugarCard.gameObject.SetActive(false);

        um.DisAllUp("Notification");
    }
    public void UpdateDexData()
    {
        GameObject.Find("Main Camera").GetComponent<SimpleDemo>().enabled = false;

        newSugars.Clear();
        sugarDiskImage.transform.localPosition = diskPosition;
        foundSugar.transform.Find("FamilyBackground").gameObject.SetActive(true);
        GameObject.Find("FamilyBackground").transform.Find("TopBar/Found Count").GetComponent<Text>().text = "Found: " + foundSugar.GetComponent<FindAddedSugar>().allScanned.Count;

        scannedAddedSugars = foundSugar.GetComponent<FindAddedSugar>().scannedAddedSugars;
        sugarFromMain = foundSugar.GetComponent<FindAddedSugar>().allScanned;
        foreach (string ni in sugarFromMain)
        {
            if (!allCollectedSugars.Contains(ni) && ni.ToLower() != "no added sugar")
            {
                newSugars.Add(ni);
                allCollectedSugars.Add(ni.ToLower());

            }
        }
        foreach (List<string> s in foundSugar.GetComponent<FindAddedSugar>().dbList)
        {
            foreach (string ss in newSugars)
            {
                if (s[cv.GetComponent<FindAddedSugar>().nameIndex].ToLower() == ss.ToLower())
                {
                    var sc = GameObject.Find(s[cv.GetComponent<FindAddedSugar>().deckNumIndex]);
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



                        sci.GetComponentInChildren<Text>().text = "";

                        //placing and resizing the monster image in sugardex

                        sci.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                        sci.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);
                        sci.GetComponent<RectTransform>().localScale = new Vector2(1.5f, 1.5f);

                        sci.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + s[cv.GetComponent<FindAddedSugar>().familyIndex]);
                        bool newDesign = newMonsterFamilyDesign.Contains(s[cv.GetComponent<FindAddedSugar>().familyIndex]);
                        string monsterImagePath = newDesign ? "Images/Monsters/" + s[cv.GetComponent<FindAddedSugar>().familyIndex] + "/" + sc.name : "Images/Monsters/" + s[cv.GetComponent<FindAddedSugar>().familyIndex];
                        sci.GetComponent<Image>().sprite = Resources.Load<Sprite>(monsterImagePath);
                        //sci.gameObject.AddComponent<Button>().onClick.AddListener(() => summonSystem.GetComponent<SummonSystem>().PopupSugarInfoCardInSugarDex(sc.name, s[cv.GetComponent<FindAddedSugar>().familyIndex]));
                        sc.transform.Find("Image").GetComponent<Button>().enabled = true;
                    }
                }
            }

        }
    }
    public void UpdateCounterOfEachSugar()
    {
        foreach (string sugar in scannedAddedSugars.ConvertAll(item => item.ToLower()))
        {
            GameObject.Find(sugar).transform.Find("Counter").GetComponent<Text>().text = "X" + sugarCardData.GetNumberOfSugar(sugar).ToString();
        }
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        sugarCardData.sugars.Clear();
    }
}
