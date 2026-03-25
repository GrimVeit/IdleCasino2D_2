using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerVisitorView : View
{
    [SerializeField] private VisitorView visitorPrefab_1;
    [SerializeField] private VisitorView visitorPrefab_2;
    [SerializeField] private List<string> skinsId_1;
    [SerializeField] private List<string> skinsId_2;
    [SerializeField] private Transform transformParentVisitors;
    [SerializeField] private Transform traspawnPoint;

    private readonly Dictionary<IVisitor, VisitorView> visitorViews = new();

    public void SpawnVisitor(List<CasinoEntityType> route, ISoundProvider soundProvider)
    {
        VisitorView view;

        if (Random.value < 0.5f)
        {
            view = Instantiate(visitorPrefab_1, transformParentVisitors);
            view.SetSkin(skinsId_1[Random.Range(0, skinsId_1.Count)]);
        }
        else
        {
            view = Instantiate(visitorPrefab_2, transformParentVisitors);
            view.SetSkin(skinsId_2[Random.Range(0, skinsId_2.Count)]);
        }
        view.transform.SetLocalPositionAndRotation(traspawnPoint.localPosition, visitorPrefab_1.transform.rotation);

        var presenter = new VisitorPresenter(new VisitorModel(route, soundProvider), view);
        presenter.Initialize();
        presenter.Show();

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
        view.HideDestroy();
        visitorViews.Remove(visitor);
    }

    #region Output

    public event Action<IVisitor> OnSpawnVisitor;
    public event Action<IVisitor> OnDestroyVisitor;

    #endregion
}
