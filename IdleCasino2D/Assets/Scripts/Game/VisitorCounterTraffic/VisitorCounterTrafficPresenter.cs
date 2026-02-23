using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCounterTrafficPresenter : IVisitorCounterTrafficProvider
{
    private readonly VisitorCounterTrafficModel _model;

    public VisitorCounterTrafficPresenter(VisitorCounterTrafficModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    public void PlayTraffic() => _model.PlayTraffic();
    public void StopTraffic() => _model.StopTraffic();
}

public interface IVisitorCounterTrafficProvider
{
    public void PlayTraffic();
    public void StopTraffic();
}
