using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    private AsyncOperation _async;

    [SerializeField]
    private Image _healthBarStatus = null;

    [SerializeField]
    private GameObject _healthBarRoot = null;

    public void LoadLevel1()
    {

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        _healthBarRoot.SetActive(true);

	    _async = SceneManager.LoadSceneAsync("Level1");
        while (!_async.isDone)
        {
            Debug.Log(_async.progress);
            _healthBarStatus.fillAmount = _async.progress;
            yield return null;
        }
        Debug.Log("Loading complete");
    }

}
