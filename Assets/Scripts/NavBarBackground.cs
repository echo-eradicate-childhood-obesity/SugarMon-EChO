using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavBarBackground : MonoBehaviour {

    private bool menuStatus;
    public GameObject menuBarBackgound;

    void Start()
    {
        menuStatus = true;
    }
    public void ToggleMenu()
    {
        if (menuStatus) OpenMenu();
        else CloseMenu();
    }
	private void OpenMenu()
    {
        menuStatus = false;
        menuBarBackgound.GetComponent<Animator>().Play("OpenMenu");
    }

    private void CloseMenu()
    {
        menuStatus = true;
        menuBarBackgound.GetComponent<Animator>().Play("CloseMenu");
    }
}
