using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCasinoPersonalPresenter : IShopCasinoPersonalListener
{
    private readonly ShopCasinoPersonalModel _model;
    private readonly ShopCasinoPersonalView _view;

    public ShopCasinoPersonalPresenter(ShopCasinoPersonalModel model, ShopCasinoPersonalView view)
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
        _view.OnChoosePersonalType += _model.SetShopPersonalGroup;
        _view.OnChoosePersonalSkinId += _model.SetSkinId;

        _model.OnChooseShopPersonalGroup += SetGroup;
        _model.OnChooseSkinId += _view.Choose;
        _model.OnUnchooseSkinId += _view.Unchoose;
    }

    private void DeactivateEvents()
    {
        _view.OnChoosePersonalType -= _model.SetShopPersonalGroup;
        _view.OnChoosePersonalSkinId -= _model.SetSkinId;

        _model.OnChooseShopPersonalGroup -= SetGroup;
        _model.OnChooseSkinId -= _view.Choose;
        _model.OnUnchooseSkinId -= _view.Unchoose;
    }

    private void SetGroup(ShopCasinoPersonalDataGroup personalDataGroup)
    {
        _view.SetDataGroup(personalDataGroup);

        OnChoosePersonalGroup?.Invoke();
    }

    #region Output

    public event Action OnChoosePersonalGroup;

    #endregion
}

public interface IShopCasinoPersonalListener
{
    public event Action OnChoosePersonalGroup;
}
