using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HostessEntityModel
{
    private readonly List<CasinoEntityHostessTargetAdapter> _casinoEntities = new();
    private readonly List<CasinoEntityEntranceQueueAdapter> _casinoEntityEntrances = new();
    private readonly List<(IVisitor, ICasinoEntityVisitorTraffic)> _firstVisitors = new();
    private readonly IVisitorPathTrafficProvider _visitorPathTrafficProvider;

    private (IVisitor visitor, ICasinoEntityVisitorTraffic casinoTraffic) _currentVisitor;

    private bool isActiveCasinoEntityInteractive = false;
    private bool isActiveEntranceQueueInteractive = false;

    public HostessEntityModel(List<ICasinoEntityInfo> casinoEntities, IVisitorPathTrafficProvider visitorPathTrafficProvider)
    {
        _visitorPathTrafficProvider = visitorPathTrafficProvider;

        foreach (var entity in casinoEntities)
        {
            if (entity is ICasinoEntitySpotClickListener clickListener &&
                entity is ICasinoEntityInteractiveProvider interactiveProvider &&
                entity is ICasinoEntityVisitorTraffic visitorTraffic &&
                entity.CasinoEntityType != CasinoEntityType.EntranceQueue)
            {
                var dto = new CasinoEntityHostessTargetAdapter(entity, clickListener, interactiveProvider, visitorTraffic);
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
                };
            }
        }
    }

    public void LeaveVisitor()
    {
        _firstVisitors.Remove(_currentVisitor);

        _currentVisitor.casinoTraffic.RemoveVisitor(_currentVisitor.visitor);
        _visitorPathTrafficProvider.TryAssignLeave(_currentVisitor.visitor);
        _currentVisitor = (null, null);

        ActivateAll();

        OnLeave?.Invoke();
    }

    private void OnQueueVisitorClick(IVisitor visitor, CasinoEntityEntranceQueueAdapter queueAdapter)
    {
        if(!isActiveEntranceQueueInteractive) return;

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
        if (!isActiveCasinoEntityInteractive) return;

        if (!dto.CasinoEntityInfo.IsOpen || !dto.CasinoEntityInfo.CanJoin) return;

        _currentVisitor.casinoTraffic.RemoveVisitor(_currentVisitor.visitor);
        _currentVisitor.visitor.SetNextStep();
        _visitorPathTrafficProvider.TryAssign(_currentVisitor.visitor, dto.CasinoEntityVisitorTraffic);
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
