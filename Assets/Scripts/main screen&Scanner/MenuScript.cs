/*
 * This file was created by Mark Botaish on May 14th, 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuScript : MonoBehaviour {

    /*
     * Changes the text of the button to loading...
     * <This function gets called from buttons in the main menu>
     */
    public void changeButtonText(Text text)
    {
        text.text = "Loading...";
    }

    /*
     * Loads a scene based on the scene name. If the name does not exist log an error
     * <This function gets called from buttons in the main menu>
     */
    public void ChangeScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogError("The <" + sceneName + "> scene could not be found! Make sure <" + sceneName + "> is added in the build settings.");
    }

    /*
     * Load a website based on a URL
     * <This function gets called from the button in the main menux>
     */
    public void loadWebsite()
    {
        Application.OpenURL("https://echoforgood.org/");
    }
}
