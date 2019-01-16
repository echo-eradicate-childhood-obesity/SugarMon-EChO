using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelfPlay : MonoBehaviour {

    private float holdTime;

    public float HoldTime
    {
        get { return holdTime; }
        private set { holdTime = value; }
    }

    private bool noTouch;

    public bool NoTouch
    {
        get { return noTouch; }
        set { noTouch = value; }
    }

    private float test;

    private void Awake()
    {
        holdTime = 2.0f;
        NoTouch = false;
    }

    //private void Start()
    //{
    //    holdTime = 2.0f;
    //    noTouch = false;
    //}
    private void OnEnable()
    {
        StartCoroutine(SelfAnimeCounter());
    }


    IEnumerator SelfAnimeCounter()
    {
        yield return new WaitForSeconds(holdTime);
        GameObject.Find("Canvas").GetComponent<FindAddedSugar>().Test();
        Debug.Log("anime");
        yield return null;
    }
}
