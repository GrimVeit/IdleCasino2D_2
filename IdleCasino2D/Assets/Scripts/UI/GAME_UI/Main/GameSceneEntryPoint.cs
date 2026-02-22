using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Sounds sounds;
    [SerializeField] private UIGameRoot menuRootPrefab;

    private UIGameRoot sceneRoot;
    private ViewContainer viewContainer;

    private BankPresenter bankPresenter;
    private ParticleEffectPresenter particleEffectPresenter;
    private SoundPresenter soundPresenter;

    private ClickDispatcherPresenter clickDispatcherPresenter;
    //private TouchCameraPresenter touchCameraPresenter;

    private StateMachine_Game stateMachine;

    public void Run(UIRootView uIRootView)
    {
        sceneRoot = menuRootPrefab;

        uIRootView.AttachSceneUI(sceneRoot.gameObject, Camera.main);

        //viewContainer = sceneRoot.GetComponent<ViewContainer>();
        //viewContainer.Initialize();

        //soundPresenter = new SoundPresenter
        //            (new SoundModel(sounds.sounds, PlayerPrefsKeys.IS_MUTE_SOUNDS, PlayerPrefsKeys.KEY_VOLUME_SOUND, PlayerPrefsKeys.KEY_VOLUME_MUSIC),
        //            viewContainer.GetView<SoundView>());

        //particleEffectPresenter = new ParticleEffectPresenter
        //    (new ParticleEffectModel(),
        //    viewContainer.GetView<ParticleEffectView>());

        //bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());

        clickDispatcherPresenter = new ClickDispatcherPresenter(new ClickDispatcherModel());
        //touchCameraPresenter = new TouchCameraPresenter(viewContainer.GetView<TouchCameraView>());
        
        sceneRoot.SetSoundProvider(soundPresenter);
        sceneRoot.Activate();

        ActivateEvents();

        //soundPresenter.Initialize();
        //particleEffectPresenter.Initialize();
        //sceneRoot.Initialize();
        //bankPresenter.Initialize();
        
        //stateMachine = new StateMachine_Game();

        //stateMachine.Initialize();
        //touchCameraPresenter.Initialize();

        Debug.Log("GameSceneEntryPoint Run");
        clickDispatcherPresenter.Activate();
        //touchCameraPresenter.ActivateInteractive();
    }

    private void ActivateEvents()
    {
        ActivateTransitions();
    }

    private void DeactivateEvents()
    {
        DeactivateTransitions();
    }

    private void ActivateTransitions()
    {

    }

    private void DeactivateTransitions()
    {

    }

    private void Deactivate()
    {
        clickDispatcherPresenter?.Deactivate();
        sceneRoot.Deactivate();
        soundPresenter?.Dispose();
    }

    private void Dispose()
    {
        DeactivateEvents();

        soundPresenter?.Dispose();
        sceneRoot?.Dispose();
        particleEffectPresenter?.Dispose();
        bankPresenter?.Dispose();

        //touchCameraPresenter?.Dispose();
        
        stateMachine?.Dispose();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    #region Output


    public event Action OnClickToMenu;
    public event Action OnClickToGame;

    private void HandleClickToMenu()
    {
        Deactivate();

        OnClickToMenu?.Invoke();
    }

    private void HandleClickToGame()
    {
        Deactivate();

        OnClickToGame?.Invoke();
    }

    #endregion
}
