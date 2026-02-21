using System;

public class ExitEntityPresenter : ICasinoEntity
{
    private readonly ExitEntityModel _model;

    public ExitEntityPresenter(ExitEntityModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnVisitorRealised += RealiseVisitor;
    }

    private void DeactivateEvents()
    {
        _model.OnVisitorRealised -= RealiseVisitor;
    }

    public void AddVisitor(IVisitor visitor) => _model.AddVisitor(visitor);

    public void RemoveVisitor(IVisitor visitor) => _model.RemoveVisitor(visitor);

    public void ActivateManualInteractive()
    {

    }

    public void DeactivateManualInteractive()
    {

    }

    public void SetDealer(IDealer newDealer)
    {

    }

    #region Output

    public event Action<IVisitor, ICasinoEntity> OnVisitorRealised;

    private void RealiseVisitor(IVisitor visitor)
    {
        OnVisitorRealised?.Invoke(visitor, this);
    }

    #endregion

    #region Input
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Exit;

    public int MaxSeats => _model.MaxSeats;

    public int OccupiedSeats => _model.OccupiedSeats;

    public bool HasFreeSeats => _model.HasFreeSeats;

    public bool CanJoin => _model.CanJoin;

    #endregion
}
