using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class GeoLocationModel
{
    public event Action OnErrorGetCountry;
    public event Action<string> OnGetCountry;

    private string URL_GET_IP = "https://ipinfo.io/json";

    public void GetUserCountry()
    {
        Coroutines.Start(GetIPInfo_Coroutine());
    }

    private IEnumerator GetIPInfo_Coroutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(URL_GET_IP);

        var operation = request.SendWebRequest();

        float timeOut = 3f;
        float startTime = Time.time;

        yield return new WaitUntil(() => operation.isDone || (Time.time - startTime) > timeOut);

        if (request.result != UnityWebRequest.Result.Success || !operation.isDone)
        {
            Debug.LogError("Error: " + request.result);
            OnErrorGetCountry?.Invoke();
        }
        else if (request.result == UnityWebRequest.Result.Success)
        {
            var jsonResult = request.downloadHandler.text;
            IPInfo ipInfo = JsonUtility.FromJson<IPInfo>(jsonResult);
            Debug.Log($"Country: {ipInfo.country}");
            OnGetCountry?.Invoke(ipInfo.country);
        }
    }
}

public class IPInfo
{
    public string ip;
    public string city;
    public string region;
    public string country;
    public string loc;
    public string org;
    public string postal;
    public string timezone;
    public string readme;
}
