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
        script.GetDialogContext().Talk(inputField.text, onReply);
		InitInputField();
	}

    private void onReply(string reply, string reply_yomi)
    {
        script.GetSpeechSynthesizer().speak(reply_yomi);
        Debug.Log("reply: " + reply);
        Debug.Log("reply_yomi: " + reply_yomi);
    }

    void InitInputField() {
        inputField.text = "";
        inputField.ActivateInputField();
    }
}
