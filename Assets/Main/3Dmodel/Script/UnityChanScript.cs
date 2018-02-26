using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanScript : MonoBehaviour {


    private Animator animator;

    private SpeechSynthesizer synth;
    private DialogContext dc;

    private string VoiceString="";

	// Use this for initialization
	void Start () {
        this.animator = GetComponent<Animator>();
        synth = new SpeechSynthesizer(SpeechSynthesizer.Voice.maki, this);
        dc = new DialogContext(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (VoiceString=="こんにちは")
        {
            this.animator.SetBool("Pause1", true);
            if (this.animator.GetBool("Pause1"))
            {
                synth.speak("ただいまマイクのテスト中", pitch: 1.5);
            }
            VoiceString = "";
        }
        else
        {
            this.animator.SetBool("Pause1", false);
        }
        if (VoiceString == "じゃあね")
        {
            this.animator.SetBool("Pause2", true);
            if (this.animator.GetBool("Pause2"))
            {
                synth.speak("声の高さも自由自在", pitch: 1.0);
            }
            VoiceString = "";
        }
        else
        {
            this.animator.SetBool("Pause2", false);
        }
    }

    public DialogContext GetDialogContext() {
        return dc;
    }

    public SpeechSynthesizer GetSpeechSynthesizer() {
        return synth;
    }

    public void SetVoiceStr(string str)
    {
        this.VoiceString = str;
    }

}