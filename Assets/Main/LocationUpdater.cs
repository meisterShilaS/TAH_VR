using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationUpdater : MonoBehaviour {

    int maxWait = 120;
    private string latitude;
    private string longitude;
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject context;


    IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }
        Input.location.Start();
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            maxWait--;
            yield return new WaitForSeconds(1);
        }
        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }
        else
        {
            this.latitude = Input.location.lastData.latitude.ToString();
            this.longitude = Input.location.lastData.longitude.ToString();
        }
        Input.location.Stop();

        //androidstudio側のクラスを参照できるようにする
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        context.Call("getLocation", this.latitude, this.longitude);
    }

}
