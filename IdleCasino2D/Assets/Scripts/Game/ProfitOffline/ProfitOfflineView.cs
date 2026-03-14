using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfitOfflineView : View
{
    [SerializeField] private TextMeshProUGUI textEarn;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private Button buttonEarn;


    public void Initialize()
    {
        Debug.Log("AUTHOR");

        buttonEarn.onClick.AddListener(ClickToCollect);
    }

    public void Dispose()
    {
        buttonEarn.onClick.RemoveListener(ClickToCollect);
    }

    public void SetEarn(int value, string time)
    {
        textEarn.text = $"+{value}$";
        textTime.text = time;
    }

    #region Output

    public event Action OnClickToCollect;

    private void ClickToCollect()
    {
        OnClickToCollect?.Invoke();
    }

    #endregion
}
