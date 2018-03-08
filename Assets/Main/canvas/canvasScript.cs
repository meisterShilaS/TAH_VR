using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasScript : MonoBehaviour {


    private Vector3 target;


	// Use this for initialization
	void Start () {

        target.x = 0;
        target.y = 0;
        target.z = 0;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(target);
    }
}
