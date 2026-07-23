using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial06_Complete_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly UIGameRoot _sceneRoot;

    private IEnumerator timer;

    public Tutorial06_Complete_State_Game(IStateMachineProvider machineProvider, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - TUTORIAL 06 COMPLETE / GAME</color>");

        if (timer != null) Coroutines.Stop(timer);

        timer = Timer();
        Coroutines.Start(timer);
    }

    public void ExitState()
    {
        if (timer != null) Coroutines.Stop(timer);
    }

    private IEnumerator Timer()
    {
        _tutorialDialogueProvider.SetMessage("tutorial.complete.01", 2f);

        yield return new WaitForSeconds(2.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.complete.02", 2.7f);

        yield return new WaitForSeconds(3.2f);

        _tutorialDialogueProvider.SetMessage("tutorial.complete.03", 3f);

        yield return new WaitForSeconds(3.5f);

        _sceneRoot.CloseBlackBackgroundPanel();
        _tutorialMaskotProvider.Deactivate();

        yield return new WaitForSeconds(0.3f);

        ChangeStateToMain();
    }

    private void ChangeStateToMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
