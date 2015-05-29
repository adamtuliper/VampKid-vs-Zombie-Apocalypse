using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour
{

    private GameController _gameController;
    // Use this for initialization
    void Start()
    {
        //Find the GameController
        _gameController = GameController.GetGameControllerInScene();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _gameController.IncrementCoinScore(10);
            Destroy(gameObject);
        }
    }
}
