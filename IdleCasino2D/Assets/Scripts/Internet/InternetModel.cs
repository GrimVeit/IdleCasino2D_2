using System;
using System.Collections;
using UnityEngine;

public class InternetModel
{
    public event Action<string> OnGetStatusDescription;
    public event Action OnInternetAvailable;
    public event Action OnInternetUnvailable;

    public void StartCheckConnection()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Подключения к интернету нет");
            OnGetStatusDescription?.Invoke("Please check internet connection...");
            OnInternetUnvailable?.Invoke();
        }
        else
        {
            Debug.Log("Подключения к интернету есть");
            OnGetStatusDescription?.Invoke("Loading...");
            OnInternetAvailable?.Invoke();
        }
    }

    public void Dispose()
    {

    }
}
