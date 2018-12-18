using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SugarDisk : MonoBehaviour {
    private GameObject foundSugar;

    [HideInInspector]
    public int foundMonsterNumber;

    public GameObject sugarDiskImage;
    private Vector3 diskPosition;
    private GameObject cv;
    private GameObject[] allTypesOfSugars;

    private List<string> sugarFromMain, newSugars;
    public List<string> allCollectedSugars;
    private int numCount;
    // Use this for initialization
    void Start () {

        Transform sugarDiskImage = GameObject.Find("Canvas").transform.Find("FamilyBackground");
        diskPosition = sugarDiskImage.transform.localPosition;
        foundMonsterNumber = 0;
        foundSugar = GameObject.Find("Canvas");
        cv = GameObject.Find("Canvas");
        newSugars = new List<string>();

        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenSugarDisk()
    {
        GameObject.Find("Main Camera").GetComponent<SimpleDemo>().enabled = false;
        newSugars.Clear();
        sugarDiskImage.transform.localPosition = diskPosition;
        foundSugar.transform.Find("FamilyBackground").gameObject.SetActive(true);
        GameObject.Find("FamilyBackground").transform.Find("TopBar/Found Count").GetComponent<Text>().text = "Found: " + foundSugar.GetComponent<FindAddedSugar>().allScanned.Count;


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
            foreach(string ss in newSugars)
            {
                if (s[cv.GetComponent<FindAddedSugar>().nameIndex].ToLower() == ss.ToLower())
                {
                    var sc = GameObject.Find(s[cv.GetComponent<FindAddedSugar>().deckNumIndex]);
                    var sci = sc.transform.Find("Image");

                    if (sc != null)
                    {
                        sc.name = ss;
                        sc.transform.Find("Name").GetComponent<Text>().text = char.ToUpper(ss[0]) + ss.Substring(1);
                        sc.transform.Find("Image").GetComponentInChildren<Text>().text = "";
                        
                        
                        //placing and resizing the monster image in sugardex
                        sci.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
                        sci.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                        sci.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);
                        sci.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/monster");

                        //Change monster color
                        Debug.Log(s[cv.GetComponent<FindAddedSugar>().familyIndex]);
                        if(s[cv.GetComponent<FindAddedSugar>().familyIndex] == "Dextrin Monsters") sci.GetComponent<Image>().color = new Color32(255, 0, 0, 100);
                        
                        else if (s[cv.GetComponent<FindAddedSugar>().familyIndex] == "Cane Monsters") sci.GetComponent<Image>().color = new Color32(0, 255, 0, 100);

                        else if (s[cv.GetComponent<FindAddedSugar>().familyIndex] == "OSE Monsters") sci.GetComponent<Image>().color = new Color32(0, 0, 225, 100);

                        else if (s[cv.GetComponent<FindAddedSugar>().familyIndex] == "Syrup Monsters") sci.GetComponent<Image>().color = new Color32(255, 255, 225, 100);

                        else if (s[cv.GetComponent<FindAddedSugar>().familyIndex] == "Concentrate Monsters") sci.GetComponent<Image>().color = new Color32(0, 0, 0, 100);

                        else if (s[cv.GetComponent<FindAddedSugar>().familyIndex] == "Sugar Monsters") sci.GetComponent<Image>().color = new Color32(255, 0, 200, 100);

                        else if (s[cv.GetComponent<FindAddedSugar>().familyIndex] == "Dextrin Monsters") sci.GetComponent<Image>().color = new Color32(255, 255, 0, 100);

                        else if (s[cv.GetComponent<FindAddedSugar>().familyIndex] == "Other Monsters") sci.GetComponent<Image>().color = new Color32(255, 120, 0, 100);

                    }
                }
            }
            
        }
    }
    public void CloseSugarDisk()
    {
        GameObject.Find("Main Camera").GetComponent<SimpleDemo>().enabled = true;
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Sugar Disk White");
        GameObject.Find("Canvas").transform.Find("FamilyBackground").gameObject.SetActive(false);
    }


    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
