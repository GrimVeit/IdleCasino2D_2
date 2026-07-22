using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial01_Welcome_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly UIGameRoot _sceneRoot;

    private IEnumerator timer;

    public Tutorial01_Welcome_State_Game(IStateMachineProvider machineProvider, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - TUTORIAL 01 WELCOME / GAME</color>");

        if(timer != null) Coroutines.Stop(timer);

        timer = Timer();
        Coroutines.Start(timer);
    }

    public void ExitState()
    {
        if (timer != null) Coroutines.Stop(timer);
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.2f);

        _sceneRoot.OpenBlackBackgroundPanel();

        yield return new WaitForSeconds(0.3f);

        _tutorialMaskotProvider.Activate();

        yield return new WaitForSeconds(0.3f);

        _tutorialDialogueProvider.SetMessage("tutorial.welcome.01", 2f);

        yield return new WaitForSeconds(2.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.welcome.02", 3f);

        yield return new WaitForSeconds(3.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.welcome.03", 3.5f);

        yield return new WaitForSeconds(4f);

        _tutorialDialogueProvider.SetMessage("tutorial.welcome.04", 3.8f);

        yield return new WaitForSeconds(5.3f);

        ChangeStateToTutorial2();
    }

    private void ChangeStateToTutorial2()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial02_FirstTables_State_Game>());
    }
}
