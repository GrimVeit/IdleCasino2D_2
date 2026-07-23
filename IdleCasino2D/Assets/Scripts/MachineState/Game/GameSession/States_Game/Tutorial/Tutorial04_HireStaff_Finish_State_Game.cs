using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial04_HireStaff_Finish_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IFilterShopCasinoStaffProvider _filterShopCasinoStaffActivatorProvider;
    private readonly IFilterShopCasinoStaffListener _filterShopCasinoStaffListener;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly IAdministratorVisualProvider _administratorVisualProvider;
    private readonly ITouchCameraProvider _touchCameraProvider;
    private bool isStaffBuyed = false;

    private IEnumerator timer;

    public Tutorial04_HireStaff_Finish_State_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IFilterShopCasinoStaffProvider filterShopCasinoStaffActivatorProvider, IFilterShopCasinoStaffListener filterShopCasinoStaffListener, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, IAdministratorVisualProvider administratorVisualProvider, ITouchCameraProvider touchCameraProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _filterShopCasinoStaffActivatorProvider = filterShopCasinoStaffActivatorProvider;
        _filterShopCasinoStaffListener = filterShopCasinoStaffListener;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _administratorVisualProvider = administratorVisualProvider;
        _touchCameraProvider = touchCameraProvider;
    }

    public void EnterState()
    {
        if (timer != null) Coroutines.Stop(timer);

        isStaffBuyed = false;

        _filterShopCasinoStaffListener.OnStaffPurchased += ChangeStateToMainState;

        _sceneRoot.OpenSelectStaffPanel();
    }

    public void ExitState()
    {
        if (timer != null) Coroutines.Stop(timer);

        _filterShopCasinoStaffListener.OnStaffPurchased -= ChangeStateToMainState;
    }

    private void ChangeStateToMainState()
    {
        if (isStaffBuyed) return;

        isStaffBuyed = true;

        if (timer != null) Coroutines.Stop(timer);

        timer = Timer();
        Coroutines.Start(timer);
    }

    private IEnumerator Timer()
    {
        _sceneRoot.CloseSelectStaffPanel();
        _sceneRoot.CloseAvatarBalancePanel();
        _sceneRoot.CloseBlackBackgroundPanel();
        _filterShopCasinoStaffActivatorProvider.ClearFailText();

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 6; i++)
        {
            _touchCameraProvider.SetPosition($"Manager_{i}", 1f);

            yield return new WaitForSeconds(2f);
        }

        _sceneRoot.OpenBlackBackgroundPanel();

        yield return new WaitForSeconds(0.3f);

        _tutorialMaskotProvider.Activate();

        yield return new WaitForSeconds(0.3f);

        _tutorialDialogueProvider.SetMessage("tutorial.staff.04", 2f);

        yield return new WaitForSeconds(2.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.staff.05", 3f);

        yield return new WaitForSeconds(4.5f);

        _administratorVisualProvider.Deactivate();

        ChangeStateToTutorial05_Start();
    }

    private void ChangeStateToTutorial05_Start()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial05_Leaderboard_Start_State_Game>());
    }
}
