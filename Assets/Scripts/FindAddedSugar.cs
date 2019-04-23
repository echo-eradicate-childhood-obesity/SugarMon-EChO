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
using System;



public class FindAddedSugar : MonoBehaviour
{
    //ref of singleton 
    UIManager um;

    //private IScanner BarcodeScanner;
    public static List<string> repository = new List<string>();
    private static List<string> db = new List<string>();
    public List<List<string>> dbList = new List<List<string>>();
    //public AudioClip newSugarSound, foundSugarSound, noSugarSound;
    public NumbersOfEachSugar sugarCardData;
    public AudioSource Audio;

    private int currentNumMonster = 0;

    protected List<string> upcs;
    protected List<string> ingredients;

    public GameObject scanFrame;
    public GameObject summonSystem;
    public GameObject greenCartGo;

    private int numCount;
    public GameObject sugarDex, redDot, canvas, familyBackground, mainCam;
    public GameObject totalCount, foundCount;

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
        greenCartGo.gameObject.SetActive(false);

        //get singleton ref
        um = UIManager.Instance;
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
            sugarDex.GetComponent<SugarDisk>().allCollectedSugars.Add(PlayerPrefs.GetString("num_" + i));
            allScanned.Add(PlayerPrefs.GetString("num_" + i));
        }

        //Remove duplicates
        allScanned.Distinct().ToList();

        familyBackground.gameObject.SetActive(true);
        GameObject.Find("FamilyContent").GetComponent<PopulateFamilyPanels>().PopulateFamilies();

        
        //Family Background
        foreach (List<string> s in dbList)
        {
            foreach (string ss in sugarDex.GetComponent<SugarDisk>().allCollectedSugars)
            {
                if (s[nameIndex].ToLower() == ss.ToLower())
                {
                    var sc = GameObject.Find(s[deckNumIndex]);
                    if (sc != null)
                    {
                        sc.name = ss;
                        List<string> sugarWords = ss.Split(' ').ToList();
                        sc.transform.Find("Counter").GetComponent<Text>().text = "X" + sugarCardData.GetNumberOfSugar(ss).ToString();
                        if (sugarWords[0].ToCharArray().Count() > 12)
                        {
                            string text = char.ToUpper(ss[0]) + ss.Substring(1);
                            text = text.Insert(12, "- ");
                            sc.transform.Find("Name").GetComponent<Text>().text = text;

                        }
                        else sc.transform.Find("Name").GetComponent<Text>().text = char.ToUpper(ss[0]) + ss.Substring(1);


                        var sci = sc.transform.Find("Image");
                        sci.GetComponentInChildren<Text>().text = "";
                        sci.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        sci.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                        sci.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);
                        sci.GetComponent<RectTransform>().localScale = new Vector2(1.5f, 1.5f);

                        sci.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + s[familyIndex] + "/" + sc.name);
                        //sci.gameObject.AddComponent<Button>().onClick.AddListener(() => summonSystem.GetComponent<SummonSystem>().PopupSugarInfoCardInSugarDex(sc.name, s[familyIndex]));
                    }
                }
            }

        }

        GameObject.Find("SugarDisk").GetComponent<SugarDisk>().allCollectedSugars = sugarDex.GetComponent<SugarDisk>().allCollectedSugars.Distinct().ToList();
        foundCount.GetComponent<Text>().text = "Found: " + sugarDex.GetComponent<SugarDisk>().allCollectedSugars.Count;
        totalCount.GetComponent<Text>().text = "Total: " + repository.Count;
        sugarDex.GetComponent<SugarDisk>().CloseSugarDisk();
    }


    // Update is called once per frame
    void Update()
    {


    }
    
    public void AllTypeOfSugars(string ingredientFromDB, string bcv)
    {

        sugarDex.GetComponent<Button>().enabled = false;

        //Barcode not in database
        if (ingredientFromDB == "Not Found")
        {
            scanFrame.SetActive(false);
            CreateSugarMonster("Not Found");
        }
        else
        {
            currentNumMonster = 0;
            scannedAddedSugars.Clear();
            
            ingredientFromDB = Regex.Replace(ingredientFromDB, "[^a-zA-Z0-9_.,; ]+", "");
            ingredientFromDB = ingredientFromDB.Replace('.', ',').Replace(';', ',');
            List<string> dbIngredientList = ingredientFromDB.Split(',').ToList();
            dbIngredientList = dbIngredientList.ConvertAll(item => item.Trim());



            if (mainCam.GetComponent<SimpleDemo>().superBarCode)
            {
                dbIngredientList = repository;  //this line of code is for getting all sugar to test
                mainCam.GetComponent<SimpleDemo>().superBarCode = false;
            }

            foreach (string r in repository)
            {
                if (dbIngredientList.Contains(r.ToLower()))
                {
                    dbIngredientList.IndexOf(r.ToLower());
                    sugarCardData.GetSugar(r);
                    scannedAddedSugars.Add(char.ToUpper(r[0]) + r.Substring(1));
                    if (!allScanned.Contains(r.ToLower()))
                    {
                        allScanned.Add(r.ToLower());


                        //this is the newly add indicator showing func.
                        foreach (List<string> sl in dbList)
                        {
                            if (sl[nameIndex].ToLower() == r.ToLower())
                            {
                                Info info = new Info(sl[familyIndex]);
                                um.IndicateController(info,"Notification");
                            }
                        }
                        

                        numCount++;
                        //playerprefAs.set array
                        PlayerPrefs.SetString("num_" + numCount, r.ToLower());
                        //playerprefs.set array.length()
                        PlayerPrefs.SetInt("count", numCount);
                    }
                }
            }
            scannedAddedSugars.Clear();
            if (scannedAddedSugars.Count == 0)
            {
                //add green cart code here
                GreenCartController.Instance.PCAdd(bcv);
                GreenCartController.Instance.PC.PCSave();

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
        var anim = Instantiate(GameObject.Find(scannedAddedSugars[currentNumMonster]), GameObject.Find("Canvas").transform) as GameObject;
        anim.name = "Animation";
        anim.GetComponent<Image>().sprite = monster.GetComponent<Image>().sprite;
        anim.AddComponent<Animator>();
        anim.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/Monster") as RuntimeAnimatorController;


        if (s == "Sugar")
        {
            canvas.transform.Find("Animation/Sugar Name").GetComponent<Text>().text = scannedAddedSugars[currentNumMonster];
            yield return new WaitForSeconds(2f);
            if (currentNumMonster + 1 == scannedAddedSugars.Count)
            {
                Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster]));
            }
            else
            {
                GameObject.Find(scannedAddedSugars[currentNumMonster]).GetComponentInChildren<Text>().text = scannedAddedSugars[currentNumMonster + 1];
                
            }
            ChangeNextCardText();
            anim.GetComponent<Animator>().Play("SugarCardToDex");
            yield return new WaitForSeconds(1f);
            Destroy(anim);
        }
        else if (s == "NoAddedSugar")
        {
            yield return new WaitForSeconds(2f);
            if (currentNumMonster + 1 == scannedAddedSugars.Count)
            {
                Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster]));
            }
            anim.GetComponent<Animator>().Play("SugarCardToDex");
            yield return new WaitForSeconds(1f);
            Destroy(anim);
            ChangeNextCardText();
        }
        else if (s == "NotFound")
        {
            yield return new WaitForSeconds(2f);
            if (currentNumMonster + 1 == scannedAddedSugars.Count)
            {
                Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster]));
            }
            anim.GetComponent<Animator>().Play("NotFoundCard");
            yield return new WaitForSeconds(1f);
            scanFrame.SetActive(true);
            Destroy(anim);
            ChangeNextCardText();
        }
        
    }
    private void ChangeNextCardText()
    {
        currentNumMonster++;
        if (currentNumMonster == scannedAddedSugars.Count)
        {
            if (redDot.gameObject.activeSelf)
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
            sugarDex.GetComponent<Button>().enabled = true;
            mainCam.GetComponent<SimpleDemo>().Invoke("ClickStart", 3f); //wait for 3 seconds for next scan
        }
        else
        {

            if (!sugarInWall.Contains(scannedAddedSugars[currentNumMonster].ToLower()))
            {

#if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
#endif
                monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NewAddedSugar");
                redDot.gameObject.SetActive(true);
            }
            else monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CollectedAddedSugar");

            if (!sugarInWall.Contains(scannedAddedSugars[currentNumMonster].ToLower())) sugarInWall.Add(scannedAddedSugars[currentNumMonster].ToLower());



            monster.name = scannedAddedSugars[currentNumMonster];
            canvas.transform.Find(scannedAddedSugars[currentNumMonster] + "/Sugar Name").GetComponent<Text>().text = scannedAddedSugars[currentNumMonster];
            DisplayMonsterDesign(monster.name.ToLower());

            //Audio.Play();
            DisplayMonsters();

        }
    }
    public void DisplayMonsters()
    {
        ts = mainCam.GetComponent<SimpleDemo>().tutorialStage;
        if (scannedAddedSugars.Contains("Not Found"))
        {

            StartCoroutine("AnimatorSugarCardToDex", "NotFound");
            GameObject.Destroy(GameObject.Find("Not Found"));
            mainCam.GetComponent<SimpleDemo>().Invoke("ClickStart", 3f); //wait for 3 seconds for next scan

            sugarDex.GetComponent<Button>().enabled = true;
        }
        else
        {
            if (scannedAddedSugars[currentNumMonster] != "No Added Sugar")
            {

                StartCoroutine("AnimatorSugarCardToDex", "Sugar");
            }
            else StartCoroutine("AnimatorSugarCardToDex", "NoAddedSugar");
        }
    }


    //Instantiate monster
    public void CreateSugarMonster(string sugarName)
    {
        
        ts = mainCam.GetComponent<SimpleDemo>().tutorialStage;
        
        monster = Instantiate(Resources.Load("Prefabs/Monster"), GameObject.Find("Canvas").transform) as GameObject;

        monster.name = sugarName;

        if (sugarName == "No Added Sugar")
        {
            monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NoAddedSugar");
            monster.transform.Find("SugarDesign").gameObject.SetActive(false);
        }
        else if (sugarName == "Not Found")
        {
            monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Not Found Card");
            monster.transform.Find("SugarDesign").gameObject.SetActive(false);
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
                redDot.gameObject.SetActive(true);
            }
            else monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CollectedAddedSugar");
            GameObject.Find("Canvas").transform.Find(sugarName + "/Sugar Name").GetComponent<Text>().text = sugarName;

            DisplayMonsterDesign(sugarName);
            
            if (!sugarInWall.Contains(sugarName.ToLower())) sugarInWall.Add(sugarName.ToLower());

        }
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
            else if (sugarName == "Not Found")
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
        else
        {
            DisplayMonsters();
        }
    }

    public void DisplayMonsterDesign(string sugarName)
    {
        var indexOfSugar = repository.IndexOf(sugarName.ToLower()) + 1;
        var monsterFamily = dbList[indexOfSugar][familyIndex];
        if(monsterFamily == "Dextrin Monsters" || monsterFamily == "Cane Monsters") monster.transform.Find("SugarDesign").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + monsterFamily + "/" + sugarName);
        else monster.transform.Find("SugarDesign").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + monsterFamily);
    }
}
