using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class VisitorPathTrafficModel : IDisposable
{
    private readonly CasinoEntityPathTrafficVisitorAdapter _exitEntity;
    private readonly List<CasinoEntityPathTrafficVisitorAdapter> _casinoEntities = new();

    private readonly ISpawnerVisitorProvider _spawnerVisitorProvider;
    private readonly ISpawnerVisitorListener _spawnerVisitorListener;

    public VisitorPathTrafficModel(
        List<ICasinoEntityInfo> casinoEntities,
        ISpawnerVisitorProvider spawnerVisitorProvider,
        ISpawnerVisitorListener spawnerVisitorListener)
    {
        foreach (var entity in casinoEntities)
        {
            if (entity is ICasinoEntityVisitorTraffic traffic)
            {
                var dto = new CasinoEntityPathTrafficVisitorAdapter(entity, traffic);
                dto.OnVisitorRealised += OnEntityVisitorRealised;
                dto.Initialize();

                _casinoEntities.Add(dto);
            }
        }

        _exitEntity = _casinoEntities
            .FirstOrDefault(e => e.CasinoEntityType == CasinoEntityType.Exit)
            ?? throw new Exception("Exit entity not found in traffic system.");

        _spawnerVisitorProvider = spawnerVisitorProvider;
        _spawnerVisitorListener = spawnerVisitorListener;

        _spawnerVisitorListener.OnAddVisitor += TryAssign;
    }

    private void OnEntityVisitorRealised(
        IVisitor visitor,
        CasinoEntityPathTrafficVisitorAdapter dto)
    {
        dto.CasinoEntityVisitorTraffic.RemoveVisitor(visitor);

        if (dto.CasinoEntityType == CasinoEntityType.Exit)
        {
            _spawnerVisitorProvider.DestroyVisitor(visitor);
            return;
        }

        if (visitor.MoveNextStep())
        {
            TryAssign(visitor);
        }
        else
        {
            _exitEntity.CasinoEntityVisitorTraffic.AddVisitor(visitor);
        }
    }

    private void TryAssign(IVisitor visitor)
    {
        var targetType = visitor.CurrentTarget;

        var candidates = _casinoEntities
            .Where(e => e.CasinoEntityType == targetType && e.CanJoin)
            .ToList();

        if (candidates.Count > 0)
        {
            var target = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            target.CasinoEntityVisitorTraffic.AddVisitor(visitor);
        }
        else
        {
            _exitEntity.CasinoEntityVisitorTraffic.AddVisitor(visitor);
        }
    }

    public void Dispose()
    {
        foreach (var dto in _casinoEntities)
        {
            dto.OnVisitorRealised -= OnEntityVisitorRealised;
            dto.Dispose();
        }

        _spawnerVisitorListener.OnAddVisitor -= TryAssign;
    }
}

public class CasinoEntityPathTrafficVisitorAdapter
{
    public ICasinoEntityInfo CasinoEntityInfo { get; }
    public ICasinoEntityVisitorTraffic CasinoEntityVisitorTraffic { get; }

    public CasinoEntityType CasinoEntityType => CasinoEntityInfo.CasinoEntityType;
    public bool CanJoin => CasinoEntityInfo.CanJoin;

    public event Action<IVisitor, CasinoEntityPathTrafficVisitorAdapter> OnVisitorRealised;

    public CasinoEntityPathTrafficVisitorAdapter(
        ICasinoEntityInfo casinoEntityInfo,
        ICasinoEntityVisitorTraffic casinoEntityVisitorTraffic)
    {
        CasinoEntityInfo = casinoEntityInfo;
        CasinoEntityVisitorTraffic = casinoEntityVisitorTraffic;
    }

    private void HandleVisitorRealised(IVisitor visitor)
    {
        OnVisitorRealised?.Invoke(visitor, this);
    }

    public void Initialize()
    {
        CasinoEntityVisitorTraffic.OnVisitorRealised += HandleVisitorRealised;
    }

    public void Dispose()
    {
        CasinoEntityVisitorTraffic.OnVisitorRealised -= HandleVisitorRealised;
    }
}
