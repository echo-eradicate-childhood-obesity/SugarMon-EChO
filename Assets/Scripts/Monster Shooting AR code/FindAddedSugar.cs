using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Text;
using System.Threading.Tasks;
using System;
using TMPro;


public class FindAddedSugar : MonoBehaviour
{
    //ref of singleton 
    UIManager um;
    private static FindAddedSugar instance;
    public static FindAddedSugar Instance { get { return instance; } }
    //private IScanner BarcodeScanner;
    public static List<string> repository = new List<string>();
    private static List<string> db = new List<string>();
    public List<List<string>> dbList = new List<List<string>>();
    //public AudioClip newSugarSound, foundSugarSound, noSugarSound;
    public NumbersOfEachSugar sugarCardData;
    public AudioSource Audio;
    public RuntimeAnimatorController animController;
    private SimpleDemo simpleDemo;
    private bool wasSkipped = false;


    [SerializeField]
    string superCode;

    private int currentNumMonster = 0;

    protected List<string> upcs;
    protected List<string> ingredients;
    public TextAsset sugarRepository;

    public string toggleOption;
    public Button toggleButton;

    public Sprite Sound;
    public Sprite Vibrate;
    public Sprite Mute;

    public bool vibrate;
    public bool sound;
    private bool soundInitialized = false;
    public AudioSource goodSound;
    public AudioSource badSound;
    public AudioSource unknownSound;
    public AudioSource onSound;
    public AudioSource sweepSound;
    private bool firstBadSound;

    public GameObject scanFrame;
    public GameObject summonSystem;
    public GameObject greenCartGo;
    public GameObject greenCartBtn;
    public GameObject dropdownMenu;

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
    public string hiddenColumn = "Where it's hidden";
    public string descriptionColumn = "Description";


    [HideInInspector]
    public int familyIndex, deckNumIndex, nameIndex, repoNumIndex, familyNum, hiddenIndex, descriptionIndex;

    [HideInInspector]
    public List<string> sugarInWall, fms = new List<string>(), scannedAddedSugars = new List<string>(), allScanned = new List<string>();

    [HideInInspector]
    public Dictionary<string, int> familyDictionary;

    [HideInInspector]
    public List<string> repo;

    private GameObject monster;

    private int ts;

    // Use this for initialization
    void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Sound = Resources.Load<Sprite>("Images/Sound") as Sprite;
        Vibrate = Resources.Load<Sprite>("Images/Vibrate") as Sprite;
        Mute = Resources.Load<Sprite>("Images/Mute") as Sprite;

        if (PlayerPrefs.HasKey("ToggleOption"))
        {
            toggleOption = PlayerPrefs.GetString("ToggleOption");
            switch (toggleOption)
            {
                case "Sound":
                    SetSound();
                    break;
                case "Vibrate":
                    SetVibrate();
                    break;
                case "Mute":
                    SetMute();
                    break;
            }
        } else
        {
            SetSound();
        }
        firstBadSound = true;
        soundInitialized = true;
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
        //TextAsset sugarRepository = (TextAsset)Resources.Load("Database", typeof(TextAsset));
        string dbContent = Encoding.UTF7.GetString(sugarRepository.bytes);
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
        hiddenIndex = dbList[0].IndexOf(hiddenColumn);
        descriptionIndex = dbList[0].IndexOf(descriptionColumn);
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

        repo = repository;

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


        //Update Family Background
        sugarDex.GetComponent<SugarDisk>().UpdateSugarDex(dbList, sugarDex.GetComponent<SugarDisk>().allCollectedSugars);
        #region Old Update Family Backgound Func
        //foreach (List<string> s in dbList)
        //{
        //    foreach (string ss in sugarDex.GetComponent<SugarDisk>().allCollectedSugars)
        //    {
        //        if (s[nameIndex].ToLower() == ss.ToLower())
        //        {
        //            var sc = GameObject.Find(s[deckNumIndex]);
        //            if (sc != null)
        //            {
        //                sc.name = ss;
        //                List<string> sugarWords = ss.Split(' ').ToList();
        //                sc.transform.Find("Counter").GetComponent<Text>().text = "X" + sugarCardData.GetNumberOfSugar(ss).ToString();
        //                if (sugarWords[0].ToCharArray().Count() > 12)
        //                {
        //                    string text = char.ToUpper(ss[0]) + ss.Substring(1);
        //                    text = text.Insert(12, "- ");
        //                    sc.transform.Find("Name").GetComponent<Text>().text = text;

        //                }
        //                else sc.transform.Find("Name").GetComponent<Text>().text = char.ToUpper(ss[0]) + ss.Substring(1);


        //                var sci = sc.transform.Find("Image");
        //                sci.GetComponentInChildren<Text>().text = "";
        //                sci.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        //                sci.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        //                sci.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        //                sci.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
        //                sci.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 150);
        //                sci.GetComponent<RectTransform>().localScale = new Vector2(1.5f, 1.5f);

