using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WheelEntityModel
{
    public event Action<IVisitor> OnVisitorRealised;

    public int MaxSeats => 1;
    public int OccupiedSeats => visitors.Count;
    public bool HasFreeSeats => visitors.Count < MaxSeats;
    public bool CanJoin => isOpen && HasFreeSeats;

    private readonly Node _nodePlace;
    private readonly List<IVisitor> visitors = new();
    public bool ContainsVisitor(IVisitor visitor) => visitors.Contains(visitor);

    private readonly IGameSpot _wheelSpot;
    private IEnumerator gameRoutine;

    private bool isOpen = true;
    private bool isGameRunning;
    private bool isVisitorReady;   // дошёл ли до стола
    private bool isManualInteractive;

    public WheelEntityModel(IGameSpot wheelSpot, Node node)
    {
        _wheelSpot = wheelSpot;
        _nodePlace = node;
    }

    public void Initialize()
    {
        _wheelSpot.OnClick += ManualStartGame;
    }

    public void Dispose()
    {
        _wheelSpot.OnClick -= ManualStartGame;
    }

    #region Gameplay

    private void OnVisitorDestination(INpc npc, Node node)
    {
        npc.ActivateNpcRotation(NpcRotationEnum.BackRight);
        isVisitorReady = true;

        TryStartGame(npc as IVisitor, auto: true);
    }

    private void ManualStartGame()
    {
        if (!isManualInteractive)
            return;

        var visitor = visitors[0];
        TryStartGame(visitor, auto: false);
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

    public void OpenEntity()
    {
        isOpen = true;
        _wheelSpot.ActivateAnimation("idle");
    }

    public void CloseEntity()
    {
        isOpen = false;
        _wheelSpot.ActivateAnimation("not open");
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
}