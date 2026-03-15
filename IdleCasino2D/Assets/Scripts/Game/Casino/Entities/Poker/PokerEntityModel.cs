using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;
using Random = UnityEngine.Random;

public class PokerEntityModel
{
    public bool IsOpen => isOpen;
    public bool IsGameRunning => isGameRunning;
    public bool CanJoin => isOpen && visitors.Count < 1;
    public int CountStaff => _dealer != null ? 1 : 0;

    private readonly Node _nodePlaceVisitor;
    private readonly Node _nodePlaceStaff;
    private readonly Dictionary<IVisitor, VisitorState> visitors = new();

    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;
    private readonly IGameSpot _pokerSpot;
    private IDealer _dealer;

    private IEnumerator messageRoutine;
    private IEnumerator gameRoutine;

    private bool isOpen = false;
    private bool isGameRunning = false;
    private bool isVisitorReady;   // дошёл ли до стола
    private bool isEntityInteractive = true;

    public PokerEntityModel(ICasinoProfitStoreInfo casinoProfitStoreInfo, IGameSpot gameSpot, Node nodePlaceVisitor, Node nodePlaceStaff)
    {
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _pokerSpot = gameSpot;
        _nodePlaceVisitor = nodePlaceVisitor;
        _nodePlaceStaff = nodePlaceStaff;
    }

    public void Initialize()
    {
        _pokerSpot.OnClick += SpotClick;

        if (messageRoutine != null) Coroutines.Stop(messageRoutine);

        messageRoutine = SingleVisitorTalk();
        Coroutines.Start(messageRoutine);
    }

    public void Dispose()
    {
        _pokerSpot.OnClick -= SpotClick;

        if (messageRoutine != null) Coroutines.Stop(messageRoutine);
    }

    public void SetStaff(IStaff newDealer)
    {
        _dealer = newDealer as IDealer;
        _dealer.SetMove(_nodePlaceStaff);
        _dealer.Show();
        _dealer.ActivateAnimation(DealerAnimationEnum.Idle);
        _dealer.ActivateNpcRotation(NpcRotationEnum.FrontLeft);

        if(visitors.Count > 0)
            TryStartGame(visitors.Keys.First(), auto: true);
    }

    #region Gameplay

    private void OnVisitorDestination(INpc npc, Node node)
    {
        var visitor = npc as IVisitor;

        visitor.ActivateNpcRotation(NpcRotationEnum.BackRight);
        visitors[visitor] = VisitorState.At;

        isVisitorReady = true;

        if (_dealer != null)
        {
            TryStartGame(visitor, auto: true);
        }
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

        if (!auto && _dealer != null) // защита от ручного запуска при дилере
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

        _dealer?.ActivateAnimation(DealerAnimationEnum.Game);
        _pokerSpot.ActivateAnimation("game");
        visitor.ActivatePlay();

        yield return new WaitForSeconds(5f);

        _dealer?.ActivateAnimation(DealerAnimationEnum.Idle);
        _pokerSpot.ActivateAnimation("idle");
        visitor.ActivateWin();
        OnAddCoins?.Invoke(visitor.Position, _casinoProfitStoreInfo.GetProfit(CasinoEntityType.Poker));

        yield return new WaitForSeconds(1f);

        visitor.ActivateIdle();

        yield return new WaitForSeconds(0.5f);

        isGameRunning = false;
        isVisitorReady = false;

        OnVisitorRealised?.Invoke(visitor);
    }

    #endregion

    #region MANUAL ACTIVATOR

    public void ActivateEntityInteractive() => isEntityInteractive = true;
    public void DeactivateEntityInteractive() => isEntityInteractive = false;

    #endregion

    #region MANUAL

    public void ManualStartGame()
    {
        if (!isEntityInteractive || visitors.Count == 0)
            return;

        TryStartGame(visitors.Keys.First(), auto: false);
    }

    #endregion

    #region MAIN ACTIVATOR

    public void Open()
    {
        isOpen = true;
        _pokerSpot.ActivateAnimation("idle");
    }

    public void Close()
    {
        isOpen = false;
        _pokerSpot.ActivateAnimation("not open");
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
        visitor.MoveTo(_nodePlaceVisitor, false);

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
        if(!isEntityInteractive) return;

        OnSpotClick?.Invoke();
    }

    #endregion

    #region HIGHLIGHT

    public void ActivateHighlight()
    {
        _pokerSpot.ActivateHightlight();
    }

    public void DeactivateHighlight()
    {
        _pokerSpot.DeactivateHighlight();
    }

    #endregion

    #region VISITOR CLICK

    private void VisitorClick(IVisitor visitor)
    {
        SetMessage(visitor);
    }

    #endregion

    #region VISITOR MESSAGE

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
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.GoToPoker));
                break;
            case VisitorState.At:
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.PlayingPoker));
                break;
        }
    }

    #endregion
}

