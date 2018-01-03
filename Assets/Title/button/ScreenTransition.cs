using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SceneLoad()
    {
		var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        
        jo.Call("startRecognition");

        SceneManager.LoadScene("MainScene");
    }
}
