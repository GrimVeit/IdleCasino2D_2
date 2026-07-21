using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMaskotPresenter
{
    private readonly TutorialMaskotView _view;

    public TutorialMaskotPresenter(TutorialMaskotView view)
    {
        _view = view;
    }

    public void Activate() => _view.Activate();
    public void Deactivate() => _view.Deactivate();
}

public interface ITutorialMaskotProvider
{
    public void Activate();
    public void Deactivate();
}
