using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{

    public Image SplashImage;
    public string NextScene;
    public float TimeToFadeIn;
    public float TimeTillFadeOut;
    public float TimeToFadeOut;
    public float TimeTillNextScene;

    IEnumerator Start()
    {
        
        SplashImage.canvasRenderer.SetAlpha(0.1f);
        FadeIn();
        yield return new WaitForSeconds(TimeTillFadeOut);
        FadeOut();
        yield return new WaitForSeconds(TimeTillNextScene);
        SceneManager.LoadScene(NextScene);
    }

    void FadeIn()
    {
        SplashImage.CrossFadeAlpha(1.0f, TimeToFadeIn, false);
    }
    void FadeOut()
    {
        SplashImage.CrossFadeAlpha(0.0f, TimeToFadeOut, false);
    }
}