using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteEntityModel
{
    public bool IsOpen => isOpen;
    public bool IsGameRunning => isGameRunning;
    public bool CanJoin => isOpen && visitors.Count < 1;
    public int CountStaff => _dealer != null ? 1 : 0;

    private readonly Node _nodePlaceVisitor;
    private readonly Node _nodePlaceStaff;
    private readonly List<IVisitor> visitors = new();

    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;
    private readonly IGameSpot _rouletteSpot;
    private IDealer _dealer;
    private IEnumerator gameRoutine;

    private bool isOpen = false;
    private bool isGameRunning = false;
    private bool isVisitorReady;   // äîø¸ë ëè äî ñòîëà
    private bool isEntityInteractive = true;

    public RouletteEntityModel(ICasinoProfitStoreInfo casinoProfitStoreInfo, IGameSpot gameSpot, Node nodePlaceVisitor, Node nodePlaceStaff)
    {
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _rouletteSpot = gameSpot;
        _nodePlaceVisitor = nodePlaceVisitor;
        _nodePlaceStaff = nodePlaceStaff;
    }

    public void Initialize()
    {
        _rouletteSpot.OnClick += SpotClick;
    }

    public void Dispose()
    {
        _rouletteSpot.OnClick -= SpotClick;
    }

    public void SetStaff(IStaff newDealer)
    {
        _dealer = newDealer as IDealer;
        _dealer.SetMove(_nodePlaceStaff);
        _dealer.Show();
        _dealer.ActivateAnimation(DealerAnimationEnum.Idle);
        _dealer.ActivateNpcRotation(NpcRotationEnum.FrontRight);

        if (visitors.Count > 0)
            TryStartGame(visitors[0], auto: true);
    }

    #region Gameplay

    private void OnVisitorDestination(INpc npc, Node node)
    {
        npc.ActivateNpcRotation(node.RotationEnum);
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

        _dealer?.ActivateAnimation(DealerAnimationEnum.Game);
        _rouletteSpot.ActivateAnimation("game");
        visitor.ActivatePlay();

        yield return new WaitForSeconds(5f);

        _dealer?.ActivateAnimation(DealerAnimationEnum.Idle);
        _rouletteSpot.ActivateAnimation("idle");
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

        TryStartGame(visitors[0], auto: false);
    }

    #endregion

    #region MAIN ACTIVATOR

    public void Open()
    {
        isOpen = true;
        _rouletteSpot.ActivateAnimation("idle");
    }

    public void Close()
    {
        isOpen = false;
        _rouletteSpot.ActivateAnimation("not open");
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
        visitor.MoveTo(_nodePlaceVisitor, false);

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
        Debug.Log("CLICK ENTITY");

        if (!isEntityInteractive) return;

        Debug.Log("CLICK ENTITY");

        OnSpotClick?.Invoke();
    }

    #endregion

    #region HIGHLIGHT

    public void ActivateHighlight()
    {
        _rouletteSpot.ActivateHightlight();
    }

    public void DeactivateHighlight()
    {
        _rouletteSpot.DeactivateHighlight();
    }

    #endregion
}
