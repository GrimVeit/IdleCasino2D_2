using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WheelEntityModel
{
    public bool IsOpen => isOpen;
    public bool CanJoin => isOpen && visitors.Count < 1;
    public bool IsGameRunning => isGameRunning;

    private readonly Node _nodePlace;
    private readonly List<IVisitor> visitors = new();
    public bool ContainsVisitor(IVisitor visitor) => visitors.Contains(visitor);

    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;
    private readonly IGameSpot _wheelSpot;
    private IEnumerator gameRoutine;

    private bool isOpen = false;
    private bool isGameRunning;
    private bool isVisitorReady;   // дошёл ли до стола
    private bool isManualInteractive;
    private bool isEntityInteractive = true;

    public WheelEntityModel(ICasinoProfitStoreInfo casinoProfitStoreInfo, IGameSpot wheelSpot, Node node)
    {
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _wheelSpot = wheelSpot;
        _nodePlace = node;
    }

    public void Initialize()
    {
        _wheelSpot.OnClick += SpotClick;
    }

    public void Dispose()
    {
        _wheelSpot.OnClick -= SpotClick;
    }

    #region Gameplay

    private void OnVisitorDestination(INpc npc, Node node)
    {
        npc.ActivateNpcRotation(NpcRotationEnum.BackRight);
        isVisitorReady = true;

        TryStartGame(npc as IVisitor, auto: true);
    }

    private void TryStartGame(IVisitor visitor, bool auto)
    {
        if (!CanStartGame(visitor, auto))
            return;

        StartGame(visitor);
    }

    private bool CanStartGame(IVisitor visitor, bool auto)
    {
        if (isGameRunning)
            return false;

        if (visitor == null)
            return false;

        if (!isVisitorReady)
            return false;

        return true;
    }

    private void StartGame(IVisitor visitor)
    {
        if (gameRoutine != null)
            Coroutines.Stop(gameRoutine);

        gameRoutine = Game(visitor);
        Coroutines.Start(gameRoutine);
    }

    private IEnumerator Game(IVisitor visitor)
    {
        isGameRunning = true;

        _wheelSpot.ActivateAnimation("game");
        visitor.ActivatePlay();

        yield return new WaitForSeconds(4f);

        _wheelSpot.ActivateAnimation("idle");
        visitor.ActivateWin();
        OnAddCoins?.Invoke(visitor.Position, _casinoProfitStoreInfo.GetProfit(CasinoEntityType.Wheel));

        yield return new WaitForSeconds(1f);

        visitor.ActivateIdle();

        yield return new WaitForSeconds(0.5f);

        isGameRunning = false;
        isVisitorReady = false;

        OnVisitorRealised?.Invoke(visitor);
    }

    #endregion

    public void ActivateEntityInteractive() => isEntityInteractive = true;
    public void DeactivateEntityInteractive() => isEntityInteractive = false;


    #region MANUAL ACTIVATOR

    public void ActivateManualInteractive() => isManualInteractive = true;
    public void DeactivateManualInteractive() => isManualInteractive = false;

    #endregion

    #region MANUAL

    public void ManualStartGame()
    {
        if (!isManualInteractive)
            return;

        var visitor = visitors[0];
        TryStartGame(visitor, auto: false);
    }

    #endregion

    #region MAIN ACTIVATOR

    public void Open()
    {
        isOpen = true;
        _wheelSpot.ActivateAnimation("idle");
    }

    public void Close()
    {
        isOpen = false;
        _wheelSpot.ActivateAnimation("not open");
    }

    #endregion

    #region VISITOR TRAFFIC

    public event Action<IVisitor> OnVisitorRealised;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.Contains(visitor))
            return;

        visitors.Add(visitor);
        visitor.OnEndDestination += OnVisitorDestination;
        visitor.MoveTo(_nodePlace, false);

        isVisitorReady = false;
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.Contains(visitor))
            return;

        visitor.OnEndDestination -= OnVisitorDestination;
        visitors.Remove(visitor);

        isVisitorReady = false;
    }

    #endregion

    #region PROFIT

    public event Action<Vector3, int> OnAddCoins;

    #endregion

    #region SPOT CLICK

    public event Action OnSpotClick;

    private void SpotClick()
    {
        if(!isEntityInteractive) return;

        OnSpotClick?.Invoke();
    }

    #endregion
}