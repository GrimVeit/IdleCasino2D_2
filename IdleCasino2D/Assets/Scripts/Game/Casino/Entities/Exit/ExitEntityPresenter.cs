using System;

public class ExitEntityPresenter : ICasinoEntityInfo, ICasinoEntityVisitorTraffic
{
    private readonly ExitEntityModel _model;

    public ExitEntityPresenter(ExitEntityModel model)
    {
        _model = model;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    public void ActivateEntityInteractive()
    {

    }

    public void DeactivateEntityInteractive()
    {

    }

    public void SetDealer(IDealer newDealer)
    {

    }

    #region VISITOR TRAFFIC

    public event Action<IVisitor> OnVisitorRealised
    {
        add => _model.OnVisitorRealised += value;
        remove => _model.OnVisitorRealised -= value;
    }

    public void AddVisitor(IVisitor visitor) => _model.AddVisitor(visitor);

    public void RemoveVisitor(IVisitor visitor) => _model.RemoveVisitor(visitor);

    #endregion

    #region INFO
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Exit;
    public bool IsOpen => true;
    public bool CanJoin => true;
    public bool IsGameRunning => false;

    #endregion
}
