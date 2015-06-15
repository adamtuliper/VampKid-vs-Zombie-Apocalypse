using UnityEngine;
using System.Collections;

public class DestroyMeIn : MonoBehaviour
{

    public float Lifetime = 5f;

	void Start () {
        //Destroy us in this many seconds
	    Destroy(gameObject, Lifetime);
	}
}
