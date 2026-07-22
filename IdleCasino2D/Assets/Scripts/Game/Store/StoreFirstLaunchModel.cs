using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreFirstLaunchModel
{
    public bool IsFirstLaunch => _isFirstLaunch;

    private bool _isFirstLaunch = true;

    private readonly string _key;

    public StoreFirstLaunchModel(string key)
    {
        _key = key;
    }

    public void Initialize()
    {
        _isFirstLaunch = PlayerPrefs.GetInt(_key, 1) == 1;
    }

    public void CompleteFirstLaunch()
    {
        _isFirstLaunch = false;
        Save();
    }

    public void Dispose()
    {
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetInt(_key, _isFirstLaunch ? 1 : 0);
        PlayerPrefs.Save();
    }
}
