using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamImageApplyer : MonoBehaviour {
    Texture texture;
    public Material mat;
    SimpleDemo sd;
    void Start()
    {
        mat.SetTexture("_MainTex", texture);
        sd = this.GetComponent<SimpleDemo>();
    }

    void Update()
    {
        mat.SetTexture("_MainTex", texture);
        texture = sd.BarcodeScanner.Camera.Texture;
    }
    // Use this for initialization
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(texture, destination);
    }
}
