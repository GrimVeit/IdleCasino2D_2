using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial05_Leaderboard_Start_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IClickVisualProvider _clickVisualProvider;
    private bool isOpenLeaderboard = false;

    private IEnumerator timer;

    public Tutorial05_Leaderboard_Start_State_Game(IStateMachineProvider machineProvider, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, UIGameRoot sceneRoot, IClickVisualProvider clickVisualProvider)
    {
        _machineProvider = machineProvider;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _sceneRoot = sceneRoot;
        _clickVisualProvider = clickVisualProvider;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - TUTORIAL 05 FIRST TABLES / GAME</color>");

        isOpenLeaderboard = false;
        _sceneRoot.OnClickToLeader_AvatarBalance += OpenLeaderboard;

        if (timer != null) Coroutines.Stop(timer);

        timer = Timer();
        Coroutines.Start(timer);
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToLeader_AvatarBalance -= OpenLeaderboard;

        if (timer != null) Coroutines.Stop(timer);

        _sceneRoot.CloseMainPanel();
    }

    private IEnumerator Timer()
    {
        _tutorialDialogueProvider.SetMessage("tutorial.leaderboard.01", 3.5f);

        yield return new WaitForSeconds(4f);

        _tutorialDialogueProvider.SetMessage("tutorial.leaderboard.02", 3.2f);

        yield return new WaitForSeconds(3.7f);

        _tutorialMaskotProvider.Deactivate();
        _sceneRoot.CloseBlackBackgroundPanel();
        _sceneRoot.OpenAvatarBalancePanel();
        _clickVisualProvider.Show();
        _clickVisualProvider.MoveTo("LEADERBOARD_MAINPANEL");

        yield return new WaitUntil(() => isOpenLeaderboard == true);

        _clickVisualProvider.Hide();

        ChangeStateToTutorial5_Finish();
    }

    private void OpenLeaderboard()
    {
        isOpenLeaderboard = true;
    }

    private void ChangeStateToTutorial5_Finish()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial05_Leaderboard_Finish_State_Game>());
    }
}
