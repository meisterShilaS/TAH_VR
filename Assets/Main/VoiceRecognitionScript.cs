using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceRecognitionScript : MonoBehaviour
{

    private string voiceString;

    public GameObject unitychan;       //unitychanが入る変数

    private UnityChanScript script;     //unitychanのscriptが入る関数


    void Start()
    {
        unitychan = GameObject.Find("unitychan");               //Unityちゃんをオブジェクトの名前から取得して変数に格納する
        script = unitychan.GetComponent<UnityChanScript>();     //unitychanの中にあるUnityChanScriptを取得して変数に格納する
    }


    public void onCallBackString(string str)
    {
        voiceString = str;
        script.SetVoiceStr(voiceString);        //unitychanのスクリプトに音声認識文字列を渡す
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        context.Call("startRecognition");       //再び音声認識を開始

    }
}
