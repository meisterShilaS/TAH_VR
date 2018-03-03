using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class VoiceRecognitionScript : MonoBehaviour
{

    public GameObject unitychan;       //unitychanが入る変数
    private UnityChanScript script;     //unitychanのscriptが入る関数
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject context;
    public Text text;


    void Start()
    {
        //Unityちゃんをオブジェクトの名前から取得して変数に格納する
        unitychan = GameObject.Find("unitychan");
        //unitychanの中にあるUnityChanScriptを取得して変数に格納する
        script = unitychan.GetComponent<UnityChanScript>();

        //androidstudio側のクラスを参照できるようにする
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

    }


    //andridstudio側で取得した音声認識のデータが返ってくる関数
    //返ってきた文字列をunityちゃんのスクリプトに渡す
    public void onCallBackString(string str)
    {
        //天気の情報を求められた場合
        if (str == "今日の天気を教えて")
        {
            context.Call("startSearchWeather",0);       //現在の天気を調べるようにしてる
        }
        script.SetVoiceStr(str);        //unitychanのスクリプトに音声認識文字列を渡す
        context.Call("startRecognition");       //再び音声認識を開始

    }

    public void onCallBackWeather(string str)
    {
        text.text = str;
    }
}