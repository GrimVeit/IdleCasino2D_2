using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankTransactionVisualPresenter
{
    private readonly BankTransactionVisualModel _model;
    private readonly BankTransactionVisualView _view;

    public BankTransactionVisualPresenter(BankTransactionVisualModel model, BankTransactionVisualView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {

    }

    private void DeactivateEvents()
    {

    }
}
