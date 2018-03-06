using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callUnityChan : MonoBehaviour
{

    private Vector3 position;
    private Vector3 target;
    private Animator animator;
    private bool pointer;
    private float time;
    private bool notMV;


    private void Start()
    {
        target.x = 0;
        target.y = 0;
        target.z = -1.64f;
        this.animator = GetComponent<Animator>();
        pointer = true;
        notMV = true;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pointer&&notMV==false) {
            time += Time.deltaTime;

            if (time >= 2)
            {
                notMV = true;
                this.animator.SetBool("Run_R", false);
            }
        }
        else if(pointer==false&&notMV==false)
        {
            Debug.Log("raycast");
            Ray ray = new Ray(Camera.main.transform.position,
                Camera.main.transform.rotation * Vector3.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Tube")
                {
                    this.animator.SetBool("Run_R", true);
                    position.x = hit.point.x;
                    position.y = 0;
                    position.z = hit.point.z;
                    this.transform.position = position;
                    this.transform.LookAt(target);

                }
            }
        }
    }

    public void exitPointer()
    {
        pointer = false;
        notMV = false;
        time = 0;
        Debug.Log("exitPointer");
    }

    public void enterPointer()
    {
        pointer = true;
        this.animator.SetBool("Run_R", false);
        Debug.Log("enterPointer");
    }
}
