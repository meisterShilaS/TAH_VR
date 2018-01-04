using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanScript : MonoBehaviour {


    private Animator animator;

    private SpeechSynthesizer synth;

	// Use this for initialization
	void Start () {
        this.animator = GetComponent<Animator>();
        synth = new SpeechSynthesizer(SpeechSynthesizer.Voice.maki, this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("1"))
        {
            if(!this.animator.GetBool("Pause1")) {
                synth.speak(new SpeechSynthesizer.SpeechInfo("ただいまマイクのテスト中", pitch:1.5));
            }
            this.animator.SetBool("Pause1", true);
        }
        else
        {
            this.animator.SetBool("Pause1", false);
        }
        if (Input.GetKey("2"))
        {
            if(!this.animator.GetBool("Pause2")) {
                synth.speak(new SpeechSynthesizer.SpeechInfo("声の高さも自由自在", pitch:1.0));
            }
            this.animator.SetBool("Pause2", true);
        }
        else
        {
            this.animator.SetBool("Pause2", false);
        }
    }
}