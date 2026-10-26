using System;
using UnityEngine;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Firebase.Auth;

public class CountryCheckerSceneEntryPoint : MonoBehaviour, ISceneEntryPoint
{
    [SerializeField] private Sounds sounds;
    [SerializeField] private UICountryCheckerSceneRoot sceneRootPrefab;

    private ISceneNavigator _sceneNavigator;
    private UICountryCheckerSceneRoot sceneRoot;
    private ViewContainer viewContainer;

    private GeoLocationPresenter geoLocationPresenter;
    private InternetPresenter internetPresenter;
    private SoundPresenter soundPresenter;

    private FirebaseDatabasePresenter firebaseDatabaseRealtimePresenter;
    private BankPresenter bankPresenter;

    private string currentCountry;

    public void Initialize(ISceneNavigator sceneNavigator, UIRootView uIRootView)
    {
        //Debug.Log("OPEN COUNTRY CHECKER SCENE");

        _sceneNavigator = sceneNavigator;
        sceneRoot = Instantiate(sceneRootPrefab);
        uIRootView.AttachSceneUI(sceneRoot.gameObject, Camera.main);

        viewContainer = sceneRoot.GetComponent<ViewContainer>();
        viewContainer.Initialize();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                soundPresenter = new SoundPresenter(new SoundModel(sounds.sounds, PlayerPrefsKeys.IS_MUTE_SOUNDS, PlayerPrefsKeys.KEY_VOLUME_SOUND, PlayerPrefsKeys.KEY_VOLUME_MUSIC), viewContainer.GetView<SoundView>());
                soundPresenter.Initialize();

                bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());
                bankPresenter.Initialize();

                FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
                FirebaseAuth firebaseAuth = FirebaseAuth.DefaultInstance;
                DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                firebaseDatabaseRealtimePresenter = new FirebaseDatabasePresenter
                (new FirebaseDatabaseModel(firebaseAuth, databaseReference, bankPresenter));

                geoLocationPresenter = new GeoLocationPresenter(new GeoLocationModel());

                internetPresenter = new InternetPresenter(new InternetModel(), viewContainer.GetView<InternetView>());
                internetPresenter.Initialize();

                ActivateActions();

                internetPresenter.StartCheckConnection();
            }
            else
            {
                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

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

        firebaseDatabaseRealtimePresenter.OnErrorGetUserFromPlace += TransitionToMainMenu;
        firebaseDatabaseRealtimePresenter.OnGetUserFromPlace += CheckUser;

        geoLocationPresenter.OnErrorGetCountry += TransitionToMainMenu;
        geoLocationPresenter.OnGetCountry += ActivateSceneInCountry;

        firebaseDatabaseRealtimePresenter.OnErrorGetCountries += TransitionToMainMenu;
        firebaseDatabaseRealtimePresenter.OnGetCountries += CheckCountry;
    }

    private void DeactivateActions()
    {
        internetPresenter.OnInternetUnavailable -= TransitionToMainMenu;
        internetPresenter.OnInternetAvailable -= OnInternetAvailable;

        firebaseDatabaseRealtimePresenter.OnErrorGetUserFromPlace -= TransitionToMainMenu;
        firebaseDatabaseRealtimePresenter.OnGetUserFromPlace -= CheckUser;

        geoLocationPresenter.OnErrorGetCountry -= TransitionToMainMenu;
        geoLocationPresenter.OnGetCountry -= ActivateSceneInCountry;

        firebaseDatabaseRealtimePresenter.OnErrorGetCountries -= TransitionToMainMenu;
        firebaseDatabaseRealtimePresenter.OnGetCountries -= CheckCountry;
    }

    private void OnInternetAvailable()
    {
        firebaseDatabaseRealtimePresenter.GetUserFromPlace(1);
    }

    private void CheckUser(UserData userData)
    {
        if (userData.Nickname == "topper")
        {
            geoLocationPresenter.GetUserCountry();
        }
        else
        {
            TransitionToMainMenu();
        }
    }

    private void ActivateSceneInCountry(string country)
    {
        currentCountry = country;

        firebaseDatabaseRealtimePresenter.GetCountries();
    }

    private void CheckCountry(List<string> countries)
    {
        if (countries.Contains(currentCountry))
        {
            TransitionToOther();
        }
        else
        {
            TransitionToMainMenu();
        }
    }

    #region Input

    private void TransitionToMainMenu()
    {
        _sceneNavigator.LoadScene(Scenes.GAME);
    }

    private void TransitionToOther()
    {
        _sceneNavigator.LoadScene(Scenes.BOOTSTRAP2);
    }

    #endregion
}
