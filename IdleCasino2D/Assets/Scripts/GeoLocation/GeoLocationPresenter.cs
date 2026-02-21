using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class GeoLocationPresenter
{
    private GeoLocationModel geoLocationModel;

    public GeoLocationPresenter(GeoLocationModel geoLocationModel)
    {
        this.geoLocationModel = geoLocationModel;
    }

    public void GetUserCountry()
    {
        geoLocationModel.GetUserCountry();
    }

    public event Action OnErrorGetCountry
    {
        add => geoLocationModel.OnErrorGetCountry += value;
        remove => geoLocationModel.OnErrorGetCountry -= value;
    }

    public event Action<string> OnGetCountry
    {
        add { geoLocationModel.OnGetCountry += value; }
        remove { geoLocationModel.OnGetCountry -= value; }
    }
}
