using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;
using Random = UnityEngine.Random;

public class SlotMachineEntityModel
{
    public bool IsOpen => isOpen;
    public bool CanJoin => isOpen && visitors.Count < 1;
    public bool IsGameRunning => isGameRunning;

    private readonly Node _nodePlace;
    private readonly Dictionary<IVisitor, VisitorState> visitors = new();


    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;
    private readonly IGameSpot _slotSpot;

    private IEnumerator messageRoutine;
    private IEnumerator gameRoutine;

    private bool isOpen = false;
    private bool isGameRunning = false;
    private bool isVisitorReady = false;   // дошёл ли до стола
    private bool isManualInteractive = false;
    private bool isEntityInteractive = true;

    public SlotMachineEntityModel(ICasinoProfitStoreInfo casinoProfitStoreInfo, IGameSpot slotSpot, Node node)
    {
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _slotSpot = slotSpot;
        _nodePlace = node;
    }

    public void Initialize()
    {
        _slotSpot.OnClick += SpotClick;

        if(messageRoutine != null ) Coroutines.Stop(messageRoutine);

        messageRoutine = SingleVisitorTalk();
        Coroutines.Start(messageRoutine);
    }

    public void Dispose()
    {
        _slotSpot.OnClick -= SpotClick;

        if (messageRoutine != null) Coroutines.Stop(messageRoutine);
    }

    #region Gameplay

    private void OnVisitorDestination(INpc npc, Node node)
    {
        var visitor = npc as IVisitor;

        visitor.ActivateNpcRotation(NpcRotationEnum.BackRight);
        visitors[visitor] = VisitorState.At;

        isVisitorReady = true;

        TryStartGame(visitor, auto: true);
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

        _slotSpot.ActivateAnimation("game");
        visitor.ActivatePlay();

        yield return new WaitForSeconds(5f);

        _slotSpot.ActivateAnimation("idle");
        visitor.ActivateWin();
        OnAddCoins?.Invoke(visitor.Position, _casinoProfitStoreInfo.GetProfit(CasinoEntityType.Slot));

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

        var visitor = visitors.Keys.First();
        TryStartGame(visitor, auto: false);
    }

    #endregion

    #region MAIN ACTIVATOR

    public void Open()
    {
        isOpen = true;
        _slotSpot.ActivateAnimation("idle");
    }

    public void Close()
    {
        isOpen = false;
        _slotSpot.ActivateAnimation("not open");
    }

    #endregion

    #region VISITOR TRAFFIC

    public event Action<IVisitor> OnVisitorRealised;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.ContainsKey(visitor))
            return;

        visitors.Add(visitor, VisitorState.GoTo);
        visitor.OnClick += VisitorClick;
        visitor.OnEndDestination += OnVisitorDestination;
        visitor.MoveTo(_nodePlace, false);

        isVisitorReady = false;
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.ContainsKey(visitor))
            return;

        visitor.OnClick -= VisitorClick;
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
        if (!isEntityInteractive) return;

        OnSpotClick?.Invoke();
    }

    #endregion

    #region HIGHLIGHT

    public void ActivateHighlight()
    {
        _slotSpot.ActivateHightlight();
    }

    public void DeactivateHighlight()
    {
        _slotSpot.DeactivateHighlight();
    }

    #endregion

    #region VISITOR CLICK

    private void VisitorClick(IVisitor visitor)
    {
        SetMessage(visitor);
    }

    #endregion

    #region MESSAGE

    private IEnumerator SingleVisitorTalk()
    {
        while (true)
        {
            if (visitors.Count == 0)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            IVisitor visitor = visitors.Keys.First();

            if (Random.value <= 0.7f)
            {
                SetMessage(visitor);
            }

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    private void SetMessage(IVisitor visitor)
    {
        if (!visitors.TryGetValue(visitor, out var state))
            return;

        switch (state)
        {
            case VisitorState.GoTo:
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.GoToSlot));
                break;
            case VisitorState.At:
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.PlayingSlot));
                break;
        }
    }

    #endregion
}