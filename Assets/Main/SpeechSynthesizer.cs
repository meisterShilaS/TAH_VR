using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  ☆ 参考にしたサイト一覧 ☆
//  Unityでdocomoの音声合成APIを使用する方法
//      https://qiita.com/kanatano_mirai/items/677fde8589a4d810329a
//  ドコモの音声合成APIにしゃべってもらおうと思ったけど...
//      https://qiita.com/tkinjo1/items/a1cf73a471f06ab3ff65
//  Unityのコルーチン機能を使う 
//      https://cfm-art.sakura.ne.jp/sys/archives/419
//  【Unity】WWWクラスを利用して、JSONを読み込む
//      http://albatrus.com/main/unity/7510
//  音声合成リファレンス
//      https://dev.smt.docomo.ne.jp/?p=docs.api.page&api_name=text_to_speech&p_name=api_1
//  Unity - Scripting API: AudioClip
//      https://docs.unity3d.com/ScriptReference/AudioClip.html

// 少しクラス大きくしすぎた...
// 音声データの変換のとこ別のクラスに分けてもいいかも？
// あとできれば一つのAPI専用じゃなくて別のAPIでも動くように汎用性をもたせたい
public class SpeechSynthesizer {
    // 声の種類
    public enum Voice {
        nozomi, seiji, akari, anzu, hiroshi, kaho,
        koutarou, maki, nanako, osamu, sumire
    };

    // APIに必要なキー
    private const string API_KEY =
        "59522f317336726d514662495a62434170633768464f32636d31496161615a766431434a44767045707443";

    private const string API_URL =
        "https://api.apigw.smt.docomo.ne.jp/aiTalk/v1/textToSpeech?APIKEY=" + API_KEY;
    
    // とりあえず16bit big endianで決め打ち
    private const int BIT_DEPTH       = 16; // ビット深度
    private const int BYTES_PER_FRAME = 2;  // 1フレームあたりのバイト数

    // APIから取得する音声データのフォーマット
    private const int SAMPLE_RATE = 16000;  // サンプル周波数
    private const int N_CHANNELS  = 1;      // チャネル数

    // 取得する音声データの量子化された振幅の最大絶対値+1
    // = |-2^15|+1 = 32769
    private const float MAX_AMP = 32769;

    private readonly string voiceName; // 声の種類の名前
    
    private readonly MonoBehaviour mb;
    private readonly AudioSource audioSource; // 再生用のAudioSource

    private byte[] rawSoundData; // APIから取得した音声データ
    
    public SpeechSynthesizer(Voice voice, MonoBehaviour caller) {
        voiceName = voice.ToString();
        mb = caller;
        audioSource = mb.gameObject.AddComponent<AudioSource>();
    }

    public class SpeechInfo {
        // ↓APIの仕様
        // https://dev.smt.docomo.ne.jp/?p=docs.api.page&api_name=text_to_speech&p_name=api_1
        // 実数値は範囲が決まってるので一応コンストラクタでエラーチェックしてます
        // 小数点第二位まで指定可能なので余分は切捨してます(1.2345なら45は切捨）
        // まだパラメータあんま弄ってないのでいろいろ試してみてください
        // このクラスは単なるパラメータ保存用の構造体みたいなもんで
        // 実際のサーバに送るデータはgenerateSSMLで作ってます
        public readonly string text;    // 喋らせる内容
        public readonly double pitch;   // 声の高さ(0.50～2.00)
        public readonly double range;   // 声の高さが変化する範囲？だと思う(0.00～2.00)
        public readonly double rate;    // 読み上げる速さ(0.50～4.00)
        public readonly double volume;  // 音量(0.00～2.00)

        public SpeechInfo(  string text,
                            double pitch  = 1.0,
                            double range  = 1.0,
                            double rate   = 1.0,
                            double volume = 1.0 )
        {
            if(!(
                0.50 <= pitch  && pitch  <= 2.00 &&
                0.00 <= range  && range  <= 2.00 &&
                0.50 <= rate   && rate   <= 4.00 &&
                0.00 <= volume && volume <= 2.00
            )) Debug.LogError("value range error");

            this.text   = text;
            this.pitch  = pitch;
            this.range  = range;
            this.rate   = rate;
            this.volume = volume;
        }
    };

    public void speak(SpeechInfo si) {
        mb.StartCoroutine(speakCoroutine(si));
    }

    private IEnumerator speakCoroutine(SpeechInfo si) {
        yield return getRawSoundData(si);
        audioSource.clip = convertAudioClip();
        audioSource.Play();
    }

    // APIのサーバに送るSSML(音声合成用の記述言語)を生成
    private string generateSSML(SpeechInfo si) {
        return  "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
            +       "<speak version=\"1.1\">"
            +           "<voice name=\"" + voiceName + "\">"
            +               "<prosody pitch=\""  + si.pitch .ToString("0.00") + "\">"
            +                "<prosody range=\""  + si.range .ToString("0.00") + "\">"
            +                 "<prosody rate=\""   + si.rate  .ToString("0.00") + "\">"
            +                  "<prosody volume=\"" + si.volume.ToString("0.00") + "\">"
            +                   si.text
            +                  "</prosody>"
            +                 "</prosody>"
            +                "</prosody>"
            +               "</prosody>"
            +           "</voice>"
            +       "</speak>"  ;
    }

    // APIのサーバから音声データを取得するコルーチン
    private IEnumerator getRawSoundData(SpeechInfo si) {
        var data = System.Text.Encoding.UTF8.GetBytes(generateSSML(si));

        var headers = new Dictionary<string, string>() {
            { "Content-Type"   , "application/ssml+xml" },
            { "Accept"         , "audio/L16"            }
        };

        var www = new WWW(API_URL, data, headers);
        yield return www;

        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogError("www Error:" + www.error);
            yield break;
        }

        rawSoundData = www.bytes;
    }

    // 取得した音声データはそのままではUnityで扱えないので
    // rawSoundDataをUnityで扱えるAudioClipに変換するメソッド
    private AudioClip convertAudioClip() {
        int lengthSamples = rawSoundData.Length / N_CHANNELS;

        AudioClip ac = AudioClip.Create("SpeechAudio",
            lengthSamples, N_CHANNELS, SAMPLE_RATE, false);

        float[] data = new float[rawSoundData.Length / BYTES_PER_FRAME];
        unchecked {
            for (int i = 0; i < data.Length; i++) {
                // 16bit big endian前提でfloatに変換
                // 値の範囲は 変換前 -32768 <= x <= +32767
                //            変換後 -1.0   <  x <  +1.0
                data[i] = (((sbyte)rawSoundData[i*2] << 8) + rawSoundData[i*2+1]) / MAX_AMP;
            }
        }
        ac.SetData(data, 0);

        return ac;
    }
}
