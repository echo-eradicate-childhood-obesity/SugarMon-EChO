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

public class SimpleDemo : MonoBehaviour
{
    private IScanner BarcodeScanner;
    public RawImage Image;
    private bool inDB;
    private static List<string> usdaList = new List<string>();

    // Disable Screen Rotation on that screen
    void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    void Start()
    {
        //Read USDA Database
        TextAsset usdatxt = (TextAsset)Resources.Load("NoDupeDatabase");
        string usdaContent = Encoding.UTF7.GetString(usdatxt.bytes);
        usdaList = usdaContent.Split(new char[] { '\n' }).ToList();
        usdaList = usdaList.ConvertAll(item => item.ToLower().Trim());

        // Create a basic scanner
        BarcodeScanner = new Scanner();
        BarcodeScanner.Camera.Play();

        // Display the camera texture through a RawImage
        BarcodeScanner.OnReady += (sender, arg) => {
            // Set Orientation & Texture
            Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
            Image.transform.localScale = BarcodeScanner.Camera.GetScale();
            Image.texture = BarcodeScanner.Camera.Texture;

            // Keep Image Aspect Ratio
            //var rect = Image.GetComponent<RectTransform>();
            //var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
            //rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        };
        
        // Track status of the scanner
        //BarcodeScanner.StatusChanged += (sender, arg) => {
        //  Debug.Log("Status: " + BarcodeScanner.Status);
        //};
        Invoke("ClickStart", 1f);


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
            barCodeValue = barCodeValue.Remove(0, 1);
            foreach (string p in usdaList)
            {
                if (p.Contains(barCodeValue))
                {
                    inDB = true;
                    GameObject.Find("Canvas").GetComponent<FindAddedSugar>().AllTypeOfSugars(p.ToLower());
                    break;
                }
            }
            if (!inDB && GameObject.Find("Not Found") == null) GameObject.Find("Canvas").GetComponent<FindAddedSugar>().AllTypeOfSugars("Not Found");

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