using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public Image progressBar;
    public float timeUntilStart = 1.0f;
    public float timeDuration = 2.5f;
    
    public float fillRepeatRate = 0.25f;
    private float fillRepeatAmount;
    private bool first = true;

    // Use this for initialization
    void Start () {
        progressBar = GetComponent<Image>();
        progressBar.fillAmount = 0;
	}

    void Update()
    {
        if (first)
        {
            first = false;
            fillRepeatAmount = (float)(fillRepeatRate / timeDuration);
            InvokeRepeating("Fill", timeUntilStart, fillRepeatRate);
        }
        
    }

    // Update is called once per frame
    void Fill () {
        if (progressBar.fillAmount < 1)
        {
            progressBar.fillAmount += fillRepeatAmount;
        }
    }
}
