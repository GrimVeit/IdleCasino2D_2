using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorTrafficPresenter
{
    private readonly VisitorTrafficModel _model;

    public VisitorTrafficPresenter(VisitorTrafficModel model)
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

public interface IVisitorTrafficProvider
{
    public void PlayTraffic();
    public void StopTraffic();
}
