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

    private int testnum = 0;

    public AudioSource Audio;

    private int currentNumMonster = 0;
    private int numOfAddedSugar;

    private int upcIndex;
    private int ingredientIndex;
    protected List<string> upcs;
    protected List<string> ingredients;

    private GameObject disk;
    private GameObject mask;
    private GameObject scannButton;
    public GameObject scanFrame;

    [HideInInspector]
    public int familyIndex, deckNumIndex, nameIndex, repoNumIndex;

    [HideInInspector]
    public List<string> sugarInWall, scannedAddedSugars = new List<string>(), allScanned = new List<string>();
    
    private GameObject monster;

    // Use this for initialization


    void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
    void Start() {

        sugarInWall = new List<string>();
        disk = GameObject.Find("SugarDisk");
        //mask = GameObject.Find("Mask");

        scannButton = GameObject.Find("ScanButton");
        //BarcodeScanner = new Scanner();
        //BarcodeScanner.Camera.Play();
        upcs = new List<string>();
        ingredients = new List<string>();

        //Read Sugar Repository
        //TextAsset txt = (TextAsset)Resources.Load("The Added Sugar Repository", typeof(TextAsset));
        //string content = txt.text;
        //repository = content.Split(new char[] { '\n' }).ToList();
        //repository = repository.ConvertAll(item => Regex.Replace(item, @"\(.*?\)", "").ToLower().Trim());    //Remove product name in parentheses after added sugar names

        //Read Database 
        TextAsset dbtxt = (TextAsset)Resources.Load("Database", typeof(TextAsset));
        string dbcontent = dbtxt.text;
        db = dbcontent.Split(new char[] { '\n' }).ToList();

        //Debug.Log(usdacontent);

        //Save data in a list of lists
        for (int i = 0; i < db.Count; i++)
        {
            dbList.Add(db[i].Split(new char[] { '\t' }).ToList());
            dbList[i] = dbList[i].ConvertAll(item => item.Trim());
        }
        familyIndex = dbList[0].IndexOf("MonstersFamily");
        deckNumIndex = dbList[0].IndexOf("Number in the App");
        nameIndex = dbList[0].IndexOf("Added Sugar List Name");
        repoNumIndex = dbList[0].IndexOf("Number in Added Sugar Repository");  //use only when you need sugar number in sugar repository

        //Print all sugar names
        //foreach(List<string> s in dbList)
        //{
        //    Debug.Log(s[nameIndex]);
        //}

        repository = new List<string>();
        foreach (List<string> s in dbList)
        {
            repository.Add(s[nameIndex].ToLower());
        }

        //Initiative sugar disk
        GameObject.Find("Canvas").transform.FindChild("Background").gameObject.SetActive(true);
        GameObject.Find("Content").GetComponent<PopulateGrid>().Populate();
        GameObject.Find("SugarDisk").GetComponent<SugarDisk>().CloseSugarDisk();

        
    }
    

    // Update is called once per frame
    void Update () {
        

    }

    public void GetFoundAddedSugar()
    {
        //List<string> scannedAddedSugars = new List<string>() { "Sugar", "Brown Sugar" }; 
        
    }
    //public void TriggerGoogleVision()
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

    //public static List<string> CheckAddedSugar(List<string> ingredients)
    public void AllTypeOfSugars(string ingredientFromDB)
    {
        //Get the list of added sugars from the disk
        //var sugarFromDisk = disk.GetComponent<SugarDisk>().sugarInWall;

        //var ingredients = new List<string>() { "syrup", "natural flavors", "peppermint oil", "sugar" };

        //Check if each ingredient is added sugar 
        List<List<string>> ingredients = new List<List<string>>();
        //ingredients.Add(new List<string>() { "natural flavors", "juice concentrate", "peppermint" });
        ingredients.Add(new List<string>() { "syrup", "natural flavors", "peppermint oil" });
        ingredients.Add(new List<string>() { "brown sugar", "strawberry", "white sugar" });
        scannButton.SetActive(false);

        int n = 0;
        numOfAddedSugar = 0;
        currentNumMonster = 0;

        scannedAddedSugars.Clear();
        //allScanned.Clear();

        foreach (string r in repository)
        {
            if (ingredientFromDB.Contains(r.ToLower()))
            {
                scannedAddedSugars.Add(char.ToUpper(r[0]) + r.Substring(1));
            }
        }
        

        //foreach (string i in ingredients[testnum])
        //{
        //    if (repository.Contains(i.ToLower()))
        //    {
        //        if (!sugarInWall.Contains(char.ToUpper(i[0]) + i.Substring(1))) sugarInWall.Add(char.ToUpper(i[0]) + i.Substring(1));
        //        scannedAddedSugars.Add(char.ToUpper(i[0]) + i.Substring(1));
        //    }     
        //}
        foreach (string s in scannedAddedSugars)
        {
            if (!allScanned.Contains(s)) allScanned.Add(s);
        }
        //foreach (string s in scannedAddedSugars) Debug.Log(s);

        //No added sugar

        foreach (string s in scannedAddedSugars) Debug.Log(s);

        if (scannedAddedSugars.Count == 0)
        {
            //Change image of monster
            scannedAddedSugars.Add("No Added Sugar");
            CreateSugarMonster(scannedAddedSugars[currentNumMonster]);
            //mask.SetActive(false);
            scanFrame.SetActive(false);
        }
        //Include added sugar
        else
        {
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(GameObject.Find("ScanButton").GetComponent<Button>().onClick, displayMonsters);
            //UnityEditor.Events.UnityEventTools.RemovePersistentListener(GameObject.Find("ScanButton").GetComponent<Button>().onClick, AllTypeOfSugars);
            //mask.SetActive(false);
            scanFrame.SetActive(false);


            CreateSugarMonster(scannedAddedSugars[currentNumMonster]);

        }
        testnum++;
        if (testnum == ingredients.Count) testnum = 0;



        //return scannedAddedSugars;
    }
    public void DisplayMonsters()
    {
        //GameObject.Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster]));
        currentNumMonster++;
        //Debug.Log(currentNumMonster + ": " + scannedAddedSugars.Count);
        if (currentNumMonster == scannedAddedSugars.Count)
        {
            GameObject.Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster - 1]));
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(GameObject.Find("ScanButton").GetComponent<Button>().onClick, AllTypeOfSugars);
            //UnityEditor.Events.UnityEventTools.RemovePersistentListener(GameObject.Find("ScanButton").GetComponent<Button>().onClick, displayMonsters);
            //mask.SetActive(true);
            scanFrame.SetActive(true);
            scannButton.SetActive(true);
        }
        else
        {
            monster.name = scannedAddedSugars[currentNumMonster];
            this.GetComponentInChildren<Text>().text = scannedAddedSugars[currentNumMonster];
            //Audio.Play();
        }
    }


    //Instantiate monster
    public void CreateSugarMonster(string sugarName)
    {
        
        if (!this.GetComponent<AudioSource>().isActiveAndEnabled)
        {
            this.GetComponent<AudioSource>().enabled = true;
            this.GetComponent<AudioSource>().clip = newSugarSound;
        }

        GameObject stage = GameObject.Find("RawImage");
        monster = Instantiate(Resources.Load("Prefabs/Monster"), stage.transform) as GameObject;
        monster.name = sugarName;

        //Audio.Play();
        


        //UnityEditor.Events.UnityEventTools.AddPersistentListener(this.GetComponentInChildren<Button>().onClick, DisplayMonsters);
        this.GetComponentInChildren<Button>().onClick.AddListener(() => DisplayMonsters());
        monster.transform.localScale = new Vector3(20, 25, 0);
        if (sugarName == "No Added Sugar")
        {
            monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NoAddedSugar");
        }
        else
        {


            if (!sugarInWall.Contains(sugarName.ToLower()))
            {
                monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NewAddedSugar");
                GameObject.Find("SugarDisk").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/SugarDexButtonNotification");
            }
            else monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CollectedAddedSugar");

            this.GetComponentInChildren<Text>().text = sugarName;

            if (!sugarInWall.Contains(sugarName.ToLower())) sugarInWall.Add(sugarName.ToLower());

        }
        //monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + sugarName);
        //monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/monster");
    }
}
