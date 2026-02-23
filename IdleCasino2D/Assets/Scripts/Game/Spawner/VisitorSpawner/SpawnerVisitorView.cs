using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVisitorView : View
{
    [SerializeField] private VisitorView visitorPrefab;
    [SerializeField] private Transform transformParentVisitors;
    [SerializeField] private Transform traspawnPoint;

    private readonly Dictionary<IVisitor, VisitorView> visitorViews = new();

    public void SpawnVisitor(List<CasinoEntityType> route)
    {
        var view = Instantiate(visitorPrefab, transformParentVisitors);
        view.transform.SetLocalPositionAndRotation(traspawnPoint.localPosition, visitorPrefab.transform.rotation);
        var presenter = new VisitorPresenter(new VisitorModel(route), view);
        presenter.Initialize();

        visitorViews.Add(presenter, view);

        OnSpawnVisitor?.Invoke(presenter);
    }

    public void DestroyVisitor(IVisitor visitor)
    {
        if (!visitorViews.ContainsKey(visitor))
        {
            Debug.LogWarning("Not found visitor");
            return;
        }

        OnDestroyVisitor?.Invoke(visitor);

        var view = visitorViews[visitor];
        visitorViews.Remove(visitor);
        Destroy(view.gameObject);
    }

    #region Output

    public event Action<IVisitor> OnSpawnVisitor;
    public event Action<IVisitor> OnDestroyVisitor;

    #endregion
}
