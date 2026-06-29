using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseModel
{
    public event Action<List<string>> OnGetCountries;
    public event Action OnErrorGetCountries;

    public event Action<string> OnGetLink;
    public event Action OnErrorGetLink;

    private const string BaseUrl = "https://soundclicker-cd631-default-rtdb.firebaseio.com/PublicData_IdleCasino";

    private List<string> _countries;
    private string _link;

    #region Public Methods

    public async void GetCountries()
    {
        try
        {
            string url = $"{BaseUrl}/Geo.json";

            string json = await Get(url);

            Debug.Log("RAW GEO JSON: " + json);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (dict == null)
            {
                Debug.LogError("Geo is null");
                OnErrorGetCountries?.Invoke();
                return;
            }

            foreach (var country in dict.Values)
            {
                Debug.Log("Country: " + country);
            }

            _countries = new List<string>(dict.Values);

            OnGetCountries?.Invoke(_countries);
        }
        catch (Exception e)
        {
            Debug.LogError("GetCountries error: " + e.Message);
            OnErrorGetCountries?.Invoke();
        }
    }

    public async void GetLink()
    {
        try
        {
            string url = $"{BaseUrl}/Link.json";

            string json = await Get(url);

            Debug.Log("RAW LINK JSON: " + json);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (dict == null)
            {
                Debug.LogError("Link is null");
                OnErrorGetLink?.Invoke();
                return;
            }

            if (dict.TryGetValue("Link", out string link))
            {
                Debug.Log("Link: " + link);

                _link = link;
                OnGetLink?.Invoke(_link);
            }
            else
            {
                Debug.LogError("Key 'Link' not found");
                OnErrorGetLink?.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("GetLink error: " + e.Message);
            OnErrorGetLink?.Invoke();
        }
    }

    #endregion

    #region HTTP

    //private async Task<string> Get(string url)
    //{
    //    using var client = new HttpClient();
    //    return await client.GetStringAsync(url);
    //}

    private async Task<string> Get(string url)
    {
        using var request = UnityWebRequest.Get(url);

        request.timeout = 5; // секунды

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"HTTP Error: {request.error}");
            return null;
        }

        return request.downloadHandler.text;
    }

    #endregion
}
