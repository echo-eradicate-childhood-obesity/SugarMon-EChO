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
                    if (sc != null)
                    {
                        sc.name = ss;
                        sc.transform.Find("Name").GetComponent<Text>().text = char.ToUpper(ss[0]) + ss.Substring(1);
                        sc.transform.Find("Image").GetComponentInChildren<Text>().text = "";
                        
                        //placing and resizing the monster image in sugardex
                        sc.transform.Find("Image").GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
                        sc.transform.Find("Image").GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sc.transform.Find("Image").GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        sc.transform.Find("Image").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                        sc.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);

                        sc.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/monster");
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
