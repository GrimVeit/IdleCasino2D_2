using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameEntryPoint : ISceneNavigator
{
    private static GameEntryPoint instance;

    private readonly Coroutines coroutines;
    private readonly UIRootView rootView;

    private ISceneEntryPoint currentScene;

    private bool isLoading = false;

    public GameEntryPoint()
    {
        coroutines = new GameObject("[Coroutines]")
            .AddComponent<Coroutines>();

        Object.DontDestroyOnLoad(coroutines.gameObject);

        var prefab = Resources.Load<UIRootView>("UIRoot");
        rootView = Object.Instantiate(prefab);

        Object.DontDestroyOnLoad(rootView.gameObject);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Autorun()
    {
        SetupGlobalSettings();

        instance = new GameEntryPoint();

        instance.LoadScene(Scenes.BOOTSTRAP, false);
    }

    private static void SetupGlobalSettings()
    {
        Application.targetFrameRate = 90;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void LoadScene(string sceneName, bool showLoading = true)
    {
        if (isLoading) return;

        coroutines.StartCoroutine(LoadSceneRoutine(sceneName, showLoading));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, bool showLoading)
    {
        isLoading = true;

        currentScene?.Dispose();
        currentScene = null;

        if (showLoading)
        {
            yield return rootView.ShowLoadingScreen(0);
            yield return new WaitForSeconds(0.4f);
        }

        yield return SceneManager.LoadSceneAsync(sceneName);

        currentScene = Object
            .FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ISceneEntryPoint>()
            .FirstOrDefault();

        if (showLoading)
        {
            yield return new WaitForSeconds(0.6f);
        }

        if (currentScene == null)
        {
            Debug.LogError($"Scene '{sceneName}' doesn't contain ISceneEntryPoint");
            yield break;
        }

        currentScene.Initialize(this, rootView);

        if (showLoading)
        {
            yield return new WaitForSeconds(1f);
            yield return rootView.HideLoadingScreen(0);
        }

        isLoading = false;
    }

    //private IEnumerator LoadSceneAndRun<TSceneEntry>(string scene, bool showLoading = false, Action<TSceneEntry> setup = null)
    //    where TSceneEntry : MonoBehaviour
    //{
    //    if (showLoading)
    //    {
    //        yield return rootView.ShowLoadingScreen(0);
    //        yield return new WaitForSeconds(0.4f);
    //    }

    //    yield return SceneManager.LoadSceneAsync(scene);

    //    if (showLoading)
    //    {
    //        yield return new WaitForSeconds(0.6f);
    //    }

    //    var sceneEntryPoint = Object.FindObjectOfType<TSceneEntry>();
    //    setup?.Invoke(sceneEntryPoint);

    //    if (showLoading)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        yield return rootView.HideLoadingScreen(0);
    //    }
    //}
}

public interface ISceneNavigator
{
    void LoadScene(string sceneName, bool showLoading = true);
}

public interface ISceneEntryPoint
{
    void Initialize(ISceneNavigator navigator, UIRootView uiRoot);
    void Dispose();
}

