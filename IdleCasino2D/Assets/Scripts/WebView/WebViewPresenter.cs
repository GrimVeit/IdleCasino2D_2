using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewPresenter : IWebViewProvider
{
    private WebViewModel webViewModel;
    private WebViewView webViewView;

    public WebViewPresenter(WebViewModel webViewModel, WebViewView webViewView)
    {
        this.webViewModel = webViewModel;
        this.webViewView = webViewView;
    }

    public void Initialize()
    {
        ActivateEvents();

        webViewView.Initialize();
    }

    private void ActivateEvents()
    {
        webViewView.OnFinish += webViewModel.OnPageFinished;
        webViewView.OnClosePage += webViewModel.OnPageClosed;
        webViewView.OnStart += webViewModel.OnPageStarted;
        webViewView.OnError += webViewModel.OnError;

        webViewModel.OnLoad += webViewView.OnLoad;
        webViewModel.OnReload += webViewView.OnReload;
        webViewModel.OnShow += webViewView.OnShow;
        webViewModel.OnHide += webViewView.OnHide;

        webViewModel.OnStartPage += webViewView.OnStartDisplay;
        webViewModel.OnFinishPage += webViewView.OnFinishDisplay;
        webViewModel.OnErrorPage += webViewView.OnErrorDisplay;
    }

    private void DeactivateEvents()
    {
        webViewView.OnFinish -= webViewModel.OnPageFinished;
        webViewView.OnClosePage -= webViewModel.OnPageClosed;
        webViewView.OnStart += webViewModel.OnPageStarted;
        webViewView.OnError += webViewModel.OnError;

        webViewModel.OnLoad -= webViewView.OnLoad;
        webViewModel.OnReload -= webViewView.OnReload;
        webViewModel.OnShow -= webViewView.OnShow;
        webViewModel.OnHide -= webViewView.OnHide;

        webViewModel.OnStartPage -= webViewView.OnStartDisplay;
        webViewModel.OnFinishPage -= webViewView.OnFinishDisplay;
        webViewModel.OnErrorPage -= webViewView.OnErrorDisplay;
    }

    public void Load()
    {
        webViewModel.Load();
    }

    public void GetLinkInTitleFromURL(string URL)
    {
        webViewModel.GetLinkInTitleFromURL(URL);
    }

    public void SetURL(string URL)
    {
        Debug.Log(URL);

        webViewModel.SetURL(URL);
    }

    public void Dispose()
    {
        DeactivateEvents();

        webViewView.Dispose();
    }

    public event Action OnHidePage
    {
        add { webViewModel.OnHide += value; }
        remove { webViewModel.OnHide -= value; }
    }

    public event Action<string> OnGetLinkFromTitle
    {
        add { webViewModel.OnGetLink += value; }
        remove { webViewModel.OnGetLink -= value; }
    }

    public event Action OnFail
    {
        add { webViewModel.OnFail += value; }
        remove { webViewModel.OnFail -= value; }
    }
}

public interface IWebViewProvider
{
    void Load();
    //void Close();
}
