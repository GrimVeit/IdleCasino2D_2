using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView_Text : View, ITimerView, IIdentify
{
    [SerializeField] private string id;
    [SerializeField] private string textStart = "ROUND STARTS IN: ";
    [SerializeField] private TextMeshProUGUI textCount;

    public string GetID() => id;

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    public void ChangeTime(int sec)
    {
        textCount.text = $"{textStart}{sec}";
    }

    public void ActivateTimer()
    {

    }

    public void DeactivateTimer()
    {

    }
}
