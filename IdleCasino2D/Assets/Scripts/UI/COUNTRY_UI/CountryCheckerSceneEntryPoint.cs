using System;
using UnityEngine;
using System.Collections.Generic;

public class CountryCheckerSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Sounds sounds;
    [SerializeField] private UICountryCheckerSceneRoot sceneRootPrefab;

    private UICountryCheckerSceneRoot sceneRoot;
    private ViewContainer viewContainer;

    private GeoLocationPresenter geoLocationPresenter;
    private InternetPresenter internetPresenter;
    private SoundPresenter soundPresenter;
    private DatabasePresenter databasePresenter;

    private BankPresenter bankPresenter;

    private string currentCountry;

    public void Run(UIRootView uIRootView)
    {
        Debug.Log("OPEN COUNTRY CHECKER SCENE");

        sceneRoot = Instantiate(sceneRootPrefab);
        uIRootView.AttachSceneUI(sceneRoot.gameObject, Camera.main);

        viewContainer = sceneRoot.GetComponent<ViewContainer>();
        viewContainer.Initialize();

        soundPresenter = new SoundPresenter(new SoundModel(sounds.sounds, PlayerPrefsKeys.IS_MUTE_SOUNDS, PlayerPrefsKeys.KEY_VOLUME_SOUND, PlayerPrefsKeys.KEY_VOLUME_MUSIC), viewContainer.GetView<SoundView>());
        soundPresenter.Initialize();

        databasePresenter = new DatabasePresenter(new DatabaseModel());

        bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());
        bankPresenter.Initialize();

        geoLocationPresenter = new GeoLocationPresenter(new GeoLocationModel());

        internetPresenter = new InternetPresenter(new InternetModel(), viewContainer.GetView<InternetView>());
        internetPresenter.Initialize();

        ActivateActions();

        internetPresenter.StartCheckConnection();

    }

    public void Dispose()
    {
        DeactivateActions();

        internetPresenter?.Dispose();
    }

    private void ActivateActions()
    {
        internetPresenter.OnInternetUnavailable += TransitionToMainMenu;
        internetPresenter.OnInternetAvailable += OnInternetAvailable;

        geoLocationPresenter.OnErrorGetCountry += TransitionToMainMenu;
        geoLocationPresenter.OnGetCountry += ActivateSceneInCountry;

        databasePresenter.OnErrorGetCountries += TransitionToMainMenu;
        databasePresenter.OnGetCountries += CheckCountry;
    }

    private void DeactivateActions()
    {
        internetPresenter.OnInternetUnavailable -= TransitionToMainMenu;
        internetPresenter.OnInternetAvailable -= OnInternetAvailable;

        geoLocationPresenter.OnErrorGetCountry -= TransitionToMainMenu;
        geoLocationPresenter.OnGetCountry -= ActivateSceneInCountry;

        databasePresenter.OnErrorGetCountries -= TransitionToMainMenu;
        databasePresenter.OnGetCountries -= CheckCountry;
    }

    private void OnInternetAvailable()
    {
        Debug.Log("INTERNET CONNECTION = TRUE");
        geoLocationPresenter.GetUserCountry();
    }

    private void ActivateSceneInCountry(string country)
    {
        currentCountry = country;

        Debug.Log($"Country: {country}");

        databasePresenter.GetCountries();
    }

    private void CheckCountry(List<string> countries)
    {
        if (countries.Contains(currentCountry))
        {
            Debug.Log("GOOD COUNTRY = TRUE");
            TransitionToOther();
        }
        else
        {
            Debug.Log("GOOD COUNTRY = FALSE");
            TransitionToMainMenu();
        }
    }

    #region Input

    public event Action GoToGame;
    public event Action GoToOther;

    private void TransitionToMainMenu()
    {
        Dispose();
        Debug.Log("NO GOOD");
        GoToGame?.Invoke();
    }

    private void TransitionToOther()
    {
        Dispose();
        Debug.Log("GOOD");
        GoToOther?.Invoke();
    }

    #endregion
}
