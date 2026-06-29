using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabasePresenter
{
    private readonly DatabaseModel _model;

    public DatabasePresenter(DatabaseModel model)
    {
        _model = model;
    }

    #region Output

    public event Action<List<string>> OnGetCountries
    {
        add => _model.OnGetCountries += value;
        remove => _model.OnGetCountries -= value;
    }

    public event Action OnErrorGetCountries
    {
        add => _model.OnErrorGetCountries += value;
        remove => _model.OnErrorGetCountries -= value;
    }

    public event Action<string> OnGetLink
    {
        add => _model.OnGetLink += value;
        remove => _model.OnGetLink -= value;
    }

    public event Action OnErrorGetLink
    {
        add => _model.OnErrorGetLink += value;
        remove => _model.OnErrorGetLink -= value;
    }

    #endregion

    #region Input

    public void GetCountries() => _model.GetCountries();

    public void GetLink() => _model.GetLink();

    #endregion
}
