using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tutorial02_FirstTables_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly ITutorialDialogueProvider _tutorialDialogueProvider;
    private readonly ITutorialMaskotProvider _tutorialMaskotProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly ITouchCameraProvider _touchCameraProvider;
    private readonly List<ICasinoEntityInfo> _casinoEntities;

    private IEnumerator timer;

    public Tutorial02_FirstTables_State_Game(IStateMachineProvider machineProvider, ITutorialDialogueProvider tutorialDialogueProvider, ITutorialMaskotProvider tutorialMaskotProvider, UIGameRoot sceneRoot, ITouchCameraProvider touchCameraProvider, List<ICasinoEntityInfo> casinoEntities)
    {
        _machineProvider = machineProvider;
        _tutorialDialogueProvider = tutorialDialogueProvider;
        _tutorialMaskotProvider = tutorialMaskotProvider;
        _sceneRoot = sceneRoot;
        _touchCameraProvider = touchCameraProvider;
        _casinoEntities = casinoEntities;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - TUTORIAL 02 FIRST TABLES / GAME</color>");

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
        _tutorialDialogueProvider.SetMessage("tutorial.tables.01", 2f);

        yield return new WaitForSeconds(2.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.tables.02", 3f);

        yield return new WaitForSeconds(3.5f);

        _tutorialDialogueProvider.SetMessage("tutorial.tables.03", 3.5f);

        yield return new WaitForSeconds(4f);

        _tutorialMaskotProvider.Deactivate();
        _sceneRoot.CloseBlackBackgroundPanel();

        _touchCameraProvider.SetPosition("First_Slot");

        yield return new WaitForSeconds(1f);

        (_casinoEntities.FirstOrDefault(x => x.CasinoEntityType == CasinoEntityType.Slot) as ICasinoEntityActivator)?.Open();

        yield return new WaitForSeconds(2f);

        _touchCameraProvider.SetPosition("First_Wheel");

        yield return new WaitForSeconds(1f);

        (_casinoEntities.FirstOrDefault(x => x.CasinoEntityType == CasinoEntityType.Wheel) as ICasinoEntityActivator)?.Open();

        yield return new WaitForSeconds(2f);

        _sceneRoot.OpenBlackBackgroundPanel();

        yield return new WaitForSeconds(0.3f);

        _tutorialMaskotProvider.Activate();

        yield return new WaitForSeconds(0.3f);

        _tutorialDialogueProvider.SetMessage("tutorial.tables.04", 2f);

        yield return new WaitForSeconds(3.5f);

    }
}
