using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HostessEntityModel
{
    public int CountStaff => _hostess != null ? 1 : 0;

    private readonly List<CasinoEntityHostessTargetAdapter> _casinoEntities = new();
    private readonly List<CasinoEntityEntranceQueueAdapter> _casinoEntityEntrances = new();
    private readonly List<(IVisitor, ICasinoEntityVisitorTraffic)> _firstVisitors = new();
    private readonly IVisitorPathTrafficProvider _visitorPathTrafficProvider;

    private IHostess _hostess;
    private readonly Node _nodeMain;
    private readonly Node _nodeMainBack;
    private readonly List<Node> _nodesTarget = new();

    private (IVisitor visitor, ICasinoEntityVisitorTraffic casinoTraffic) _currentVisitor;

    private bool isActiveCasinoEntityInteractive = false;
    private bool isActiveEntranceQueueInteractive = false;

    private bool isHostessInBase = true;

    public HostessEntityModel(List<ICasinoEntityInfo> casinoEntities, IVisitorPathTrafficProvider visitorPathTrafficProvider, List<Node> nodesTarget)
    {
        _visitorPathTrafficProvider = visitorPathTrafficProvider;

        for (int i = 0; i < casinoEntities.Count; i++)
        {
            var entity = casinoEntities[i];

            if (entity is ICasinoEntitySpotClickListener clickListener &&
                entity is ICasinoEntityInteractiveProvider interactiveProvider &&
                entity is ICasinoEntityVisitorTraffic visitorTraffic &&
                entity.CasinoEntityType != CasinoEntityType.EntranceQueue)
            {
                var dto = new CasinoEntityHostessTargetAdapter(i, entity, clickListener, interactiveProvider, visitorTraffic);
                dto.OnCasinoSpotClicked += OnSpotClick;
                dto.Initialize();
                _casinoEntities.Add(dto);
            }



            if (entity is ICasinoEntityVisitorTraffic traffic &&
                entity is ICasinoEntityVisitorClickListener visitorClickListener &&
                entity.CasinoEntityType == CasinoEntityType.EntranceQueue)
            {
                var dto = new CasinoEntityEntranceQueueAdapter(entity, traffic, visitorClickListener);
                dto.OnCasinoSpotClicked += OnQueueVisitorClick;
                dto.Initialize();
                _casinoEntityEntrances.Add(dto);

                traffic.OnVisitorRealised += visitor =>
                {
                    if (!_firstVisitors.Contains((visitor, traffic)))
                        _firstVisitors.Add((visitor, traffic));

                    if(_hostess != null && isHostessInBase)
                    {
                        TryAssignVisitorAuto();
                    }
                };
            }
        }

        for (int i = 0; i < nodesTarget.Count; i++)
        {
            if(i == 0)
                _nodeMain = nodesTarget[i];
            else if(i == 1)
                _nodeMainBack = nodesTarget[i];
            else
                _nodesTarget.Add(nodesTarget[i]);
        }

        Debug.Log(_nodesTarget.Count);
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {
        if (_hostess != null)
            _hostess.OnEndDestination -= HostessEndDestination;
    }

    private void HostessEndDestination(INpc hostess, Node node)
    {
        if (node == _nodeMainBack)
        {
            isHostessInBase = true;

            _hostess.SetMove(_nodeMain);
            _hostess.ActivateNpcRotation(NpcRotationEnum.FrontLeft);

            TryAssignVisitorAuto();
        }
        else
        {
            _hostess.ActivateAnimation(HostessAnimationEnum.WaveHand);
            _hostess.ActivateNpcRotation(NpcRotationEnum.FrontRight);

            Coroutines.Start(ReturnToMainNode());
        }
    }

    private IEnumerator ReturnToMainNode()
    {
        yield return new WaitForSeconds(2f);

        _hostess?.MoveTo(_nodeMainBack, false);
    }

    private void TryAssignVisitorAuto()
    {
        if (_firstVisitors.Count == 0 || _hostess == null || !isHostessInBase)
            return;

        _currentVisitor = _firstVisitors[0];

        var nextTarget = _currentVisitor.visitor.SecondTarget;

        var candidates = _casinoEntities
            .Where(s => s.CasinoEntityInfo.CasinoEntityType == nextTarget &&
                        s.CasinoEntityInfo.IsOpen &&
                        s.CasinoEntityInfo.CanJoin)
            .ToList();

        if (candidates.Count == 0)
        {
            _firstVisitors.Remove(_currentVisitor);

            _currentVisitor.casinoTraffic.RemoveVisitor(_currentVisitor.visitor);
            _visitorPathTrafficProvider.TryAssignLeave(_currentVisitor.visitor);
            _currentVisitor = (null, null);
            return;
        }

        var chosen = candidates[UnityEngine.Random.Range(0, candidates.Count)];

        Coroutines.Start(DelayedAssign(chosen));
    }

    private IEnumerator DelayedAssign(CasinoEntityHostessTargetAdapter chosenEntity)
    {
        yield return new WaitForSeconds(1.5f);

        var targetNode = _nodesTarget[_casinoEntities.IndexOf(chosenEntity)];

        Debug.Log(targetNode.gameObject.name);
        Debug.Log("??????????????????????????????????????????????????????");
        Debug.Log("??????????????????????????????????????????????????????");
        Debug.Log("??????????????????????????????????????????????????????");

        _hostess.MoveTo(targetNode, false);

        isHostessInBase = false;

        yield return new WaitForSeconds(1.5f);

        _currentVisitor.casinoTraffic.RemoveVisitor(_currentVisitor.visitor);
        _currentVisitor.visitor.SetNextStep();
        _visitorPathTrafficProvider.TryAssign(_currentVisitor.visitor, chosenEntity.CasinoEntityVisitorTraffic);
        _firstVisitors.Remove(_currentVisitor);
        _currentVisitor = (null, null);
    }



    public void SetStaff(IStaff staff)
    {
        _hostess = staff as IHostess;
        _hostess.OnEndDestination += HostessEndDestination;
        _hostess.SetMove(_nodeMain);
        _hostess.Show();
        _hostess.ActivateAnimation(HostessAnimationEnum.Idle);
        _hostess.ActivateNpcRotation(NpcRotationEnum.FrontLeft);

        TryAssignVisitorAuto();
    }

    #region MANUAL

    public void LeaveVisitor()
    {
        _firstVisitors.Remove(_currentVisitor);

        _currentVisitor.casinoTraffic.RemoveVisitor(_currentVisitor.visitor);
        _visitorPathTrafficProvider.TryAssignLeave(_currentVisitor.visitor);
        _firstVisitors.Remove(_currentVisitor);
        _currentVisitor = (null, null);

        ActivateAll();

        OnLeave?.Invoke();
    }

    private void OnQueueVisitorClick(IVisitor visitor, CasinoEntityEntranceQueueAdapter queueAdapter)
    {
        if(!isActiveEntranceQueueInteractive || _hostess != null) return;

        if (!_firstVisitors.Any(d => d.Item1 == visitor)) return;

        _currentVisitor = _firstVisitors.FirstOrDefault(t => t.Item1 == visitor);

        DeactivateAll();

        var nextTarget = _currentVisitor.visitor.SecondTarget;

        var candidates = _casinoEntities
            .Where(s => s.CasinoEntityInfo.CasinoEntityType == nextTarget &&
                        s.CasinoEntityInfo.IsOpen &&
                        s.CasinoEntityInfo.CanJoin)
            .ToList();

        foreach (var c in candidates)
            c.CasinoEntityInteractiveProvider?.ActivateEntityInteractive();

        OnHostessOpenChoose?.Invoke(nextTarget);
    }

    private void OnSpotClick(CasinoEntityHostessTargetAdapter dto)
    {
        if (!isActiveCasinoEntityInteractive || _hostess != null) return;

        if (!dto.CasinoEntityInfo.IsOpen || !dto.CasinoEntityInfo.CanJoin) return;

        _currentVisitor.casinoTraffic.RemoveVisitor(_currentVisitor.visitor);
        _currentVisitor.visitor.SetNextStep();
        _visitorPathTrafficProvider.TryAssign(_currentVisitor.visitor, dto.CasinoEntityVisitorTraffic);
        _firstVisitors.Remove(_currentVisitor);
        _currentVisitor = (null, null);

        ActivateAll();

        OnSuccessAssign?.Invoke();
    }

    public void ActivateInteractiveCasinoEntity()
    {
        isActiveCasinoEntityInteractive = true;
    }

    public void DeactivateInteractiveCasinoEntity()
    {
        isActiveCasinoEntityInteractive = false;
    }



    public void ActivateEntranceQueueInteractive()
    {
        isActiveEntranceQueueInteractive = true;
    }

    public void DeactivateEntranceQueueInteractive()
    {
        isActiveEntranceQueueInteractive = false;
    }


    public void ActivateAll()
    {
        foreach (var d in _casinoEntities)
            d.CasinoEntityInteractiveProvider?.ActivateEntityInteractive();
    }

    private void DeactivateAll()
    {
        foreach (var d in _casinoEntities)
            d.CasinoEntityInteractiveProvider?.DeactivateEntityInteractive();
    }

    #endregion

    #region Output

    public event Action<CasinoEntityType?> OnHostessOpenChoose;
    public event Action OnSuccessAssign;
    public event Action OnLeave;

    #endregion
}

public class CasinoEntityHostessTargetAdapter
{
    public ICasinoEntityInfo CasinoEntityInfo { get; }
    public ICasinoEntitySpotClickListener SpotClickListener { get; }
    public ICasinoEntityVisitorTraffic CasinoEntityVisitorTraffic { get; }
    public ICasinoEntityInteractiveProvider CasinoEntityInteractiveProvider { get; }

    public event Action<CasinoEntityHostessTargetAdapter> OnCasinoSpotClicked;

    public CasinoEntityHostessTargetAdapter(
        int numberId,
        ICasinoEntityInfo casinoEntityInfo,
        ICasinoEntitySpotClickListener casinoEntitySpotClick,
        ICasinoEntityInteractiveProvider casinoEntityInteractiveProvider,
        ICasinoEntityVisitorTraffic casinoEntityVisitorTraffic)
    {
        CasinoEntityInfo = casinoEntityInfo;
        SpotClickListener = casinoEntitySpotClick;
        CasinoEntityInteractiveProvider = casinoEntityInteractiveProvider;
        CasinoEntityVisitorTraffic = casinoEntityVisitorTraffic;
    }

    private void CasinoSpotClicked()
    {
        OnCasinoSpotClicked?.Invoke(this);
    }

    public void Initialize()
    {
        if (SpotClickListener != null)
            SpotClickListener.OnSpotClick += CasinoSpotClicked;
    }

    public void Dispose()
    {
        if (SpotClickListener != null)
            SpotClickListener.OnSpotClick -= CasinoSpotClicked;
    }
}

public class CasinoEntityEntranceQueueAdapter
{
    public ICasinoEntityInfo CasinoEntityInfo { get; }
    public ICasinoEntityVisitorTraffic CasinoEntityVisitorTraffic { get; }
    public ICasinoEntityVisitorClickListener CasinoEntityVisitorClickListener { get; }

    public event Action<IVisitor, CasinoEntityEntranceQueueAdapter> OnCasinoSpotClicked;

    public CasinoEntityEntranceQueueAdapter(
        ICasinoEntityInfo casinoEntityInfo,
        ICasinoEntityVisitorTraffic casinoEntityVisitorTraffic,
        ICasinoEntityVisitorClickListener casinoEntityVisitorClickListener)
    {
        CasinoEntityInfo = casinoEntityInfo;
        CasinoEntityVisitorTraffic = casinoEntityVisitorTraffic;
        CasinoEntityVisitorClickListener = casinoEntityVisitorClickListener;
    }

    private void CasinoSpotClicked(IVisitor visitor)
    {
        OnCasinoSpotClicked?.Invoke(visitor, this);
    }

    public void Initialize()
    {
        CasinoEntityVisitorClickListener.OnVisitorClick += CasinoSpotClicked;
    }

    public void Dispose()
    {
        CasinoEntityVisitorClickListener.OnVisitorClick -= CasinoSpotClicked;
    }
}
