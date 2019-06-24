using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour {


    public bool test;
    public GameObject upcNumText, resetButton, foodDexCounter;

    // Use this for initialization
    void Awake () {
        ToggleTestObject(test);
	}

    private void ToggleTestObject(bool b)
    {
        upcNumText.SetActive(b);
        resetButton.SetActive(b);
        foodDexCounter.SetActive(b);
    }
}
