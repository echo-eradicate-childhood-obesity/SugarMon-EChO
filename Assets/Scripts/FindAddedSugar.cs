using BarcodeScanner;
using BarcodeScanner.Scanner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;
using System.IO;
using System.Text;



public class FindAddedSugar : MonoBehaviour
{

    //private IScanner BarcodeScanner;
    private static List<string> repository = new List<string>();
    private static List<string> db = new List<string>();
    public List<List<string>> dbList = new List<List<string>>();
    public AudioClip newSugarSound, foundSugarSound, noSugarSound;

    public GameObject movingMonster;
    public AudioSource Audio;

    private int currentNumMonster = 0;

    private int upcIndex;
    private int ingredientIndex;
    protected List<string> upcs;
    protected List<string> ingredients;

    private GameObject scannButton;
    public GameObject scanFrame;

    private int numCount;


    //need to follow the title in Database.txt
    [Header("Column names")]
    [Tooltip("In put must be exactly the same with the titles in Database.txt")]
    public string numberInAppColumn = "Number in the App";
    public string sugarNameColumn = "Added Sugar List Name";
    public string numberInRepositoryColumn = "Number in Added Sugar Repository";
    public string monsterFamilyColumn = "MonstersFamily";


    [HideInInspector]
    public int familyIndex, deckNumIndex, nameIndex, repoNumIndex, familyNum;

    [HideInInspector]
    public List<string> sugarInWall, fms = new List<string>(), scannedAddedSugars = new List<string>(), allScanned = new List<string>();

    [HideInInspector]
    public Dictionary<string, int> familyDictionary;

    private GameObject monster;

    private int ts;

    // Use this for initialization
    void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
    void Start()
    {
        //Load player's data
        numCount = PlayerPrefs.GetInt("count");
        for (int i = 1; i <= PlayerPrefs.GetInt("count"); i++)
        {
            sugarInWall.Add(PlayerPrefs.GetString("num_" + i));
        }

        upcs = new List<string>();
        ingredients = new List<string>();

        //Read Database 
        TextAsset dbtxt = (TextAsset)Resources.Load("Database", typeof(TextAsset));
        string dbContent = Encoding.UTF7.GetString(dbtxt.bytes);
        db = dbContent.Split(new char[] { '\n' }).ToList();

        //Save data in a list of lists
        for (int i = 0; i < db.Count; i++)
        {
            dbList.Add(db[i].Split(new char[] { '\t' }).ToList());
            dbList[i] = dbList[i].ConvertAll(item => item.Trim());
        }

        familyIndex = dbList[0].IndexOf(monsterFamilyColumn);
        deckNumIndex = dbList[0].IndexOf(numberInAppColumn);
        nameIndex = dbList[0].IndexOf(sugarNameColumn);
        repoNumIndex = dbList[0].IndexOf(numberInRepositoryColumn);  //use only when you need the sugar index in sugar repository


        //Find out how many different families and how many type of sugar they contain 
        //Save in familyDictionary [family name, count]
        //Save all types of added sugar in the repository variable
        familyDictionary = new Dictionary<string, int>();
        repository = new List<string>();
        foreach (List<string> item in dbList)
        {
            repository.Add(item[nameIndex].ToLower());
            if (!familyDictionary.ContainsKey(item[familyIndex]))
            {
                familyDictionary.Add(item[familyIndex], 1);
            }
            else
            {
                int count = 0;
                familyDictionary.TryGetValue(item[familyIndex], out count);
                familyDictionary.Remove(item[familyIndex]);
                familyDictionary.Add(item[familyIndex], count + 1);
            }
        }
        fms = familyDictionary.Keys.ToList();

        //Remove title
        fms.RemoveAt(0);  
        repository.RemoveAt(0);
        familyNum = fms.Count;
        
        //Initiative sugar disk
        for (int i = 1; i <= PlayerPrefs.GetInt("count"); i++)
        {
            GameObject.Find("SugarDisk").GetComponent<SugarDisk>().allCollectedSugars.Add(PlayerPrefs.GetString("num_" + i));
            allScanned.Add(PlayerPrefs.GetString("num_" + i));
        }

        //Remove duplicates
        allScanned.Distinct().ToList();
      
        GameObject.Find("Canvas").transform.Find("FamilyBackground").gameObject.SetActive(true);
        GameObject.Find("FamilyContent").GetComponent<PopulateFamilyPanels>().PopulateFamilies();

        //Family Background
        foreach (List<string> s in dbList)
        {
            foreach (string ss in GameObject.Find("SugarDisk").GetComponent<SugarDisk>().allCollectedSugars)
            {
                if (s[nameIndex].ToLower() == ss.ToLower())
                {
                    var sc = GameObject.Find(s[deckNumIndex]);
                    if (sc != null)
                    {
                        sc.name = ss;
                        
                        sc.transform.Find("Name").GetComponent<Text>().text = char.ToUpper(ss[0]) + ss.Substring(1);

                        var sci = sc.transform.Find("Image");
                        sci.GetComponentInChildren<Text>().text = "";
                        sci.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                        sci.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);

                        sci.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/monster");
                    }
                }
            }

        }

