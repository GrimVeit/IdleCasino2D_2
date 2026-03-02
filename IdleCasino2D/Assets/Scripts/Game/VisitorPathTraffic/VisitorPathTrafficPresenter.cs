using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorPathTrafficPresenter : IVisitorPathTrafficProvider
{
    private readonly VisitorPathTrafficModel _model;

    public VisitorPathTrafficPresenter(VisitorPathTrafficModel model)
    {
        _model = model;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Input

    public void TryAssign(IVisitor visitor, ICasinoEntityVisitorTraffic casinoEntityVisitorTraffic) 
        => _model.TryAssign(visitor, casinoEntityVisitorTraffic);

    public void TryAssignLeave(IVisitor visitor)
        => _model.TryAssignVisitorLeave(visitor);

    #endregion
}

public interface IVisitorPathTrafficProvider
{
    public void TryAssign(IVisitor visitor, ICasinoEntityVisitorTraffic casinoEntityVisitorTraffic);
    public void TryAssignLeave(IVisitor visitor); 
}
