using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour {

    public static SettingsScript instance;

    private bool hasSound = true;
    private bool hasVibration = true;

    private void Awake()
    {
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

    public void toggleSound() { hasSound = !hasSound; }
    public void toggleVibration() { hasVibration = !hasSound; }
    public void setSoundSettings(bool tof) { hasSound = tof; }
    public void setVirbrationSettings(bool tof) { hasVibration = tof; }

    public bool getSoundSettings(){ return hasSound;}
    public bool getVibrationSettings(){ return hasVibration; }
}
