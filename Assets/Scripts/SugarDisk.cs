using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SugarDisk : MonoBehaviour {
    private bool wallStatus;
    private GameObject foundSugar;
    private bool firstGenerate = true;

    [HideInInspector]
    public int foundMonsterNumber;

    private Transform content;
    public GameObject sugarDiskImage;
    private Vector3 diskPosition;
    private GameObject cv;
    private GameObject[] allTypesOfSugars;

    private List<string> allCollectedSugars, sugarFromMain, newSugars;
    

    // Use this for initialization
    void Start () {
        diskPosition = sugarDiskImage.transform.localPosition;
        foundMonsterNumber = 0;
        wallStatus = false;
        foundSugar = GameObject.Find("Canvas");
        cv = GameObject.Find("Canvas");
        allCollectedSugars = new List<string>();
        newSugars = new List<string>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenSugarDisk()
    {
        newSugars.Clear();
        sugarDiskImage.transform.localPosition = diskPosition;
        wallStatus = true;
        foundSugar.transform.FindChild("Background").gameObject.SetActive(true);
        content = GameObject.Find("Background").transform.Find("Scroll View/Viewport/Content");
        GameObject.Find("Background").transform.Find("TopBar/Found Count").GetComponent<Text>().text = "FOUND: " + foundSugar.GetComponent<FindAddedSugar>().allScanned.Count;


        sugarFromMain = foundSugar.GetComponent<FindAddedSugar>().allScanned;

        foreach (string ni in sugarFromMain)
        {
            if (!allCollectedSugars.Contains(ni) && ni.ToLower() != "no added sugar") newSugars.Add(ni);
        }
        allCollectedSugars = sugarFromMain;

        
        foreach (List<string> s in foundSugar.GetComponent<FindAddedSugar>().dbList)
        {
            foreach(string ss in newSugars)
            {

                if (s[cv.GetComponent<FindAddedSugar>().nameIndex].ToLower() == ss.ToLower())
                {
                    var sc = content.transform.Find(s[cv.GetComponent<FindAddedSugar>().deckNumIndex]);
                    //Debug.Log(cv.GetComponent<FindAddedSugar>().deckNumIndex);
                    sc.name = ss;
                    sc.transform.Find("Name").GetComponent<Text>().text = ss;
                    sc.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/monster");
                    //Debug.Log(content.transform.Find(s[1]).transform.Find("Name").gameobject.GetComponent<Text>());
                }
            }
            
        }

        //foreach (string s in foundSugar.GetComponent<FindAddedSugar>().scannedAddedSugars)
        //{
        //    //Debug.Log(s);
        //    //GameObject.Find(s).SetActive(true);

        //    //Active types of added sugar that have already been found
        //    //GameObject.Find("DiskImage").transform.FindChild("Syrup").gameObject.SetActive(true);
        //    //var x = GameObject.Find("DiskImage").transform.Find(s).gameObject;
        //    //Debug.Log(x.name);
        //}


        //Retrieve found added sugars from AllTypeOfSugars() in FindAddedSugar script
        //foreach (string s in foundSugar.GetComponent<FindAddedSugar>().scannedAddedSugars)
        //{
        //    if (!sugarInWall.Contains(s)) sugarInWall.Add(s);
        //}


        //foreach (string s in sugarInWall)
        //{
        //    Debug.Log(s);
        //}
        //Debug.Log(sugarInWall.Count);
    }
    public void CloseSugarDisk()
    {
        wallStatus = false;
        GameObject.Find("Canvas").transform.FindChild("Background").gameObject.SetActive(false);
    }
}
