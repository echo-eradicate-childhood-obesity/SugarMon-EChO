using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScreenSizeHelp : MonoBehaviour {

    public Text t;

	// Use this for initialization
	void Start () {
        //t.text = "Screen Width is " + Screen.width + ", Screen Height is " + Screen.height;
        CanvasScaler cs = this.transform.GetComponent<CanvasScaler>();
        cs.referenceResolution = new Vector2(Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
