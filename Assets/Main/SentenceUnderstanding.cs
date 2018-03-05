using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public static class SentenceUnderstanding {
    public delegate void result(
        string utterance,
        string commandName,
        Dictionary<string,SlotValue> slots
    );

    private const string API_KEY =
        "59522f317336726d514662495a62434170633768464f32636d31496161615a766431434a44767045707443";
    private const string API_URL =
        "https://api.apigw.smt.docomo.ne.jp/sentenceUnderstanding/v1/task?APIKEY=" + API_KEY;

    public static void Input(string utt, result callback, MonoBehaviour mb) {
        mb.StartCoroutine(InputCoroutine(utt, callback));
    }

    private static IEnumerator InputCoroutine(string utt, result callback) {
        var data = System.Text.Encoding.UTF8.GetBytes(
            Json.Serialize(new Dictionary<string, object>() {
                { "projectKey", "OSU" },
                { "appInfo", new Dictionary<string, object>(){{"appKey", "TAH_VR" }} },
                { "clientVer", "1.0.0" },
                { "language", "ja" },
                { "userUtterance", new Dictionary<string, object>(){{"utteranceText", utt}} }
            })
        );

        var headers = new Dictionary<string, string>() {
            {"Content-Type", "application/x-www-form-urlencoded"}
        };

        var www = new WWW(API_URL, data, headers);
        yield return www;

        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogError("www Error:" + www.error);
            yield break;
        }

        string response = System.Text.Encoding.UTF8.GetString(www.bytes);
        var dialogStatus = JsonNode.Parse(response)["dialogStatus"];

        var slots = new Dictionary<string, SlotValue>();
        if (dialogStatus.Get<Dictionary<string,object>>().ContainsKey("slotStatus")) {
            foreach(var slot in dialogStatus["slotStatus"]) {
                var sv = new SlotValue();
                sv.slotValue = slot["slotValue"].Get<string>();
                sv.valueType = slot["valueType"].Get<string>();
                slots.Add(slot["slotName"].Get<string>(), sv);
            }
        }

        callback(utt, dialogStatus["command"]["commandName"].Get<string>(), slots);
    }

    public class SlotValue {
        public string slotValue;
        public string valueType;
    }
}
