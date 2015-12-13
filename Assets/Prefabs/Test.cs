using UnityEngine;
using System.Collections;

public class TestSpawn : MonoBehaviour {

    public GameObject Island;

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(CoRandomSpawn());
	}

    IEnumerator CoRandomSpawn()
    {
        Debug.Log("First Frame");
        yield return null;
        Debug.Log("Second Frame");
        yield return null;
        Debug.Log("Third Frame");

        while (true)
        {
            yield return new WaitForSeconds(5);
            Instantiate(Island, transform.position, Quaternion.identity);
        }
    }

}
