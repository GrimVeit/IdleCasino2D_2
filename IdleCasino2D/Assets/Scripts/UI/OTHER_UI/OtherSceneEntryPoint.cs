using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private UIOtherSceneRoot sceneRootPrefab;

    private UIOtherSceneRoot sceneRoot;
    private BankPresenter bankPresenter;
    private ViewContainer viewContainer;
    //private WebViewPresenter webViewPresenter;

    public void Run(UIRootView uIRootView)
    {
        sceneRoot = Instantiate(sceneRootPrefab);
        uIRootView.AttachSceneUI(sceneRoot.gameObject, Camera.main);

        viewContainer = sceneRoot.GetComponent<ViewContainer>();
        viewContainer.Initialize();

        bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());
        bankPresenter.Initialize();

        //webViewPresenter = new WebViewPresenter(new WebViewModel(), viewContainer.GetView<WebViewView>());
        //webViewPresenter.Initialize();

        ActivateActions();

        //firebaseDatabasePresenter.GetLink();
    }

    private void ActivateActions()
    {
        //firebaseDatabasePresenter.OnGetLink += GetURLBd;
        //firebaseDatabasePresenter.OnErrorGetLink += GoToMainMenu;

        //webViewPresenter.OnGetLinkFromTitle += GetUrl;
        //webViewPresenter.OnFail += GoToMainMenu;
    }

    private void DeactivateActions()
    {
        //firebaseDatabasePresenter.OnGetLink -= GetURLBd;
        //firebaseDatabasePresenter.OnErrorGetLink -= GoToMainMenu;

        //webViewPresenter.OnGetLinkFromTitle -= GetUrl;
        //webViewPresenter.OnFail -= GoToMainMenu;
    }

    private void GetURLBd(string link)
    {
        //webViewPresenter.GetLinkInTitleFromURL(link);
    }

    private void GetUrl(string URL)
    {
        if (URL == null)
        {
            GoToMainMenu();
            return;
        }

        //webViewPresenter.SetURL(URL);
        //webViewPresenter.Load();
    }

    private void GoToMainMenu()
    {
        //Debug.Log("NO GOOD, OPEN MAIN MENU");
        OnGoToMainMenu?.Invoke();
    }

    private void OnDestroy()
    {
        DeactivateActions();

        //webViewPresenter.Dispose();
    }

    #region Input

    public event Action OnGoToMainMenu;

    #endregion
}
