using System;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Rocket : MonoBehaviour
{

    private Rigidbody2D _rigidbody;
    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
#if UNITY_EDITOR
        if (Math.Abs(gameObject.transform.position.x) < .001f && Math.Abs(gameObject.transform.position.y) < .001f) EditorApplication.isPaused = true;
#endif
    }


    void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Math.Abs(gameObject.transform.position.x) < .01f)
        {
            Debug.Log("Invalid projectile position found with hash:" + gameObject.GetHashCode());
            EditorApplication.isPaused = true;
        }
#endif
        //This is used to determine if we apply velocity in -x or +x direction
        //depending on how we are instantiated. If we're instantiated with localScale.x==-1
        //then we need to move to the right
        var multiplerForRotation = -1;
        if (transform.localScale.x == -1)
        {
            multiplerForRotation = 1;
        }
        //set its velocity every frame until we hit something
        _rigidbody.velocity = new Vector2(multiplerForRotation*30, 0);
    }

   
}
