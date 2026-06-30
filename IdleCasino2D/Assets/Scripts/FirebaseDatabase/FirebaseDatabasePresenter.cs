using System;
using System.Collections.Generic;

public class FirebaseDatabasePresenter : IDatabaseRecordsEvents
{
    private readonly FirebaseDatabaseModel _model;

    public FirebaseDatabasePresenter(FirebaseDatabaseModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Output

    public event Action<List<UserData>> OnGetUsersRecords
    {
        add => _model.OnGetUsersRecords += value;
        remove => _model.OnGetUsersRecords -= value;
    }



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




    public event Action<UserData> OnGetUserFromPlace
    {
        add { _model.OnGetUserFromPlace += value; }
        remove { _model.OnGetUserFromPlace -= value; }
    }

    public event Action OnErrorGetUserFromPlace
    {
        add => _model.OnErrorGetUserFromPlace += value;
        remove => _model.OnErrorGetUserFromPlace -= value;
    }





    public void CreateEmptyDataToServer()
    {
        _model.CreateNewAccountInServer();
    }

    public void SaveChangeToServer()
    {
        _model.SaveChangesToServer();
    }

    public void DisplayUsersRecords()
    {
        _model.DisplayUsersRecords();
    }

    public void SetNickname(string nickname)
    {
        _model.SetNickname(nickname);
    }

    public void GetUserFromPlace(int place)
    {
        _model.GetUserFromPlace(place);
    }

    public void GetCountries()
    {
        _model.GetCountries();
    }

    public void GetLink()
    {
        _model.GetLink();
    }

    #endregion
}

public interface IDatabaseRecordsEvents
{
    public event Action<List<UserData>> OnGetUsersRecords;
}
