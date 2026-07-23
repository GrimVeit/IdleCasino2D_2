using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial03_Upgrade_Finish_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly ICasinoProfitStoreListener _casinoProfitStoreListener;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly ICasinoProfitProvider _casinoProfitProvider;
    private readonly IAdministratorVisualProvider _administratorVisualProvider;
    private readonly IClickVisualProvider _clickVisualProvider;

    private bool isUpgrade = false;
    private IEnumerator timer;

    public Tutorial03_Upgrade_Finish_State_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, ICasinoProfitStoreListener casinoProfitStoreListener, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, ICasinoProfitProvider casinoProfitProvider, IAdministratorVisualProvider administratorVisualProvider, IClickVisualProvider clickVisualProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _casinoProfitStoreListener = casinoProfitStoreListener;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _casinoProfitProvider = casinoProfitProvider;
        _administratorVisualProvider = administratorVisualProvider;
        _clickVisualProvider = clickVisualProvider;
    }

    public void EnterState()
    {
        if(timer != null) Coroutines.Stop(timer);

        isUpgrade = false;

        _casinoProfitStoreListener.OnProfitStoreChanged += ChangeStateToUpgrade;

        _casinoProfitProvider.ActivateUpgrade();
        _sceneRoot.OpenProfitUpgradePanel();
        _clickVisualProvider.MoveTo("UPGRADE_PROFITUPGRADEPANEL");
    }

    public void ExitState()
    {
        if (timer != null) Coroutines.Stop(timer);

        _casinoProfitStoreListener.OnProfitStoreChanged -= ChangeStateToUpgrade;
    }

    private void ChangeStateToUpgrade(CasinoEntityType type, int value, bool isFirst)
    {
        if(isUpgrade) return;

        if (!isFirst)
        {
            _casinoProfitProvider.DeactivateUpgrade();

            if (timer != null) Coroutines.Stop(timer);

            timer = Timer();
            Coroutines.Start(timer);
        }
    }

    private IEnumerator Timer()
    {
        _clickVisualProvider.Hide();

        yield return new WaitForSeconds(1f);

        _tutorialMaskotProvider.Activate();

        yield return new WaitForSeconds(0.3f);

        _tutorialDialogueProvider.SetMessage("tutorial.upgrade.05", 2f);

        yield return new WaitForSeconds(2.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.upgrade.06", 3f);

        yield return new WaitForSeconds(3.5f);

        _sceneRoot.CloseProfitUpgradePanel();
        _sceneRoot.CloseAvatarBalancePanel();

        yield return new WaitForSeconds(1f);

        _administratorVisualProvider.Deactivate();

        ChangeStateToTutorial4();
    }

    private void ChangeStateToTutorial4()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial04_HireStaff_Start_State_Game>());
    }
}
