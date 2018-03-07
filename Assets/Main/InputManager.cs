using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour {
    private InputField inputField;
    private VoiceRecognitionScript script;

	// Use this for initialization
	void Start () {
		inputField = GetComponent<InputField>();
        InitInputField();

        script = GameObject.Find("VoiceRecognitionObject").GetComponent<VoiceRecognitionScript>();
	}
	
	// Update is called once per frame
	public void onEnter () {
        script.onCallBackString(inputField.text);
        InitInputField();
	}

    public void onClick() {
        GameObject.Find("VoiceRecognitionObject").GetComponent<VoiceRecognitionScript>().onCallBackString("こっちこっち");
    }

    void InitInputField() {
        inputField.text = "";
        inputField.ActivateInputField();
    }
}
