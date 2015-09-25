using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StatusTracker : MonoBehaviour {

    public Text Log;

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Log.text += DateTime.Now.ToString() + " OnAppFocus(true)\r\n";
        }
        else
        {
            Log.text += DateTime.Now.ToString() + " OnAppFocus(false)\r\n";
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Log.text += DateTime.Now.ToString() + " OnAppPause(true)\r\n";
        }
        else
        {
            Log.text += DateTime.Now.ToString() + " OnAppPause(false)\r\n";
        }
    }

}
