using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanScript : MonoBehaviour {


    private Animator animator;

    private SpeechSynthesizer synth;
    private DialogContext dc;

    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject context;

    // Use this for initialization
    void Start () {
        this.animator = GetComponent<Animator>();
        synth = new SpeechSynthesizer(SpeechSynthesizer.Voice.maki, this);

        dc = new DialogContext(this);

        //androidstudio側のクラスを参照できるようにする
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }

	
	// Update is called once per frame
	void Update () {
        // 呼びかけと応答のルーチンはonCompleteClassificationに移動しました

        /* if (VoiceString=="こんにちは")
        {
            this.animator.SetBool("Pause1", true);
            if (this.animator.GetBool("Pause1"))
            {
                synth.speak("ただいまマイクのテスト中", pitch: 1.5);
            }
            VoiceString = "";
            context.Call("endMuteSound");
            context.Call("startRecognition");       //再び音声認識を開始
        }
        else if (VoiceString == "じゃあね")
        {
            this.animator.SetBool("Pause2", true);
            if (this.animator.GetBool("Pause2"))
            {
                synth.speak("声の高さも自由自在", pitch: 1.0);
            }
            VoiceString = "";
            context.Call("endMuteSound");
            context.Call("startRecognition");       //再び音声認識を開始
        }
        else
        {
            if (VoiceString != "") {
                dc.Talk(VoiceString, onReply);
                VoiceString = "";
                context.Call("endMuteSound");
                context.Call("startRecognition");
            }

            this.animator.SetBool("Pause1", false);
            this.animator.SetBool("Pause2", false);
        } */
    }

    // 発言の分類が完了したときに呼ばれる
    public void onCompleteClassification(
        string utterance,   // 発言文字列
        string commandName, // 発言が何らかの指令だったとき，指令のおおまかな分類
        Dictionary<string, SentenceUnderstanding.SlotValue> slots // 指令のパラメータ
    ){
        switch (commandName) {
            case "天気":  // 天気を教えてという指令だったとき
                if (slots["searchArea"].slotValue != "none" ||
                    slots["hereArround"].slotValue != "none")
                {
                    synth.speak("ごめんなさい、特定の場所の天気はわからないんです。");
                }
                else {
                    int day = 0;
                    string date = slots["date"].slotValue;
                    if (date == "今日") day = 1;
                    else if(date == "明日") day = 2;
                    else if(date == "明後日") day = 3;

                    context.Call("startSearchWeather", day);
                }
                break;
            
            default:    // その他のとき，雑談として扱う
                dc.Talk(utterance, onReply);
                break;
        }
        context.Call("endMuteSound");
        context.Call("startRecognition");   //再び音声認識を開始
    }

    // 雑談対話の結果が返ってきたとき呼ばれる
    private void onReply(string reply, string reply_yomi)
    {
        synth.speak(reply_yomi);
        Debug.Log("reply: " + reply);
        Debug.Log("reply_yomi: " + reply_yomi);
    }

    public void speakWeather(string str)
    {
        synth.speak(str);
    }
}
