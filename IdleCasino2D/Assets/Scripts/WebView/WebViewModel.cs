using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class WebViewModel
{
    //public event Action OnPageStarted_Action;
    //public event Action OnPageClosed_Action;
    public event Action OnFail;
    public event Action<string> OnGetLink;
    public event Action OnStartPage;
    public event Action OnFinishPage;
    public event Action<string> OnErrorPage;
    public event Action<string> OnLoad;
    public event Action OnReload;
    public event Action OnShow;
    public event Action OnHide;

    private string URL;

    public WebViewModel(string URL = null)
    {
        this.URL = URL;
    }

    public void GetLinkInTitleFromURL(string URL)
    {
        //Debug.Log("LOAD");
        Coroutines.Start(GetLinkOnTitle(URL));
    }

    private IEnumerator GetLinkOnTitle(string URL)
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);

        var operation = request.SendWebRequest();

        float timeOut = 4f;
        float startTime = Time.time;

        yield return new WaitUntil(() => operation.isDone || (Time.time - startTime) > timeOut);

        if (request.result != UnityWebRequest.Result.Success || !operation.isDone)
        {
            Debug.Log(request.result);
            OnFail?.Invoke();
            yield break;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            string html = request.downloadHandler.text;

            string link = GetLinkFromHTML(html);

            //Debug.Log(link);

            OnGetLink?.Invoke(link);
        }
    }

    public string GetLinkFromHTML(string title)
    {
        var match = Regex.Match(title, @"<title>s*(.+?)s*</title>", RegexOptions.IgnoreCase);
        {
            //Debug.Log(match);

            if (match.Success)
            {
                if (match.Groups[1].Value.StartsWith("https://"))
                {
                    return match.Groups[1].Value;
                }
            }

            return null;
        }
    }

    public void SetURL(string URL)
    {
        this.URL = URL;
    }

    public void Load()
    {
        if (URL == null) 
        {
            //Debug.Log("��� ������ ��� �����������");
            return;
        }

        //Debug.Log("LOAD - " + URL);
        OnLoad?.Invoke(URL);
    }

    private void Show()
    {
        //Debug.Log("����� �������� - " + URL);
        OnShow?.Invoke();
    }

    private void Hide()
    {
        //Debug.Log("������� �������� - " + URL);
        OnHide?.Invoke();
    }

    public void OnPageStarted(UniWebView webView, string URL)
    {
        OnStartPage?.Invoke();
    }

    public void OnPageFinished(UniWebView webView, string URL)
    {
        OnFinishPage?.Invoke();
        Show();
    }

    public void OnPageClosed()
    {
        Hide();
    }

    public void OnError(UniWebView webView, int code, string message)
    {
        OnErrorPage?.Invoke(message);

        //Debug.Log(code + ": " + message);
    }

    //public string GetLink(string URL)
    //{
    //    using (UnityWebRequest siteRequest = UnityWebRequest.Get(URL))
    //    {
    //        yield return siteRequest.SendWebRequest();

    //        if (siteRequest.result == UnityWebRequest.Result.Success)
    //        {
    //            string html = siteRequest.downloadHandler.text;

    //            //Debug.Log(GetLinkFromTitle(html));
    //            return html;

    //        }
    //    }
    //}
}
