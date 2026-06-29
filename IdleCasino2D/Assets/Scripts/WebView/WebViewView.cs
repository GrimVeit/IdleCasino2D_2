using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WebViewView : View, IIdentify
{
    [SerializeField] private string id;
    [SerializeField] private UniWebView uniWebView;
    [SerializeField] private TextMeshProUGUI textLoading;

    public event Action<UniWebView, int, string> OnError;
    public event Action<UniWebView, string> OnStart;
    public event Action<UniWebView, string> OnFinish;
    public event Action OnClosePage;

    //public event Action OnHide_Action;

    private IEnumerator checkRotationCoroutine;

    public string GetID() => id;

    public void Initialize()
    {
        ActivateEvents();

        uniWebView.SetShowToolbarNavigationButtons(false);
        uniWebView.SetShowToolbar(false);
    }

    private void ActivateEvents()
    {
        uniWebView.OnPageStarted += OnPageStarted;
        uniWebView.OnPageFinished += OnPageFinished;
        uniWebView.OnShouldClose += OnShouldClose;
        uniWebView.OnPageErrorReceived += OnPageErrorReceived;
    }

    private void DeactivateEvents()
    {
        uniWebView.OnPageFinished -= OnPageFinished;
        uniWebView.OnShouldClose -= OnShouldClose;
        uniWebView.OnPageErrorReceived -= OnPageErrorReceived;
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    public void OnLoad(string URL)
    {
        if(uniWebView == null)
        {
            uniWebView = gameObject.AddComponent<UniWebView>();
            uniWebView.SetShowToolbar(false);
            uniWebView.SetShowToolbarNavigationButtons(false);
            SetFullScreen();

            ActivateEvents();

        }

        uniWebView.Load(URL);
    }

    private void SetFullScreen()
    {
        uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
    }

    public void OnReload()
    {
        uniWebView.Reload();
    }

    public void OnHide()
    {
        if (checkRotationCoroutine != null)
            StopCoroutine(checkRotationCoroutine);

        Screen.orientation = ScreenOrientation.Portrait;

        uniWebView.Hide();
    }

    public void OnShow()
    {
        uniWebView.Show();
        uniWebView.SetShowToolbarNavigationButtons(false);
        uniWebView.SetShowToolbar(false);

        checkRotationCoroutine = CheckRotationDevice_Coroutine();
        StartCoroutine(checkRotationCoroutine);

        Screen.orientation = ScreenOrientation.AutoRotation;

    }

    public void OnStartDisplay()
    {
        //UnityEngine.Debug.Log("LOAD PAGE");

        textLoading.text = "Loading page...";
    }

    public void OnFinishDisplay()
    {
        textLoading.text = "";
    }

    public void OnErrorDisplay(string errorMessage)
    {
        textLoading.text = errorMessage;
    }

    private IEnumerator CheckRotationDevice_Coroutine()
    {
        var orientation = Screen.orientation;

        while (true)
        {
            if(orientation != Screen.orientation)
            {
                SetFullScreen();
                orientation = Screen.orientation;
            }
            yield return null;
        }
    }


    #region Input

    private void OnPageFinished(UniWebView webView, int statusCode, string url)
    {
        OnFinish?.Invoke(webView, url);
    }

    private void OnPageStarted(UniWebView webView, string url)
    {
        OnStart?.Invoke(webView, url);
    }

    private void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
    {
        OnError?.Invoke(webView, errorCode, errorMessage);
    }

    private bool OnShouldClose(UniWebView webView)
    {
        OnClosePage?.Invoke();

        return true;
    }

    #endregion
}
