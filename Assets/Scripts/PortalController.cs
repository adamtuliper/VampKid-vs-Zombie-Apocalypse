using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour
{
    private GameController _gameController;
    private bool _portalActivated;

    [SerializeField]
    private GameObject _lock = null;

    [SerializeField]
    private GameObject _closedDoor = null;

    [SerializeField]
    private GameObject _openDoor = null;

    //Have we shown the user a helper message when they move over the portal?
    private bool _displayedHelperMessage;

    // Use this for initialization
    void Start()
    {
        _gameController = GameController.GetGameControllerInScene();

        //Initialize our game object states
        _lock.SetActive(true);
        _closedDoor.SetActive(true);
        _openDoor.SetActive(false);
    }

    public void ActivatePortal()
    {
        //enable this portal so we can leave this level
        _portalActivated = true;
        _lock.SetActive(false);
        _closedDoor.SetActive(false);
        _openDoor.SetActive(true);
    }

    /// <summary>
    /// This will only get called when IsTrigger =true on the collider 
    /// and the component is enabled. We disable it when the scene starts
    /// because we must find the key first.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_portalActivated)
            {
                //Ask the game controller to load the next level up.
                _gameController.LoadNextLevel();
            }
            else if (!_displayedHelperMessage)
            {
                //portal isn't activated yet! need to find the key
                _displayedHelperMessage = true;
                _gameController.ShowLevelMessage("Find the key to activate the portal!");
                //Show helper message
            }
        }
    }
}
