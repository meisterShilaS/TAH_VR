using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour {
    private InputField inputField;
    private UnityChanScript script;

	// Use this for initialization
	void Start () {
		inputField = GetComponent<InputField>();
        InitInputField();

        script = GameObject.Find("unitychan").GetComponent<UnityChanScript>();
	}
	
	// Update is called once per frame
	public void onEnter () {
        script.SetVoiceStr(inputField.text);
        InitInputField();
	}

    void InitInputField() {
        inputField.text = "";
        inputField.ActivateInputField();
    }
}
