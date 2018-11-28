using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Linq;

public class API : MonoBehaviour
{
    #region Serializables
    [System.Serializable]
    public class Food
    {
        public List list;

    }
    [System.Serializable]
    public class List
    {
        public string q;
        public string sr;
        public string ds { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int total { get; set; }
        public string group { get; set; }
        public string sort { get; set; }
        public Items[] item;
    }
    [System.Serializable]
    public class FoodDesc
    {
        public Report report;

    }
    [System.Serializable]
    public class Report
    {
        public string sr;
        public Thefoods food;
    }
    [System.Serializable]
    public class Thefoods
    {
        public Ing ing;

    }

    [System.Serializable]
    public class Ing
    {
        public string desc;

    }


    [System.Serializable]
    public class Items
    {
        public int offset;
        public string group;
        public string name;
        public string ndbno;
        public string ds;
        public string manu;

    }
    #endregion

    //public Text upc;
    public string test;
    private static List<string> db = new List<string>();
    public string S_UPC;
    public string GTP;
    public string[] AddedSugar = new string[247];
    //036632008398  791083622813 036632008787
    private const string URL = "https://api.nal.usda.gov/ndb/search/?format=json&q=";
    private const string API_KEY = "kljZyOcE6L8Ml6Z5PJeNvYOcLib9C4NaQgEl4EBB";
    //public Text responseText;
    //public Text DeckText;
    public string jsonstring;
    int count = 0;
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp, i.e.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sugarmon-5e94f.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;


        //Read database
            //Remove product name in parentheses after added sugar names
        //Debug.Log(db[0]);
        //responseText.text = "Enter UPC Number";

        //print(responseText.text);


    }
    public void Request(string S_UPC)
    {

        //S_UPC = upc.text;
        Debug.Log(S_UPC);
        //S_UPC = "791083622813";
        WWW request = new WWW(URL + S_UPC + "&" + "api_key=" + API_KEY);
        //Debug.Log(URL + S_UPC + "&" + "api_key=" + API_KEY);
        //StartCoroutine(OnResponse(request));
        Debug.Log(StartCoroutine(OnResponse(request)));
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }


    private IEnumerator OnResponse(WWW req)
    {

        yield return req;

        string s = "";
        //if (count == 0)
        //{
        //    GTP = req.text;
        //    S_UPC = "00" + S_UPC;
        //    WWW request = new WWW(URL + S_UPC + "&" + "api_key=" + API_KEY);
        //    //StartCoroutine(OnResponse(request));
        //    count++;
        //    //yield return null;
        //}

        //Debug.Log(req.text);
        //if (GTP.Length == req.text.Length)
        //{
            jsonstring = req.text;

            Food food = JsonUtility.FromJson<Food>(jsonstring);
            string ndbno = food.list.item[0].ndbno; //food.list.item.ndbno.ToString();
        if (ndbno == "45148363") {
            GTP = req.text;
            S_UPC = "00" + S_UPC;
            WWW request = new WWW(URL + S_UPC + "&" + "api_key=" + API_KEY);
            StartCoroutine(OnResponse(request));
        }
        else {
            WWW newrequest = new WWW("https://api.nal.usda.gov/ndb/reports/?ndbno=" + ndbno + "&type=f&format=json&api_key=MdQT6uuqMt4epBN9IAGOO1qSPH9GhMmIdJLwVlNP");
            //callback(StartCoroutine(OnnewResponse(newrequest)));
            StartCoroutine(OnnewResponse(newrequest, (status) => {
                Debug.Log(status);
            }));
            
        }
        //}
        //else if (GTP.Length < req.text.Length)
        //{
        //    jsonstring = req.text;

        //    Food food = JsonUtility.FromJson<Food>(jsonstring);

            
        //    string ndbno = food.list.item[0].ndbno;
        //    WWW newrequest = new WWW("https://api.nal.usda.gov/ndb/reports/?ndbno=" + ndbno + "&type=f&format=json&api_key=MdQT6uuqMt4epBN9IAGOO1qSPH9GhMmIdJLwVlNP");
        //    StartCoroutine(OnnewResponse(newrequest));
        //}
        //else
        //{
        //    //responseText.text = "UPC not Found";
        //    print("UPC not Found");
        //    //DeckText.text = "N/A";
        //    //print(DeckText.text);
        //}



    }
    private IEnumerator OnnewResponse(WWW req, System.Action<string> callback)
    {

        yield return req;

        jsonstring = req.text;
        FoodDesc thefood = JsonUtility.FromJson<FoodDesc>(jsonstring);
        callback(thefood.report.food.ing.desc);
        //thefood.report.food.ing.desc;
        //yield return thefood.report.food.ing.desc;
    }
}