        GameObject.Find("SugarDisk").GetComponent<SugarDisk>().allCollectedSugars = GameObject.Find("SugarDisk").GetComponent<SugarDisk>().allCollectedSugars.Distinct().ToList();
        GameObject.Find("FamilyBackground").transform.Find("TopBar/Found Count").GetComponent<Text>().text = "Found: " + GameObject.Find("SugarDisk").GetComponent<SugarDisk>().allCollectedSugars.Count;
        GameObject.Find("SugarDisk").GetComponent<SugarDisk>().CloseSugarDisk();
    }


    // Update is called once per frame
    void Update()
    {


    }
    //void TriggerGoogleVision()
    //{
    //    //Trigger Google Vision
    //    StartCoroutine(GameObject.Find("Plane").GetComponent<WebCamTextureToCloudVision>().Capture((result) =>
    //    {
    //        result = result.ToLower();
    //        //List<string> resultHolder = result.Replace("\n", ",").Split(',').ToList();
    //        //resultHolder = resultHolder.ConvertAll(item => item.Trim());


    //        Debug.Log("Result: " + result);
    //        foreach (string r in repository) {

    //            if (result.Contains(r.Trim()))  //need to discuss how to check if there is any added sugar in the result from google vision result
    //            {
    //                Debug.Log("Sugar: " + r);
    //            }
    //        }
    //    }));
    //}

    public void AllTypeOfSugars(string ingredientFromDB)
    {

        GameObject.Find("SugarDisk").GetComponent<Button>().enabled = false;

        //Barcode not in database
        if (ingredientFromDB == "Not Found")
        {
            scanFrame.SetActive(false);
            scannedAddedSugars.Add("No Added Sugar");
            CreateSugarMonster("Not Found");
        }
        else
        {
            currentNumMonster = 0;
            scannedAddedSugars.Clear();

            //Split ingredient string to each individual ingredient
            ingredientFromDB = Regex.Replace(ingredientFromDB, "[^a-zA-Z0-9_., ]+", "", RegexOptions.Compiled);
            List<string> dbIngredientList = ingredientFromDB.Split(',').ToList();
            dbIngredientList = dbIngredientList.ConvertAll(item => item.Trim().ToLower());

            foreach (string r in repository)
            {
                if (dbIngredientList.Contains(r.ToLower()))
                {
                    scannedAddedSugars.Add(char.ToUpper(r[0]) + r.Substring(1));
                    if (!allScanned.Contains(r.ToLower()))
                    {
                        allScanned.Add(r.ToLower());

                        numCount++;
                        //playerprefAs.set array
                        PlayerPrefs.SetString("num_" + numCount, r.ToLower());
                        //playerprefs.set array.length()
                        PlayerPrefs.SetInt("count", numCount);
                    }
                }
            }
            if (scannedAddedSugars.Count == 0)
            {
                //Change image of monster
                scannedAddedSugars.Add("No Added Sugar");
                CreateSugarMonster(scannedAddedSugars[currentNumMonster]);
                scanFrame.SetActive(false);
            }
            //Include added sugar
            else
            {
                scanFrame.SetActive(false);
                CreateSugarMonster(scannedAddedSugars[currentNumMonster]);
            }
        }
    }

    private IEnumerator AnimatorSugarCardToDex(string s)
    {
        //Animation - Card To Dex
        var anim = Instantiate(Resources.Load("Prefabs/Monster"), GameObject.Find("Canvas").transform) as GameObject;
        anim.name = "Animation";
        anim.GetComponent<Image>().sprite = monster.GetComponent<Image>().sprite;
        anim.AddComponent<Animator>();
        anim.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/Monster") as RuntimeAnimatorController;

        if (s == "Sugar")
        {
            GameObject.Find("Canvas").transform.Find("Animation/Sugar Name").GetComponent<Text>().text = scannedAddedSugars[currentNumMonster];
            anim.GetComponent<Animator>().Play("SugarCardToDex");
        }
        else if(s == "NoAddedSugar")
        {
            anim.GetComponent<Animator>().Play("SugarCardToDex");
        }
        else if (s == "NotFound")
        {
            anim.GetComponent<Animator>().Play("NotFoundCard");
        }


        yield return new WaitForSeconds(1f);
        Destroy(anim);
    }

    public void DisplayMonsters()
    {
        ts = GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage;
        if (scannedAddedSugars.Contains("Not Found"))
        {
            StartCoroutine("AnimatorSugarCardToDex", "NotFound");
            GameObject.Destroy(GameObject.Find("Not Found"));
            GameObject.Find("Main Camera").GetComponent<SimpleDemo>().Invoke("ClickStart", 3f); //wait for 3 seconds for next scan

            scanFrame.SetActive(true);
            GameObject.Find("SugarDisk").GetComponent<Button>().enabled = true;
        }
        else
        {
            if(scannedAddedSugars[currentNumMonster] != "No Added Sugar") StartCoroutine("AnimatorSugarCardToDex", "Sugar");
            else StartCoroutine("AnimatorSugarCardToDex", "NoAddedSugar");

            currentNumMonster++;
            if (currentNumMonster == scannedAddedSugars.Count)
            {
                if (GameObject.Find("SugarDisk").transform.Find("RedDot").gameObject.activeSelf)
                {
                    //Third stage of tutorial
                    if (ts == 2 && !scannedAddedSugars.Contains("No Added Sugar"))
                    {
                        TutorialController.initMask();
                        GameObject magicTree = GameObject.Find("Magic Tree"), tutorialMask = GameObject.Find("Tutorial Mask");
                        magicTree.GetComponentInChildren<Text>().text = "Check out your collection of Sugar Monsters in the SugarDex!";
                        GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics = new List<string>() { "2-1" };
                        tutorialMask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics[0]);

                        tutorialMask.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                        tutorialMask.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
                        tutorialMask.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                        tutorialMask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                        tutorialMask.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                        tutorialMask.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 1950);

                        ts++;
                        PlayerPrefs.SetInt("TutorialStage", ts);
                    }
                }
                GameObject.Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster - 1]));
                scanFrame.SetActive(true);
                GameObject.Find("SugarDisk").GetComponent<Button>().enabled = true;
                GameObject.Find("Main Camera").GetComponent<SimpleDemo>().Invoke("ClickStart", 3f); //wait for 3 seconds for next scan
            }
            else
            {

                if (!sugarInWall.Contains(scannedAddedSugars[currentNumMonster].ToLower()))
                {

#if UNITY_ANDROID || UNITY_IOS
                    Handheld.Vibrate();
#endif
                    monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NewAddedSugar");
                    GameObject.Find("SugarDisk").transform.Find("RedDot").gameObject.SetActive(true);
                }
                else monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CollectedAddedSugar");

                if (!sugarInWall.Contains(scannedAddedSugars[currentNumMonster].ToLower())) sugarInWall.Add(scannedAddedSugars[currentNumMonster].ToLower());



                monster.name = scannedAddedSugars[currentNumMonster];
                GameObject.Find("Canvas").transform.Find(scannedAddedSugars[currentNumMonster] + "/Sugar Name").GetComponent<Text>().text = scannedAddedSugars[currentNumMonster];
                //Audio.Play();
            }
        }
    }


    //Instantiate monster
    public void CreateSugarMonster(string sugarName)
    {

        //if (!this.GetComponent<AudioSource>().isActiveAndEnabled)
        //{
        //    this.GetComponent<AudioSource>().enabled = true;
        //    this.GetComponent<AudioSource>().clip = newSugarSound;
        //}

        ts = GameObject.Find("Main Camera").GetComponent<SimpleDemo>().tutorialStage;
        GameObject stage = GameObject.Find("Canvas");
        monster = Instantiate(Resources.Load("Prefabs/Monster"), stage.transform) as GameObject;

        monster.name = sugarName;

        
        if (ts == 1)
        {
            TutorialController.initMask();
            GameObject magicTree = GameObject.Find("Magic Tree"), tutorialMask = GameObject.Find("Tutorial Mask");
            GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics = new List<string>() { "1-1", "1-2" };
            tutorialMask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics[0]);

            if (sugarName == "No Added Sugar")
            {
                magicTree.GetComponentInChildren<Text>().text = "Yay! Looks like you found a healthy food with 0 Sugar Monsters!";
            }
            else if(sugarName == "Not Found")
            {
                magicTree.GetComponentInChildren<Text>().text = "I’m still growing so check back again in the future!";
            }
            else
            {
                magicTree.GetComponentInChildren<Text>().text = "Wow! Looks like you found a Sugar Monster!";
            }
            ts++;
            PlayerPrefs.SetInt("TutorialStage", ts);
        }
        //Audio.Play();

        GameObject.Find("Canvas").transform.Find(sugarName).GetComponentInChildren<Button>().onClick.AddListener(() => DisplayMonsters());
        
        if (sugarName == "No Added Sugar")
        {
            monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NoAddedSugar");
        }
        else if (sugarName == "Not Found")
        {
            monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Not Found Card");
            scannedAddedSugars.Add("Not Found");
        }
        else
        {
            //Find new added sugar
            if (!sugarInWall.Contains(sugarName.ToLower()))
            {
                #if UNITY_ANDROID || UNITY_IOS
                    Handheld.Vibrate();
                #endif
                monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NewAddedSugar");
                GameObject.Find("SugarDisk").transform.Find("RedDot").gameObject.SetActive(true);
            }
            else monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CollectedAddedSugar");

            GameObject.Find("Canvas").transform.Find(sugarName + "/Sugar Name").GetComponent<Text>().text = sugarName;

            if (!sugarInWall.Contains(sugarName.ToLower())) sugarInWall.Add(sugarName.ToLower());

        }
    }
}
