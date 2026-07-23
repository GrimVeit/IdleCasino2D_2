using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial05_Leaderboard_Finish_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IAdministratorVisualProvider _administratorVisualProvider;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;

    private IEnumerator timer;

    public Tutorial05_Leaderboard_Finish_State_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IAdministratorVisualProvider administratorVisualProvider, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _administratorVisualProvider = administratorVisualProvider;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - TUTORIAL 05 FIRST TABLES / GAME</color>");

        if(timer != null) Coroutines.Stop(timer);

        timer = Timer();
        Coroutines.Start(timer);

        _administratorVisualProvider.Activate();
        _sceneRoot.OpenLeaderboardPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
    }

    public void ExitState()
    {
        if (timer != null) Coroutines.Stop(timer);
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(2f);

        _tutorialMaskotProvider.Activate();

        yield return new WaitForSeconds(0.3f);

        _tutorialDialogueProvider.SetMessage("tutorial.leaderboard.03", 3f);

        yield return new WaitForSeconds(3.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.leaderboard.04", 3f);

        yield return new WaitForSeconds(3.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.leaderboard.05", 3.5f);

        yield return new WaitForSeconds(4f);

        _sceneRoot.CloseLeaderboardPanel();
        _sceneRoot.CloseAvatarBalancePanel();

        yield return new WaitForSeconds(1f);

        _administratorVisualProvider.Deactivate();

        ChangeStateToTutorial6();
    }

    private void ChangeStateToTutorial6()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial06_Complete_State_Game>());
    }
}
