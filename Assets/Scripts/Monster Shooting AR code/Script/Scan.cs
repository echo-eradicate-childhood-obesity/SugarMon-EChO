using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scan : MonoBehaviour {

    private bool cameraAvailable;
    private WebCamTexture backCamera;

    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fitter;

	// Use this for initialization
	private void Start () {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No camera detected");
            cameraAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                backCamera = new WebCamTexture(devices[i].name, Screen.width,Screen.height);
                
            }
        }

        if(backCamera == null)
        {
            
            Debug.Log("Unable to find back camera");
            return;
        }

        backCamera.Play();
        background.texture = backCamera;

        cameraAvailable = true;
	}
	
	// Update is called once per frame
    private void Update () {
        if(!cameraAvailable) 
        {
            return;
        }

        //Fit camera when device rotates

        float ratio = (float)backCamera.width / (float)backCamera.height;
        fitter.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f: 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
	}
}
