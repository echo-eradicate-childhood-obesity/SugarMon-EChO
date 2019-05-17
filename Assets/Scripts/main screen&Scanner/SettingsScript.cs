/*
 * This file was created by Mark Botaish on May 15th, 2019
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour {

    public static SettingsScript instance;

    private bool hasSound = true;
    private bool hasVibration = true;

    private void Awake()
    {
        //Singleton 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Change settings for sound and vibrations
    public void toggleSound() { hasSound = !hasSound; }
    public void toggleVibration() { hasVibration = !hasSound; }
    public void setSoundSettings(bool tof) { hasSound = tof; }
    public void setVirbrationSettings(bool tof) { hasVibration = tof; }

    //Get the settings for the sound and vibration settings
    public bool getSoundSettings(){ return hasSound;}
    public bool getVibrationSettings(){ return hasVibration; }

    //Go to a different scene
    public void BackToMainMenu() { SceneManager.LoadScene("Menu"); }


}
