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

    public IScanner BarcodeScanner;
    public RawImage Camera;
    public TextAsset LabelInsightDatabase;

    public string gkey;
    public IRequester grequester;
    public char delimiter; // what divides the two numbers in the database (;)

    private static List<string> dbProductList = new List<string>();
    [HideInInspector]
    public int tutorialStage;

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
        /*
        if (tutorialStage == 0)
        {
            //first stage
            TutorialController.initMask();
            GameObject magicTree = GameObject.Find("Magic Tree"), tutorialMask = GameObject.Find("Tutorial Mask");
            magicTree.GetComponentInChildren<Text>().text = "Hi friend, I am the Magic Tree. I am here to help you grow healthier.";
            GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics = new List<string>() { "0-1", "0-2", "0-3" };
            tutorialMask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Tutorial Masks/" + GameObject.Find("Tutorial Mask").GetComponent<TutorialController>().tutorialStagePics[0]);
        }*/

        //Read Label Insight Database
#if UNITY_EDITOR
        string encodedContent = Encoding.UTF7.GetString(LabelInsightDatabase.bytes);
        dbProductList = encodedContent.Split(new char[] { '\n' }).ToList();
        dbProductList = dbProductList.ConvertAll(item => Regex.Replace(item, @",+", ","));
        dbProductList = dbProductList.ConvertAll(item => item.ToLower().Trim().Replace("\"*", "").Replace("[;.]", ",").TrimEnd(','));

#else
        Task.Run(()=>{
            string encodedContent = Encoding.UTF7.GetString(LabelInsightDatabase.bytes);
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
            Camera.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
            Camera.transform.localScale = BarcodeScanner.Camera.GetScale();
            Camera.texture = BarcodeScanner.Camera.Texture;
            //Keep Image Aspect Ratio
            //var rect = Camera.GetComponent<RectTransform>();
            //var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
            //rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        };
        BarcodeScanner.StatusChanged += (sender, arg) =>
        {
#if UNITY_EDITOR
            Camera.GetComponent<AspectRatioFitter>().aspectRatio = (float)BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
#else
            Camera.GetComponent<AspectRatioFitter>().aspectRatio = (float)BarcodeScanner.Camera.Width / BarcodeScanner.Camera.Height;
#endif
        };
        // Track status of the scanner
        BarcodeScanner.StatusChanged += (sender, arg) =>
        {
            float w = BarcodeScanner.Camera.Width;
            float h = BarcodeScanner.Camera.Height;
            AspectRatioFitter a = Camera.GetComponent<AspectRatioFitter>();
            a.aspectRatio = w / h;
        };

        isAndroid = false;
        //When on Android platform
#if UNITY_ANDROID
            isAndroid = true;
#endif
        //create the requester. no need for await, use this for the build. it is faster 
        //when in editor use this. but use Async method in build will be faster
        //!not support Async load TextAsset in Editor
#if UNITY_EDITOR
        StartCoroutine("InitRequester");
#else
        StartAsync();
#endif
    }


    /// <summary>
    /// The Update method from unity need to be propagated to the scanner
    /// </summary>
    public void StartScan()
    {
        Invoke("ClickStart", 3f);
    }
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

    /// <summary>
    /// *Load UPC&NDB Lookup table into memory and sign to USDARequester
    /// *requester is the used to send request to usda
    /// *grequester is use to send request to Google using google map api.
    /// *!Warnning: Seems like Unity do not support TextAsset streaming using Task. Do not use this in unity editor
    /// </summary>
    /// <returns></returns>
    private async Task StartAsync() {
        await Task.Run(() => {
            grequester = new GoogleRequester(gkey);
        });
    }
    /// <summary>
    /// * This function is same as StartAsync()
    /// * But used only in Editor
    /// </summary>
    System.Collections.IEnumerator InitRequester() {
        grequester = new GoogleRequester(gkey);
        yield return null;
    }

    /// <summary>
    /// * Send Request To Google to get Information
    /// * Using normal method will cause halt
    /// </summary>
    /// <param name="bcv"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task RequestAsync(string bcv, string name, string sugars) {

        //wait 1 second to give the location service more time to get latlng info
#if !UNITY_EDITOR
        await Task.Run(() => {
            float i = 0;
            while (i < 2) {
                i += Time.deltaTime;
            }
        });
#endif  

        var pos = Input.location.lastData;
        //change the info to an format google api support
        var info = $@"latlng={pos.latitude.ToString()},{pos.longitude.ToString()}";
        var realpos = await grequester.SendRequest(info);
        GreenCartController.Instance.PCAdd(name, bcv, realpos, sugars);
        GreenCartController.Instance.PC.PCSave();
    }
    #region UI Buttons


    /// <summary>
    /// Start scanning
    /// </summary>
    public void ClickStart()
    {
        if (BarcodeScanner == null) {
            Log.Warning("No valid camera - Click Start");
        }
        else {
            // Start Scanning
            BarcodeScanner.Scan((barCodeType, barCodeValue) => {
                BarcodeScanner.Stop();

                //Check if in test mode
                if (this.GetComponent<TestController>().test)
                {
                    GameObject.Find("UPCNumber").GetComponent<Text>().text = barCodeValue;
                }
                    
                //Check scanned code type
                if (excludedCodeType.Any(barCodeType.Contains))
                    Invoke("ClickStart", 1f);
                else {

                    //Get the index of the product containing the scanned barcode
                    var i = SearchController.BinarySearch(dbProductList, long.Parse(barCodeValue), dbProductList.Count - 1, 0);

                    string ingredient = "";
                    if (i == -1)
                        ingredient = "Not Found";
                    else
                        ingredient = dbProductList[i].ToLower();

                    GameObject.Find("Canvas").GetComponent<FindAddedSugar>().AllTypeOfSugars(ingredient, barCodeValue);
                }

            });
        }

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
        Camera = null;
        BarcodeScanner.Destroy();
        BarcodeScanner = null;

        // Wait a bit
        yield return new WaitForSeconds(0.1f);

        callback.Invoke();
    }


#endregion

    
}