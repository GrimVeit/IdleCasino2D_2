using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementEntityPresenter :
    ICasinoEntityInfo,
    ICasinoEntityStaff
{
    private readonly ManagementEntityModel _model;

    public ManagementEntityPresenter(ManagementEntityModel model)
    {
        _model = model;
    }

    public void Initialize() => _model.Initialize();
    public void Dispose() => _model.Dispose();

    #region STAFF

    public StaffType PersonalType => StaffType.Manager;
    public int CountStaffNeed => 5;
    public int CountStaff => _model.CountStaff;
    public void SetStaff(IStaff staff) => _model.SetStaff(staff);

    #endregion

    #region INFO

    public CasinoEntityType CasinoEntityType => CasinoEntityType.Management;
    public bool IsOpen => true;
    public bool CanJoin => false;
    public bool IsGameRunning => false;

    #endregion
}