        //                sci.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + sc.name);
        //                //sci.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Monsters/" + s[familyIndex] + "/" + sc.name);
        //                //sci.gameObject.AddComponent<Button>().onClick.AddListener(() => summonSystem.GetComponent<SummonSystem>().PopupSugarInfoCardInSugarDex(sc.name, s[familyIndex]));
        //            }
        //        }
        //    }

        //}
        #endregion

        GameObject.Find("SugarDisk").GetComponent<SugarDisk>().allCollectedSugars = sugarDex.GetComponent<SugarDisk>().allCollectedSugars.Distinct().ToList();
        foundCount.GetComponent<Text>().text = "Found: " + sugarDex.GetComponent<SugarDisk>().allCollectedSugars.Count;
        totalCount.GetComponent<Text>().text = "Total: " + repository.Count;
        sugarDex.GetComponent<SugarDisk>().CloseSugarDisk();

        // CanvasScaler cs = canvas.GetComponent<CanvasScaler>();
        //cs.referenceResolution = new Vector2 ()
    }


    // Update is called once per frame
    void Update() {
#if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape) && sugarDex.GetComponent<SugarDisk>().sugarDexOpen == true)
        {
            sugarDex.GetComponent<SugarDisk>().CloseSugarDisk();
        }
#endif
    }

    public void ToggleOption() 
    {
        switch (toggleOption)
        {
            case "Sound":
                SetVibrate();
                break;
            case "Vibrate":
                SetMute();
                break;
            case "Mute":
                SetSound();
                break;
        }
    }

    public void SetSound()
    {
        toggleOption = "Sound";
        PlayerPrefs.SetString("ToggleOption", toggleOption);
        sound = true;
        vibrate = true;
        if (soundInitialized)
        {
            onSound.Play();
        }
        toggleButton.GetComponent<Image>().sprite = Sound;
    }

    public void SetVibrate()
    {
        toggleOption = "Vibrate";
        PlayerPrefs.SetString("ToggleOption", toggleOption);
        sound = false;
        vibrate = true;
#if UNITY_ANDROID || UNITY_IOS
        if (soundInitialized)
        {
            Handheld.Vibrate();
        }
#endif
        toggleButton.GetComponent<Image>().sprite = Vibrate;
    }

    public void SetMute()
    {
        toggleOption = "Mute";
        PlayerPrefs.SetString("ToggleOption", toggleOption);
        sound = false;
        vibrate = false;
        toggleButton.GetComponent<Image>().sprite = Mute;
    }

    public void AllTypeOfSugars(string ingredientFromDB, string bcv)
    {
        sugarDex.GetComponent<Button>().enabled = false;
        greenCartBtn.GetComponent<Button>().enabled = false;
        mainCam.GetComponent<SimpleDemo>().enabled = false;
        if(bcv == superCode)
        {
            ingredientFromDB = "";
        }

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
            
            //ingredientFromDB = Regex.Replace(ingredientFromDB, "[^a-zA-Z0-9_.,; ]+", "");
            ingredientFromDB = ingredientFromDB.Replace('.', ',').Replace(';', ',');
            List<string> dbIngredientList = ingredientFromDB.Split(',').ToList();
            dbIngredientList = dbIngredientList.ConvertAll(item => item.Trim());
            dbIngredientList.RemoveAt(0); // remove the upc/bcv number as we already have it
            string name = dbIngredientList[0]; // get the name of the product
            name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name); // make the first letter of every word uppcase
            dbIngredientList.RemoveAt(0); // remove the name from the sugar list
            if (bcv == superCode) dbIngredientList = repository;

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
                                um.IndicateController(info,"Notification", dropdownMenu.GetComponent<TMP_Dropdown>().options);
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

            if (scannedAddedSugars.Count == 0)
            {
                //add green cart code here
                //GreenCartController.Instance.PCAdd(bcv);
                //GreenCartController.Instance.PC.PCSave();
                um.simpleDemo.RequestAsync(bcv, name, String.Join(", ", scannedAddedSugars.ToArray()));
                //Change image of monster
                scannedAddedSugars.Add("No Added Sugar");
                CreateSugarMonster(scannedAddedSugars[currentNumMonster]);
                scanFrame.SetActive(false);
                
            }
            //Include added sugar
            else
            {
                string sugars = String.Join(", ", scannedAddedSugars.ToArray());
                sugars = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(sugars); // make the first letter of every word uppcase
                um.simpleDemo.RequestAsync(bcv, name, sugars);
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
        anim.GetComponent<Animator>().runtimeAnimatorController = animController;


        if (s == "Sugar")
        {
            canvas.transform.Find("Animation/Sugar Name").GetComponent<Text>().text = scannedAddedSugars[currentNumMonster];

            

            float animWaitCounter = 0f;
            while (animWaitCounter < 2f && wasSkipped == false) // wait for 2 seconds
            {
                if (sound && firstBadSound)
                {
                    badSound.Play();
                    firstBadSound = false;
                }
                if (Input.GetButtonDown("Fire1") || Input.touchCount > 0) break; // if mouse pressed or screen tapped, end timer
                yield return null;
                animWaitCounter += Time.deltaTime;
            }
            if (animWaitCounter < 2f) // if the previous animation was skipped
            {
                if (sound)
                {
                    sweepSound.Play();
                }
                wasSkipped = true;
                yield return new WaitForSeconds(.2f); // delay between rapid SugarCardToDex animations
            }
            firstBadSound = true;
            if (currentNumMonster + 1 == scannedAddedSugars.Count) // if last card
            {
                Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster])); // destroy stationary card
                wasSkipped = false; // reset wasSkipped for the next scan
            }
            else // if there are more cards
            {
                GameObject.Find(scannedAddedSugars[currentNumMonster]).GetComponentInChildren<Text>().text = scannedAddedSugars[currentNumMonster + 1]; // set atationary card to the next card
            }
            ChangeNextCardText();
            anim.GetComponent<Animator>().Play("SugarCardToDex");
            yield return new WaitForSeconds(1f);
            Destroy(anim);
        }
        else if (s == "NoAddedSugar")
        {
            if (sound)
            {
                goodSound.Play();
            }
            yield return new WaitForSeconds(2f);
            if (currentNumMonster + 1 == scannedAddedSugars.Count)
            {
                Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster]));
            }
            anim.GetComponent<Animator>().Play("GreenCard");
            yield return new WaitForSeconds(1f);
            Destroy(anim);
            ChangeNextCardText();
        }
        else if (s == "NotFound")
        {
            if (sound)
            {
                unknownSound.Play();
            }
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
                    tutorialMask.GetComponent<RectTransform>().anchoredPosition = new Vector2(2, -15);
                    tutorialMask.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 2000);

                    ts++;
                    PlayerPrefs.SetInt("TutorialStage", ts);
                }
            }
            GameObject.Destroy(GameObject.Find(scannedAddedSugars[currentNumMonster - 1]));
            scanFrame.SetActive(true);
            
            if (GameObject.Find("Magic Tree") == null && !greenCartGo.activeSelf)
            {
                sugarDex.GetComponent<Button>().enabled = true;
                greenCartBtn.GetComponent<Button>().enabled = true;
                mainCam.GetComponent<SimpleDemo>().enabled = true;
                mainCam.GetComponent<SimpleDemo>().Invoke("ClickStart", 3f); //wait for 3 seconds for next scan
            }
        }
        else
        {

            if (!sugarInWall.Contains(scannedAddedSugars[currentNumMonster].ToLower()))
            {

#if UNITY_ANDROID || UNITY_IOS
                if (vibrate) {
                    Handheld.Vibrate();
                }
#endif
                monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NewAddedSugar");
                redDot.gameObject.SetActive(true);
            }
            else monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CollectedAddedSugar");

            if (!sugarInWall.Contains(scannedAddedSugars[currentNumMonster].ToLower())) sugarInWall.Add(scannedAddedSugars[currentNumMonster].ToLower());



            monster.name = scannedAddedSugars[currentNumMonster];
            canvas.transform.Find(scannedAddedSugars[currentNumMonster] + "/Sugar Name").GetComponent<Text>().text = scannedAddedSugars[currentNumMonster];
            monster.transform.Find("SugarDesign").GetComponent<Image>().sprite = GetMonsterDesign(monster.name.ToLower());

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
                if (vibrate) {
                Handheld.Vibrate();
                }
#endif
                monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/NewAddedSugar");
                redDot.gameObject.SetActive(true);
            }
            else monster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/CollectedAddedSugar");
            GameObject.Find("Canvas").transform.Find(sugarName + "/Sugar Name").GetComponent<Text>().text = sugarName;

            monster.transform.Find("SugarDesign").GetComponent<Image>().sprite = GetMonsterDesign(sugarName);
            
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
    /// <summary>
    /// Checks if a monster has been found
    /// </summary>
    /// <param name="sugarName">Name of the monster to look for</param>
    /// <returns>true if the monster has been found before</returns>
    public bool MonsterFound(string sugarName) {
        return sugarInWall.Contains(sugarName.ToLower());
    }
    /// <summary>
    /// Returns the Sprite of a monster given the name (not including black shading as it's just the sprite not the image)
    /// </summary>
    /// <param name="sugarName">Name of the monster to find</param>
    /// <returns>Sprite of the correct monster</returns>
    public Sprite GetMonsterDesign(string sugarName)
    {
        return Resources.Load<Sprite>("Images/Monsters/" + sugarName);
    }
}
