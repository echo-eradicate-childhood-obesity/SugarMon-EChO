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
    public RawImage Image;
    public TextAsset PerfactDatabase;

    private bool inDB;
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
        string encodedContent = Encoding.UTF7.GetString(PerfactDatabase.bytes);
        dbProductList = encodedContent.Split(new char[] { '\n' }).ToList();
        dbProductList = dbProductList.ConvertAll(item => Regex.Replace(item, @",+", ","));
        dbProductList = dbProductList.ConvertAll(item => item.ToLower().Trim().Replace("\"*", "").Replace("[;.]", ",").TrimEnd(','));

#else
        Task.Run(()=>{
            string encodedContent = Encoding.UTF7.GetString(PerfactDatabase.bytes);
            dbProductList = encodedContent.Split(new char[] { '\n' }).ToList();
            dbProductList = dbProductList.ConvertAll(item => Regex.Replace(item, @",+", ","));
            dbProductList = dbProductList.ConvertAll(item => item.ToLower().Trim().Replace("\"*", "").Replace("[;.]", ",").TrimEnd(','));
        });
#endif
        // Create a basic scanner
        BarcodeScanner = new Scanner();
        //BarcodeScanner.Camera.Play();

        // Display the camera texture through a RawImage
        BarcodeScanner.OnReady += (sender, arg) => {
            // Set Orientation & Texture
            //Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
            //Image.transform.localScale = BarcodeScanner.Camera.GetScale();
            //Image.texture = BarcodeScanner.Camera.Texture;
            
            //Keep Image Aspect Ratio
            //var rect = Image.GetComponent<RectTransform>();
            //var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
            //rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        };
        BarcodeScanner.StatusChanged += (sender, arg) =>
        {
#if UNITY_EDITOR
            //Image.GetComponent<AspectRatioFitter>().aspectRatio = (float)BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
            //Image.GetComponent<AspectRatioFitter>().aspectRatio = (float)GoogleARCore.Frame.CameraImage.Texture.height / GoogleARCore.Frame.CameraImage.Texture.width;
#else
            //Image.GetComponent<AspectRatioFitter>().aspectRatio = (float)BarcodeScanner.Camera.Width / BarcodeScanner.Camera.Height;
            //Image.GetComponent<AspectRatioFitter>().aspectRatio = (float)GoogleARCore.Frame.CameraImage.Texture.width / GoogleARCore.Frame.CameraImage.Texture.height;
#endif
        };
        // Track status of the scanner
        //BarcodeScanner.StatusChanged += (sender, arg) => {
        //  Debug.Log("Status: " + BarcodeScanner.Status);
        //};

        isAndroid = false;
        //When on Android platform
#if UNITY_ANDROID
            isAndroid = true;
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
        Image.texture = GoogleARCore.Frame.CameraImage.Texture;
        try
        {
            UIManager.Instance.ImageText.GetComponent<Text>().text = $"{((Texture2D)transform.GetComponent<GoogleARCore.ARCoreBackgroundRenderer>().BackgroundMaterial.mainTexture).GetPixels32().Length}";
        }
        catch (Exception)
        {

        }
        UIManager.Instance.StatusText.GetComponent<Text>().text = $"{BarcodeScanner.Status}";
        //UIManager.Instance.ImageText.GetComponent<Text>().text = $"{((Texture2D)GoogleARCore.Frame.CameraImage.Texture).GetPixels32().Length}";
        //UIManager.Instance.ImageWHText.GetComponent<Text>().text = $"{GoogleARCore.Frame.CameraImage.Texture.width.ToString()} & {GoogleARCore.Frame.CameraImage.Texture.height.ToString()}";
        //Debug.Log($"width is {GoogleARCore.Frame.CameraImage.Texture.width} and height is {GoogleARCore.Frame.CameraImage.Texture.height}, total length is {((Texture2D)(GoogleARCore.Frame.CameraImage.Texture)).GetPixels32().Length}");
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
            
            if (excludedCodeType.Any(barCodeType.Contains))
            {
                Invoke("ClickStart", 1f);
            }
            else
            {
                var i = SearchController.BinarySearch(dbProductList, long.Parse(barCodeValue), dbProductList.Count - 1, 0);

                bool test = transform.GetComponent<TestController>().test;

                //test
                if (i != -1)
                {
                    inDB = true;

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