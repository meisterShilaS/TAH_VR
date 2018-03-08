using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingUnityChan : MonoBehaviour
{

    private Vector3 position;
    private Vector3 target;
    private Animator animator;

    private int action=0;
    private float xPoint=0;
    private float zPoint=0;

    private float speed = 1f;
    private float radius=1.64f;
    private float startTime;
    private float startTheta;
    private bool lockOn = false;
    private float theta;

    private bool followingFlag = false;

    private bool looking=true;
    private float lookTime = 0;
    private float notLookTime = 0;

    void Start()
    {
        target.x = 0;
        target.y = 0;
        target.z = 0;
        this.animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (followingFlag)
        {
            action = 1;
            startTime = Time.time;
            lockOn = false;
            startTheta = searchTheta(this.transform.position);

            followingFlag = false;
        }


        //現在地までunityちゃんが来る処理
        if (action == 1&&lockOn==false)
        {
            this.animator.SetBool("Run_R", true);

            position.x= Mathf.Cos(Time.time-startTime+startTheta) * radius;
            position.y = 0;
            position.z = Mathf.Sin(Time.time-startTime+startTheta) * radius;
            this.transform.position = position;
            this.transform.LookAt(target);
        }
        else if(lockOn)
        {
            this.animator.SetBool("Run_R", false);
            action = 0;
        }

        /*
        //unityちゃんを見続けたときの処理
        if (looking)
        {
            lookTime+= Time.deltaTime;
            if (lookTime >= 5)
            {
                GetComponent<UnityChanScript>().GetSpeaker().speak("そんなじろじろみないで");
                lookTime = 0;
            }

        }
        //unityちゃんを見ていない時間が長いときの処理
        else
        {
            notLookTime+= Time.deltaTime;
            if (notLookTime >= 10)
            {
                GetComponent<UnityChanScript>().GetSpeaker().speak("なにしてるの？");
                notLookTime = 0;
            }
        }*/
    }

    public void follow() {
        followingFlag = true;
    }

    //ポインタがunityちゃんに乗った時の処理
    public void enterPointer()
    {
        Debug.Log("enter");
        lockOn = false;
        looking = true;
        notLookTime = 0;
    }
    
    //ポインタがunityちゃんから出たときの処理
    public void exitPointer()
    {
        Debug.Log("exit");
        lockOn = true;
        looking = false;
        lookTime = 0;
    }


    //この関数をうまく使えば追従する動きを作れる
    //今見ている視線の座標を取得する関数
    //x軸とz軸を入手できる
    //このクラスのxPointとzPointに取得データが入る
    public void getEyePoint()
    {
        Ray ray = new Ray(Camera.main.transform.position,
                Camera.main.transform.rotation * Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {


            if (hit.collider.gameObject.name == "Tube")
            {
                xPoint = hit.point.x;
                zPoint = hit.point.x;
            }
        }
    }


    //角度を求める関数
    //引数は求めるもののVector3
    //戻り値はラジアン
    //unityちゃんの位置に合わせて作ったから他の座標で使用する場合，コードを変えないといけない
    public float searchTheta(Vector3 vec)
    {
        float theta;

        if (vec.x == 0)
        {
            theta = 90*Mathf.Deg2Rad;
            if (vec.z < 0f)
            {
                theta = theta +180* Mathf.Deg2Rad;
            }
        }
        else
        {
            theta = Mathf.Atan(vec.z / vec.x);
            if (vec.x < 0f)
            {
                theta = theta + 180 * Mathf.Deg2Rad;
            }
        }

        Debug.Log(theta);

        return theta;
    }
}
