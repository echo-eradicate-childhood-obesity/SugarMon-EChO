using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScreenSizeHelp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        CanvasScaler cs = this.transform.GetComponent<CanvasScaler>();
        cs.referenceResolution = new Vector2(Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
