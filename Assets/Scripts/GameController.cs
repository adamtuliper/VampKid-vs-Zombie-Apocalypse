using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private int _coinScore;
    private int _points;

    public Text PointsText;
    public Text CoinScoreText;
    public Text LevelMessageText;
    public string LevelNameToLoad;
    //Countdown bar
    public Image LevelTimer;
    public int LevelLengthInSeconds = 30;

    public PortalController PortalControllerFromCrypt;
    public void PlayerDied()
    {
        ShowLevelMessage("You died! Restarting....");
        StartCoroutine(ReloadLevel());
    }

    /// <summary>
    /// Coroutines (those with IEnumerator) must be started via StartCoroutine()
    /// We can do things like pausee for a time in them. In this case, pause for two seconds
    /// and reload the current level
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(2);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        //Sanity checks

        if (PointsText == null) Debug.LogError("PointsText hasn't been set to a Text element in your scene yet. We can't update screen text without this.");
        if (CoinScoreText == null) Debug.LogError("CoinScoreText hasn't been set to a Text element in your scene yet. We can't update coin counter text without this.");
        if (LevelMessageText == null) Debug.LogError("LevelMessageText hasn't been set to a Text element in your scene yet. We can't update screen dialog text without this.");

        if (string.IsNullOrEmpty(LevelNameToLoad))
        {
            Debug.LogError("No scene name has been set to load. Set this in the Editor on the PortalController component on the Portal");
        }

        if (PortalControllerFromCrypt == null)
        {
            Debug.LogError("There's no PortalController reference drag/dropped into the hierarchy window for the GameController. We can't enable the portal (crypt) without this reference being set.");
        }

        if (LevelTimer == null)
        {
            Debug.LogError("There's no LevelTimer image bar defined, cannot 'count down' this level.");
        }

        //Start the countdown
        StartCoroutine(CoLevelTimer());
    }

    private IEnumerator CoLevelTimer()
    {

        float decrementAmount = 1f/LevelLengthInSeconds;

        //Since we're checking every .1 seconds and not every second need to reduce by factor of 10
        decrementAmount = decrementAmount/10;

        //
        // LevelLengthInSeconds > Time.timeSinceLevelLoad
        while (LevelTimer.fillAmount>0)
        {
            LevelTimer.fillAmount -= decrementAmount;

            //If we're down to 15% left, show green as red
            if (LevelTimer.fillAmount < .15f)
            {
                LevelTimer.color = Color.red;
            }
            //amount to decrement by each time
            yield return new WaitForSeconds(.1f);
        }

        //TIMES UP!
        ShowLevelMessage("Time is up! Restarting....");
        StartCoroutine(ReloadLevel());
}

//Enables the 'crypt' exit portal once we've picked up the key in a scene
public void EnableExitPortal()
    {

        //Find portal and enable it.
        PortalControllerFromCrypt.ActivatePortal();

    }
    internal static GameController GetGameControllerInScene()
    {
        var gc = GameObject.FindGameObjectWithTag("GameController");
        if (gc == null)
        {
            Debug.LogError("Could not find an object tagged GameController in your scene!");
            return null;
        }

        return gc.GetComponent<GameController>();
    }

    internal void LoadNextLevel()
    {

        //TODO: display a 'success' sign, loading scene, etc.
		SceneManager.LoadScene(LevelNameToLoad);
    }

    public void IncrementCoinScore(int amount)
    {
        _coinScore += amount;
        CoinScoreText.text = _coinScore.ToString();
    }

    public void IncrementPoints(int amount)
    {
        _points += amount;
        PointsText.text = _points.ToString() + " PTS";
    }

    public void ShowLevelMessage(string message)
    {
        LevelMessageText.text = message;

        //ensure we've enabled this textbox since it may be 
        //disabled in the scene view so as to not obstruct the view
        LevelMessageText.transform.gameObject.SetActive(true);
        //Make the text visible
        var color = LevelMessageText.color;
        color.a = 1;
        LevelMessageText.color = color;

        //Start fading it out
        LevelMessageText.CrossFadeAlpha(0, 5f, false);
    }
}
