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
    }


    //andridstudio側で取得した音声認識のデータが返ってくる関数
    //返ってきた文字列をおおまかに分類したものをunityちゃんのスクリプトに渡す
    public void onCallBackString(string str)
    {
        throw new Exception("ああああああ");
        SentenceUnderstanding.Input(str, script.onCompleteClassification, this);
    }

    public void onCallBackWeather(string str)
    {
        script.speakWeather(str);
    }
}