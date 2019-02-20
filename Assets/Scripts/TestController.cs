using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour {


    public bool test;
    public GameObject upcNumText, resetButton;
    // Use this for initialization
    void Update () {
        if (test)
        {
            upcNumText.SetActive(true);
            resetButton.SetActive(true);
        }
        else
        {
            upcNumText.SetActive(false);
            resetButton.SetActive(false);
        }
	}
}
