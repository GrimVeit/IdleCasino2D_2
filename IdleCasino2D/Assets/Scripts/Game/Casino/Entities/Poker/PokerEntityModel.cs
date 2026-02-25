using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PokerEntityModel
{
    public event Action<IVisitor> OnVisitorRealised;

    public bool IsGameRunning => isGameRunning;
    public bool CanJoin => isOpen && visitors.Count < 1;

    private readonly Node _nodePlace;
    private readonly List<IVisitor> visitors = new();
    public bool HasDealer() => _dealer != null;
    public bool ContainsVisitor(IVisitor visitor) => visitors.Contains(visitor);

    private readonly IGameSpot _pokerSpot;
    private IDealer _dealer;
    private IEnumerator gameRoutine;

    private bool isOpen = true;
    private bool isGameRunning = false;
    private bool isVisitorReady;   // äîø¸ë ëè äî ñòîëà
    private bool isManualInteractive = true;

    public PokerEntityModel(IGameSpot gameSpot, Node node)
    {
        _pokerSpot = gameSpot;
        _nodePlace = node;
    }

    public void Initialize()
    {
        _pokerSpot.OnClick += ManualStartGame;
    }

    public void Dispose()
    {
        _pokerSpot.OnClick -= ManualStartGame;
    }

    #region Gameplay

    private void OnVisitorDestination(INpc npc, Node node)
    {
        npc.ActivateNpcRotation(NpcRotationEnum.BackRight);
        isVisitorReady = true;

        if (_dealer != null)
        {
            TryStartGame(npc as IVisitor, auto: true);
        }
        else
        {
            //ÄÈËÅÐÀ ÍÅÒ, ÍÓÆÍÀ ËÎÃÈÊÀ ÏÎÊÀÇÀ ÎÆÈÄÀÍÈß ÑÒÀÐÒÀ ×ÒÎÁÛ ÈÃÐÎÊ ÊËÈÊÀË
        }
    }

    private void ManualStartGame()
    {
        if (!isManualInteractive || visitors.Count == 0)
            return;

        TryStartGame(visitors[0], auto: false);
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

        if (!auto && _dealer != null) // çàùèòà îò ðó÷íîãî çàïóñêà ïðè äèëåðå
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

        _dealer?.SetPlay();
        _pokerSpot.ActivateAnimation("game");
        visitor.ActivatePlay();

        yield return new WaitForSeconds(5f);

        _dealer?.SetIdle();
        _pokerSpot.ActivateAnimation("idle");
        visitor.ActivateWin();
        OnAddCoins?.Invoke(visitor.Position, 10);

        yield return new WaitForSeconds(1f);

        visitor.ActivateIdle();

        yield return new WaitForSeconds(0.5f);

        isGameRunning = false;
        isVisitorReady = false;

        OnVisitorRealised?.Invoke(visitor);
    }

    #endregion

    #region CONTROLLER

    public void ActivateManualInteractive() => isManualInteractive = true;
    public void DeactivateManualInteractive() => isManualInteractive = false;

    public void SetDealer(IDealer newDealer)
    {
        _dealer = newDealer;
        _dealer.SetIdle();
    }

    public void OpenEntity()
    {
        isOpen = true;
        _pokerSpot.ActivateAnimation("idle");
    }

    public void CloseEntity()
    {
        isOpen = false;
        _pokerSpot.ActivateAnimation("not open");
    }

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
}

