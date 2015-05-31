using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    private int _coinScore;
    private int _points;

    public Text _pointsText;
    public Text _coinScoreText;
    public Text _levelDirectionsText;
    public string LevelNameToLoad;

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
        Application.LoadLevel(Application.loadedLevel);
    }

    void Start()
    {
        //Sanity checks

        if (_pointsText == null) Debug.LogError("PointsText hasn't been set to a Text element in your scene yet. We can't update screen text without this.");
        if (_coinScoreText == null) Debug.LogError("CoinScoreText hasn't been set to a Text element in your scene yet. We can't update coin counter text without this.");
        if (_levelDirectionsText == null) Debug.LogError("LevelDirectionsText hasn't been set to a Text element in your scene yet. We can't update screen dialog text without this.");

        if (string.IsNullOrEmpty(LevelNameToLoad))
        {
            Debug.LogError("No scene name has been set to load. Set this in the Editor on the PortalController component on the Portal");
        }


        if (PortalControllerFromCrypt == null)
        {
            Debug.LogError("There's no PortalController reference drag/dropped into the hierarchy window for the GameController. We can't enable the portal (crypt) without this reference being set.");
        }
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
        Application.LoadLevel(LevelNameToLoad);

    }

    public void IncrementCoinScore(int amount)
    {
        _coinScore += amount;
        _coinScoreText.text = _coinScore.ToString();
    }

    public void IncrementPoints(int amount)
    {
        _points += amount;
        _pointsText.text = _points.ToString() + " PTS";
    }

    public void ShowLevelMessage(string message)
    {
        _levelDirectionsText.text = message;

        //ensure we've enabled this textbox since it may be 
        //disabled in the scene view so as to not obstruct the view
        _levelDirectionsText.transform.gameObject.SetActive(true);
        //Make the text visible
        var color = _levelDirectionsText.color;
        color.a = 1;
        _levelDirectionsText.color = color;

        //Start fading it out
        _levelDirectionsText.CrossFadeAlpha(0, 5f, false);
    }
}
