using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial04_HireStaff_Start_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly UIGameRoot _sceneRoot;
    private bool isOpenHireStaff = false;

    private IEnumerator timer;

    public Tutorial04_HireStaff_Start_State_Game(IStateMachineProvider machineProvider, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _sceneRoot = sceneRoot;
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
        //_tutorialDialogueProvider.SetMessage("tutorial.interface.01", 3.8f);

        yield return new WaitForSeconds(5.3f);

        //_tutorialDialogueProvider.SetMessage("tutorial.upgrade.01", 3f);
        
        //yield return new WaitForSeconds(3.5f);

        //_tutorialDialogueProvider.SetMessage("tutorial.upgrade.02", 3f);

        //yield return new WaitForSeconds(3.5f);

        //_tutorialDialogueProvider.SetMessage("tutorial.upgrade.03", 3.5f);

        //yield return new WaitForSeconds(4f);

        //_tutorialDialogueProvider.SetMessage("tutorial.upgrade.04", 3f);

        //yield return new WaitForSeconds(3.5f);

        //_tutorialMaskotProvider.Deactivate();
        //_sceneRoot.CloseBlackBackgroundPanel();
        //_sceneRoot.OpenMainPanel();
        //_sceneRoot.OpenAvatarBalancePanel();

        //yield return new WaitUntil(() => isOpenHireStaff == true);

        //ChangeStateToTutorial3_ChooseUpgradeTable();
    }

    private void OpenHireStaff()
    {
        isOpenHireStaff = true;
    }

    private void ChangeStateToTutorial3_ChooseUpgradeTable()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial03_Upgrade_ChooseUpgradeType_State_Game>());
    }
}
