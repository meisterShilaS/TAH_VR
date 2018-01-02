using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanScript : MonoBehaviour {


    private Animator animator;

	// Use this for initialization
	void Start () {
        this.animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("1"))
        {
            this.animator.SetBool("Pause1", true);
        }
        else
        {
            this.animator.SetBool("Pause1", false);
        }
        if (Input.GetKey("2"))
        {
            this.animator.SetBool("Pause2", true);
        }
        else
        {
            this.animator.SetBool("Pause2", false);
        }
    }
}
