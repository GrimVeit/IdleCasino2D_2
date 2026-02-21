using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class NicknameModel
{
    public event Action<string> OnChooseNickname;

    public event Action OnCorrectNickname;
    public event Action OnIncorrectNickname;
    public event Action<string> OnEnterRegisterLoginError;

    private readonly Regex mainRegex = new("^[a-zA-Z0-9._]*$");
    private readonly Regex invalidRegex = new(@"(\.{2,}|/{2,})");

    public string Nickname { get; private set; }

    private readonly string keyNickname;

    private ISoundProvider _soundProvider;

    public NicknameModel(string keyNickname, ISoundProvider soundProvider)
    {
        this.keyNickname = keyNickname;
        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        Nickname = PlayerPrefs.GetString(keyNickname, "ABCD123");
        OnChooseNickname?.Invoke(Nickname);
    }

    public void Dispose()
    {
        PlayerPrefs.SetString(keyNickname, Nickname);
    }

    public void ChangeNickname(string value)
    {
        //soundProvider.PlayOneShot("TextEnter");

        if (value.Length < 5)
        {
            OnEnterRegisterLoginError?.Invoke("Nickname must be at least 5 characters long");
            OnIncorrectNickname?.Invoke();
            return;
        }

        if (value.Length > 17)
        {
            OnEnterRegisterLoginError?.Invoke("Nickname must not exceed 17 characters");
            OnIncorrectNickname?.Invoke();
            return;
        }

        if (!mainRegex.IsMatch(value))
        {
            OnEnterRegisterLoginError?.Invoke("Nickname can only contain english letters, numbers, periods and slashes");
            OnIncorrectNickname?.Invoke();
            return;
        }

        if (invalidRegex.IsMatch(value))
        {
            OnEnterRegisterLoginError?.Invoke("Nickname cannot contain consencutive periods and slashes");
            OnIncorrectNickname?.Invoke();
            return;
        }

        if (value.EndsWith("."))
        {
            OnEnterRegisterLoginError?.Invoke("Nickname cannot end with a period");
            return;
        }

        Nickname = value;
        OnChooseNickname?.Invoke(Nickname);

        OnEnterRegisterLoginError?.Invoke("");
        OnCorrectNickname?.Invoke();
    }
}
