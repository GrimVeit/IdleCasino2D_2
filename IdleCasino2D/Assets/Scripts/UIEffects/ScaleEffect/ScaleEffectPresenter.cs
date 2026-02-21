using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEffectPresenter
{
    private ScaleEffectModel model;
    private ScaleEffectView view;

    public ScaleEffectPresenter(ScaleEffectModel model, ScaleEffectView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        view.Dispose();
    }

    private void ActivateEvents()
    {

    }

    private void DeactivateEvents()
    {

    }
}
