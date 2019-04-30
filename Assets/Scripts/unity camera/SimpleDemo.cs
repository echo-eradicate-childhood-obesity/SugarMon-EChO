using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
public class SimpleDemo : MonoBehaviour
{

    private IScanner BarcodeScanner;
    public RawImage Image;

    private bool inDB;
    private static List<string> dbProductList = new List<string>();
    [HideInInspector]
    public int tutorialStage;

    [HideInInspector]
    public bool superBarCode = false;

    private bool isAndroid;

    private List<string> excludedCodeType = new List<string>() { "QR_CODE", "DATA_MATRIX", "AZTEC", "PDF_417" };

    // Disable Screen Rotation on that screen
    void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        tutorialStage = PlayerPrefs.GetInt("TutorialStage");
    }

    void Start()
    {
        if (tutorialStage == 0)
        {
            //first stage
            TutorialController.initMask();
            GameObject magicTree = GameObject.Find("Magic Tree"), tutorialMask = GameObject.Find("Tutorial Mask");
            magicTree.GetComponentInChildren<Text>().text = "Hi friend, I am the Magic Tree. I am here to help you grow healthier.";
            GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics = new List<string>() { "0-1", "0-2", "0-3" };
            tutorialMask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics[0]);
        }

        //Read new USDA sorted Database
#if UNITY_EDITOR
        TextAsset PerfactDatabase = (TextAsset)Resources.Load("USDA");
        string encodedContent = Encoding.UTF7.GetString(PerfactDatabase.bytes);
        dbProductList = encodedContent.Split(new char[] { '\n' }).ToList();
        dbProductList = dbProductList.ConvertAll(item => Regex.Replace(item, @",+", ","));
        dbProductList = dbProductList.ConvertAll(item => item.ToLower().Trim().Replace("\"*", "").Replace("[;.]", ",").TrimEnd(','));

#else
        Task.Run(()=>{
            TextAsset PerfactDatabase = (TextAsset)Resources.Load("USDA");
            string encodedContent = Encoding.UTF7.GetString(PerfactDatabase.bytes);
            dbProductList = encodedContent.Split(new char[] { '\n' }).ToList();
            dbProductList = dbProductList.ConvertAll(item => Regex.Replace(item, @",+", ","));
            dbProductList = dbProductList.ConvertAll(item => item.ToLower().Trim().Replace("\"*", "").Replace("[;.]", ",").TrimEnd(','));
        });
#endif
        // Create a basic scanner
        BarcodeScanner = new Scanner();
        BarcodeScanner.Camera.Play();

        // Display the camera texture through a RawImage
        BarcodeScanner.OnReady += (sender, arg) => {
            // Set Orientation & Texture
            Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
            Image.transform.localScale = BarcodeScanner.Camera.GetScale();
            Image.texture = BarcodeScanner.Camera.Texture;

            //Keep Image Aspect Ratio
            //var rect = Image.GetComponent<RectTransform>();
            //var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
            //rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        };

        // Track status of the scanner
        //BarcodeScanner.StatusChanged += (sender, arg) => {
        //  Debug.Log("Status: " + BarcodeScanner.Status);
        //};


        if (tutorialStage != 0) Invoke("ClickStart", 1f);

        isAndroid = false;
        //When on Android platform
#if UNITY_ANDROID
            isAndroid = true;
#endif
    }


    /// <summary>
    /// The Update method from unity need to be propagated to the scanner
    /// </summary>
    void Update()
    {
        if (BarcodeScanner == null)
        {
            return;
        }
        BarcodeScanner.Update();

        if (isAndroid&& Input.GetButtonDown("Cancel"))
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack",true);
        }
    }

#region UI Buttons

    public void ClickStart()
    {
        inDB = false;
        if (BarcodeScanner == null)
        {
            Log.Warning("No valid camera - Click Start");
            return;
        }

        // Start Scanning
        BarcodeScanner.Scan((barCodeType, barCodeValue) => {
            BarcodeScanner.Stop();
            
            if (this.GetComponent<TestController>().test) GameObject.Find("UPCNumber").GetComponent<Text>().text = barCodeValue;
            
            if (excludedCodeType.Any(barCodeType.Contains))  //need test
            {
                Invoke("ClickStart", 1f);
            }
            else
            {
                var i = SearchController.BinarySearch(dbProductList, long.Parse(barCodeValue), dbProductList.Count - 1, 0);

                bool test = GameObject.Find("Main Camera").GetComponent<TestController>().test;

                //test
                if (i != -1)
                {
                    inDB = true;

                    if (test == true && barCodeValue == "044000030414") superBarCode = true;
                    GameObject.Find("Canvas").GetComponent<FindAddedSugar>().AllTypeOfSugars(dbProductList[i].ToLower(), barCodeValue);
                }
                if (!inDB && GameObject.Find("Not Found") == null) GameObject.Find("Canvas").GetComponent<FindAddedSugar>().AllTypeOfSugars("Not Found", barCodeValue);
            }

        });

    }

    /// <summary>
    /// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
    /// Trying to stop the camera in OnDestroy provoke random crash on Android
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator StopCamera(Action callback)
    {
        // Stop Scanning
        Image = null;
        BarcodeScanner.Destroy();
        BarcodeScanner = null;

        // Wait a bit
        yield return new WaitForSeconds(0.1f);

        callback.Invoke();
    }


#endregion

    
}