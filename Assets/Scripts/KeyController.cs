using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour
{
    private GameController _gameController;
    // Use this for initialization
    void Start()
    {
        _gameController = GameController.GetGameControllerInScene();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _gameController.EnableExitPortal();
            Destroy(gameObject);
        }
    }
}
