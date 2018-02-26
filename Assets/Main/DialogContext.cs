using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogContext {
    public enum Sex { Male, Woman }
    public enum Bloodtype { A, B, AB, O }
    public enum Constellations {
        牡羊, 牡牛, 双子, 蟹, 獅子, 乙女, 天秤, 蠍, 射手, 山羊, 水瓶, 魚
    }
    public enum Place {
        稚内, 旭川, 留萌, 網走, 北見, 紋別, 根室, 釧路, 帯広, 室蘭,
        浦河, 札幌, 岩見沢, 倶知安, 函館, 江差,
        青森, 弘前, 深浦, むつ, 八戸, 秋田, 横手, 鷹巣, 盛岡, 二戸,
        一関, 宮古, 大船渡, 山形, 米沢, 酒田, 新庄, 仙台, 古川, 石巻,
        白石, 福島, 郡山, 白河, 小名浜, 相馬, 若松, 田島,
        宇都宮, 大田原, 水戸, 土浦, 前橋, みなかみ, さいたま, 熊谷, 秩父, 東京,
        大島, 八丈島, 父島, 千葉, 銚子, 館山, 横浜, 小田原, 甲府, 河口湖,
        長野, 松本, 諏訪, 軽井沢, 飯田, 新潟, 津川, 長岡, 湯沢, 高田,
        相川,
        静岡, 網代, 石廊崎, 三島, 浜松, 御前崎, 富山, 伏木, 岐阜, 高山,
        名古屋, 豊橋, 福井, 大野, 敦賀, 金沢, 輪島,
        大津, 彦根, 津, 上野, 四日市, 尾鷲, 京都, 舞鶴, 奈良, 風屋,
        和歌山, 潮岬, 大阪, 神戸, 姫路, 洲本, 豊岡,
        鳥取, 米子, 岡山, 津山, 松江, 浜田, 西郷, 広島, 呉, 福山,
        庄原, 下関, 山口, 柳井, 萩, 高松, 徳島, 池田, 日和佐, 松山,
        新居浜, 宇和島, 高知, 室戸岬, 清水,
        福岡, 八幡, 飯塚, 久留米, 佐賀, 伊万里, 長崎, 佐世保, 厳原, 福江,
        大分, 中津, 日田, 佐伯, 熊本, 阿蘇乙姫, 牛深, 人吉, 宮崎, 油津,
        延岡, 都城, 高千穂, 鹿児島, 阿久根, 枕崎, 鹿屋, 種子島, 名瀬, 沖永良部,
        那覇, 名護, 久米島, 南大東島, 宮古島, 石垣島, 与那国島,
        海外
    }
    public enum Character { Default, 関西弁, 赤ちゃん }

    private const string API_KEY =
        "59522f317336726d514662495a62434170633768464f32636d31496161615a766431434a44767045707443";
    private const string API_URL =
        "https://api.apigw.smt.docomo.ne.jp/dialogue/v1/dialogue?APIKEY=" + API_KEY;

    private string context_m;
    private string mode_m;
    private MonoBehaviour mb_m;

    private string sex_m;
    private string bloodtype_m;
    private string birthdateY_m;
    private string birthdateM_m;
    private string birthdateD_m;
    private string age_m;
    private string constellations_m;
    private string place_m;
    private string character_m;

    public string nickname { get; set; }
    public string nickname_yomi { get; set; }
    public Sex? sex {
        get { return (Sex)(sex_m!=null? System.Enum.Parse(typeof(Sex), sex_m, true)) : null; }
        set { sex_m = value!=null? value.ToString() : null; }
    }
    public Bloodtype? bloodtype {
        get { return (Bloodtype)(bloodtype_m!=null?
                System.Enum.Parse(typeof(Bloodtype), bloodtype_m, true) : null); }
        set { bloodtype_m = value!=null? value.ToString() : null; }
    }
    public int? birthdateY {
        get { return birthdateY_m!=null? int.Parse(birthdateY_m) : (int?)null; }
        set {
            if(value < 1 || System.DateTime.Now.Year < value) {
                Debug.LogError("value 'birthdateY' range error");
            }
            birthdateY_m = value!=null? value.ToString() : null;
        }
    }
    public int? birthdateM {
        get { return birthdateM_m!=null? int.Parse(birthdateM_m) : (int?)null; }
        set {
            if(value < 1 || 12 < value) {
                Debug.LogError("value 'birthdateM' range error");
            }
            birthdateM_m = value!=null? value.ToString() : null;
        }
    }
    public int? birthdateD {
        get { return birthdateD_m!=null? int.Parse(birthdateD_m) : (int?)null; }
        set {
            if(value < 1 || 31 < value) {
                Debug.LogError("value 'birthdateD' range error");
            }
            birthdateD_m = value!=null? value.ToString() : null;
        }
    }
    public int? age {
        get { return age_m!=null? int.Parse(age_m) : (int?)null; }
        set {
            if(value < 1 || 999 < value) {
                Debug.LogError("value 'age' range error");
            }
            age_m = value!=null? value.ToString() : null;
        }
    }
    public Constellations? constellations {
        get { return (Constellations)(constellations_m!=null?
                System.Enum.Parse(typeof(Constellations), constellations_m, true) : null); }
        set { constellations_m = value!=null? value.ToString() : null; }
    }
    public Place? place {
        get { return (Place)(place_m!=null?
                System.Enum.Parse(typeof(Place), place_m, true) : null); }
        set { place_m = value!=null? value.ToString() : null; }
    }
    public Character character {
        get {
            if(character_m == "20") return Character.関西弁;
            else if(character_m == "30") return Character.赤ちゃん;
            else return Character.Default;
        }
        set {
            if(value == Character.関西弁) character_m = "20";
            else if(value == Character.赤ちゃん) character_m = "30";
            else character_m = null;
        }
    }

    public DialogContext(MonoBehaviour caller) {
        mb_m = caller;
    }

    public IEnumerable TalkCoroutine(string message) {
        var data = System.Text.Encoding.UTF8.GetBytes(GenerateJSON(message));

        var headers = new Dictionary<string, string>() {
            { "Content-Type"   , "application/json" }
        };

        var www = new WWW(API_URL, data, headers);
        yield return www;

        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogError("www Error:" + www.error);
            yield break;
        }

        string response = System.Text.Encoding.UTF8.GetString(www.bytes);
        ResponseJSON json = JsonUtility.FromJson<ResponseJSON>(response);
        mode_m = json.mode;
        context_m = json.context;

        Reply(json.utt, json.yomi);
    }

    private string GenerateJSON(string message) {
        string s = JSONObject("utt", message);
        if(context_m != null)           s += ',' + JSONObject("context", context_m);
        if(nickname != null)            s += ',' + JSONObject("nickname", nickname);
        if(nickname_yomi != null)       s += ',' + JSONObject("nickname_y", nickname_yomi);
        if(sex_m != null)               s += ',' + JSONObject("sex", sex_m);
        if(bloodtype_m != null)         s += ',' + JSONObject("bloodtype", bloodtype_m);
        if(birthdateY_m != null)        s += ',' + JSONObject("birthdateY", birthdateY_m);
        if(birthdateM_m != null)        s += ',' + JSONObject("birthdateM", birthdateM_m);
        if(birthdateD_m != null)        s += ',' + JSONObject("birthdateD", birthdateD_m);
        if(age_m != null)               s += ',' + JSONObject("age", age_m);
        if(constellations_m != null)    s += ',' + JSONObject("constellations", constellations_m);
        if(place_m != null)             s += ',' + JSONObject("place", place_m);
        if(mode_m != null)              s += ',' + JSONObject("mode", mode_m);
        if(character_m != null)         s += ',' + JSONObject("t", character_m);
        return '{' + s + '}';
    }

    private string JSONObject(string key, string value) {
        return '"' + key + "\":\"" + value + '"';
    }
}

[System.Serializable]
class ResponseJSON {
    public string utt;
    public string yomi;
    public string mode;
    public int da;
    public string context;
}