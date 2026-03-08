using System;

public class WheelSpotPresenter : IGameSpot
{
    private readonly WheelSpotModel _model;
    private readonly WheelSpotView _view;

    public WheelSpotPresenter(WheelSpotModel model, WheelSpotView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClick += _model.Click;

        _model.OnActivateAnimation += _view.SetAnimation;
    }

    private void DeactivateEvents()
    {
        _view.OnClick -= _model.Click;

        _model.OnActivateAnimation -= _view.SetAnimation;
    }

    #region Output

    public event Action OnClick
    {
        add => _model.OnClick += value;
        remove => _model.OnClick -= value;
    }

    #endregion

    #region Input
    public void ActivateAnimation(string name) => _model.ActivateAnimation(name);

    public void ActivateHightlight() => _view.ActivateHighlight();
    public void DeactivateHighlight() => _view.DeactivateHighlight();

    #endregion
}
