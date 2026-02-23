using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisitorPathTrafficModel
{
    private readonly ICasinoEntity _exitEntity;
    private readonly List<ICasinoEntity> _casinoEntities = new();
    private readonly ISpawnerVisitorProvider _spawnerVisitorProvider;
    private readonly ISpawnerVisitorListener _spawnerVisitorListener;

    public VisitorPathTrafficModel(List<ICasinoEntity> casinoEntities, ISpawnerVisitorProvider spawnerVisitorProvider, ISpawnerVisitorListener spawnerVisitorListener)
    {
        _casinoEntities = casinoEntities;
        _exitEntity = _casinoEntities.FirstOrDefault(data => data.CasinoEntityType == CasinoEntityType.Exit);
        _spawnerVisitorProvider = spawnerVisitorProvider;
        _spawnerVisitorListener = spawnerVisitorListener;

        _spawnerVisitorListener.OnAddVisitor += TryAssign;
    }

    public void Initialize()
    {
        _casinoEntities.ForEach(e => e.OnVisitorRealised += EntityRelease);
    }

    public void Dispose()
    {
        _casinoEntities.ForEach(e => e.OnVisitorRealised -= EntityRelease);

        _spawnerVisitorListener.OnAddVisitor -= TryAssign;
    }

    private void EntityRelease(IVisitor visitor, ICasinoEntity entity)
    {
        entity.RemoveVisitor(visitor);

        if (entity.CasinoEntityType == CasinoEntityType.Exit)
        {
            Debug.Log("Удаление персонажа");
            _spawnerVisitorProvider.DestroyVisitor(visitor);
            return;
        }

        if (visitor.MoveNextStep())
        {
            Debug.Log("Пытаемся дать новую цель");
            TryAssign(visitor);
        }
        else
        {
            Debug.Log("Целей больше нет, отправляем на удаление");
            _exitEntity.AddVisitor(visitor);
        }
    }

    private void TryAssign(IVisitor visitor)
    {
        var targetType = visitor.CurrentTarget;

        var entities = _casinoEntities
            .Where(e => e.CasinoEntityType == targetType && e.CanJoin)
            .OrderBy(e => e.OccupiedSeats).ToList();



        if (entities.Count != 0)
        {
            var entity = entities[Random.Range(0, entities.Count)];
            Debug.Log("Дали новую цель - " + targetType.ToString());
            entity.AddVisitor(visitor);
        }
        else
        {
            Debug.Log("Цель была занята, отправляем на удаление");
            _exitEntity.AddVisitor(visitor);
        }
    }
}
