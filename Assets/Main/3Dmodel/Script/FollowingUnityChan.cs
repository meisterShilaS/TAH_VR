using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingUnityChan : MonoBehaviour
{

    private Vector3 position;
    private Vector3 target;
    private Animator animator;
    private int putKey;


    private void Start()
    {
        target.x = 0;
        target.y = 0;
        target.z = -1.64f;
        this.animator = GetComponent<Animator>();
        putKey = 0;             //0:押してない||bを押したとき　1:aを押したとき
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            putKey = 1;
        }

        if (Input.GetKeyDown("b"))
        {
            putKey = 0;
        }


        if(putKey==1)
        {
            Ray ray = new Ray(Camera.main.transform.position,
                Camera.main.transform.rotation * Vector3.forward);

            this.animator.SetBool("Run_R", true);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Tube")
                {
                    position.x = hit.point.x;
                    position.y = 0;
                    position.z = hit.point.z;
                    this.transform.position = position;
                    this.transform.LookAt(target);

                }
            }
        }
        else
        {
            this.animator.SetBool("Run_R", false);
        }
    }

    public void exitPointer()
    {
        
    }

    public void enterPointer()
    {
        
    }
}
