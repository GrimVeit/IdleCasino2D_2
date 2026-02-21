using UnityEngine;

public class TypeTextEffectPresenter
{
    private TypeTextEffectModel model;
    private TypeTextEffectView view;

    public TypeTextEffectPresenter(TypeTextEffectModel model, TypeTextEffectView view)
    {
        this.model = model;
        this.view = view;
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

    }

    private void DeactivateEvents()
    {
        
    }
}
