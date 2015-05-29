using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    public void LoadLevel1()
    {
        Application.LoadLevel("Level1");
    }

    //TODO
    IEnumerator SpinAndFade()
    {
        yield return null;
    }
}
