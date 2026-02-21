using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarModel
{
    public int CurrentIndexAvatar => _currentIndexAvatar;

    public event Action<int> OnSelectAvatar;
    public event Action<int> OnDeselectAvatar;

    private int _currentIndexAvatar;

    private readonly string keyAvatar;

    public AvatarModel(string keyAvatar)
    {
        this.keyAvatar = keyAvatar;
        _currentIndexAvatar = PlayerPrefs.GetInt(keyAvatar, 0);
    }

    public void Initialize()
    {
        OnSelectAvatar?.Invoke(_currentIndexAvatar);
    }

    public void Dispose()
    {
        PlayerPrefs.SetInt(keyAvatar, _currentIndexAvatar);
    }

    public void Select(int id)
    {
        if(_currentIndexAvatar == id) return;

        OnDeselectAvatar?.Invoke(_currentIndexAvatar);

        _currentIndexAvatar = id;
        OnSelectAvatar?.Invoke(id);
    }
}
