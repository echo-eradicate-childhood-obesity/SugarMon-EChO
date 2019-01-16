using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SplashScreen : MonoBehaviour
{

    public Image SplashImage;
    public Text SplashText;
    public string NextScene;
    public float TimeToFadeIn;
    public float TimeTillFadeOut;
    public float TimeToFadeOut;
    public float TimeTillNextScene;

    IEnumerator Start()
    {
        SplashImage.canvasRenderer.SetAlpha(0.1f);
        SplashText.canvasRenderer.SetAlpha(0.1f);
        FadeIn();
        yield return new WaitForSeconds(TimeTillFadeOut);
        FadeOut();
        yield return new WaitForSeconds(TimeTillNextScene);

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        //if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        //{
        //    throw new Exception("This Webcam library can't work without the webcam authorization");
        //}
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            StopAllCoroutines();
            SceneManager.LoadScene(NextScene);
            yield return null;
        }
    }

    //void Start()
    //{
    //    StartCoroutine("Loader");

    //}


    void FadeIn()
    {
        SplashImage.CrossFadeAlpha(1.0f, TimeToFadeIn, false);
        SplashText.CrossFadeAlpha(1.0f, TimeToFadeIn, false);

    }
    void FadeOut()
    {
        SplashImage.CrossFadeAlpha(0.0f, TimeToFadeOut, false);
        SplashText.CrossFadeAlpha(0.0f, TimeToFadeOut, false);
    }


    //IEnumerator Loader()
    //{
    //    SplashImage.canvasRenderer.SetAlpha(0.1f);
    //    SplashText.canvasRenderer.SetAlpha(0.1f);

    //    //SceneManager.LoadScene(NextScene, LoadSceneMode.Additive);
    //    //SceneManager.SetActiveScene(SceneManager.GetSceneByName("SplashScreen"));
    //    FadeIn();
    //    yield return new WaitForSeconds(TimeTillFadeOut);

    //    FadeOut();
    //    yield return new WaitForSeconds(TimeTillNextScene);

    //    yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    //    //if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
    //    //{
    //    //    throw new Exception("This Webcam library can't work without the webcam authorization");
    //    //}

    //    if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    //    {
    //        StopAllCoroutines();
    //        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    //        //SceneManager.UnloadSceneAsync("SplashScreen");
    //        SceneManager.LoadScene(NextScene);
    //        yield return null;
    //    }
    //}
}