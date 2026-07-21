using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialoguePresenter
{
    private readonly TutorialDialogueModel _model;
    private readonly TutorialDialogueView _view;

    public TutorialDialoguePresenter(TutorialDialogueModel model, TutorialDialogueView view)
    {
        _model = model;
        _view = view;
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
        _model.OnSetMessage += _view.SetMessage;
    }

    private void DeactivateEvents()
    {
        _model.OnSetMessage -= _view.SetMessage;
    }

    #region Input

    public void SetMessage(string id, float duration) => _model.SetMessage(id, duration);

    #endregion
}

public interface ITutorialDialogueProvider
{
    public void SetMessage(string id, float duration);
}
