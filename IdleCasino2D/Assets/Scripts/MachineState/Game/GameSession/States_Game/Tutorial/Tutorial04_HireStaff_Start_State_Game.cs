using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial04_HireStaff_Start_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IClickVisualProvider _clickVisualProvider;
    private bool isOpenHireStaff = false;

    private IEnumerator timer;

    public Tutorial04_HireStaff_Start_State_Game(IStateMachineProvider machineProvider, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, UIGameRoot sceneRoot, IClickVisualProvider clickVisualProvider)
    {
        _machineProvider = machineProvider;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _sceneRoot = sceneRoot;
        _clickVisualProvider = clickVisualProvider;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - TUTORIAL 02 FIRST TABLES / GAME</color>");

        isOpenHireStaff = false;
        _sceneRoot.OnClickToHireStaff_MAIN += OpenHireStaff;

        if (timer != null) Coroutines.Stop(timer);

        timer = Timer();
        Coroutines.Start(timer);
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToHireStaff_MAIN -= OpenHireStaff;

        if (timer != null) Coroutines.Stop(timer);

        _sceneRoot.CloseMainPanel();
    }

    private IEnumerator Timer()
    {
        _tutorialDialogueProvider.SetMessage("tutorial.staff.01", 3.2f);

        yield return new WaitForSeconds(3.7f);

        _tutorialDialogueProvider.SetMessage("tutorial.staff.02", 3.2f);

        yield return new WaitForSeconds(3.7f);

        _tutorialDialogueProvider.SetMessage("tutorial.staff.03", 3f);

        yield return new WaitForSeconds(3.5f);

        _tutorialMaskotProvider.Deactivate();
        _sceneRoot.CloseBlackBackgroundPanel();
        _sceneRoot.OpenMainPanel();
        _sceneRoot.OpenAvatarBalancePanel();
        _clickVisualProvider.Show();
        _clickVisualProvider.MoveTo("HIRESTAFF_MAINPANEL");

        yield return new WaitUntil(() => isOpenHireStaff == true);

        ChangeStateToTutorial3_ChooseUpgradeTable();
    }

    private void OpenHireStaff()
    {
        isOpenHireStaff = true;
    }

    private void ChangeStateToTutorial3_ChooseUpgradeTable()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial04_HireStaff_ChooseStaffType_State_Game>());
    }
}
