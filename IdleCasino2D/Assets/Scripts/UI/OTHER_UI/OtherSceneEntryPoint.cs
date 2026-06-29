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
    private WebViewPresenter webViewPresenter;

    private DatabasePresenter databasePresenter;

    public void Run(UIRootView uIRootView)
    {
        sceneRoot = Instantiate(sceneRootPrefab);
        uIRootView.AttachSceneUI(sceneRoot.gameObject, Camera.main);

        viewContainer = sceneRoot.GetComponent<ViewContainer>();
        viewContainer.Initialize();

        databasePresenter = new DatabasePresenter(new DatabaseModel());

        bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());
        bankPresenter.Initialize();

        webViewPresenter = new WebViewPresenter(new WebViewModel(), viewContainer.GetView<WebViewView>());
        webViewPresenter.Initialize();

        ActivateActions();

        databasePresenter.GetLink();
    }

    private void ActivateActions()
    {
        databasePresenter.OnGetLink += GetURLBd;
        databasePresenter.OnErrorGetLink += GoToMainMenu;

        webViewPresenter.OnGetLinkFromTitle += GetUrl;
        webViewPresenter.OnFail += GoToMainMenu;
    }

    private void DeactivateActions()
    {
        databasePresenter.OnGetLink -= GetURLBd;
        databasePresenter.OnErrorGetLink -= GoToMainMenu;

        webViewPresenter.OnGetLinkFromTitle -= GetUrl;
        webViewPresenter.OnFail -= GoToMainMenu;
    }

    private void GetURLBd(string link)
    {
        webViewPresenter.GetLinkInTitleFromURL(link);
    }

    private void GetUrl(string URL)
    {
        if (URL == null)
        {
            GoToMainMenu();
            return;
        }

        webViewPresenter.SetURL(URL);
        webViewPresenter.Load();
    }

    private void GoToMainMenu()
    {
        Debug.Log("NO GOOD, OPEN MAIN MENU");
        OnGoToGame?.Invoke();
    }

    private void OnDestroy()
    {
        DeactivateActions();

        webViewPresenter.Dispose();
    }

    #region Input

    public event Action OnGoToGame;

    #endregion
}
