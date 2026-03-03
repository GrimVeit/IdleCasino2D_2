using System;
using System.Collections;
using System.Collections.Generic;

public class StaffSpawnerModel
{
    private readonly List<IStaff> _staffList = new();

    private readonly Dictionary<StaffType, Func<IStaffModel>> _modelFactory = new()
    {
        { StaffType.Croupier, () => new DealerModel() },
        { StaffType.Bartender, () => new BartenderModel() },
        { StaffType.Songstress, () => new SongstressModel() },
        { StaffType.Hostess, () => new HostessModel() },
    };

    private readonly Dictionary<StaffType, Func<IStaffModel, IStaffView, IStaff>> _presenterFactory = new()
    {
        { StaffType.Croupier, (model, view) => new DealerPresenter((DealerModel)model, (DealerView)view) },
        { StaffType.Bartender, (model, view) => new BartenderPresenter((BartenderModel)model, (BartenderView)view) },
        { StaffType.Songstress, (model, view) => new SongstressPresenter((SongstressModel)model, (SongstressView)view) },
        { StaffType.Hostess, (model, view) => new HostessPresenter((HostessModel) model, (HostessView)view) },
    };

    private ICasinoEntityStaff _currentCasinoEntityStaff;

    #region Public Methods

    public void SpawnStaff(ICasinoEntityStaff casinoEntityStaff, StaffType type)
    {
        _currentCasinoEntityStaff = casinoEntityStaff;

        OnSpawnPrefab?.Invoke(type);
    }

    public void CreateStaff(StaffType type, IStaffView view)
    {
        if (_currentCasinoEntityStaff == null) return;

        if (!_modelFactory.TryGetValue(type, out var modelCreator))
            throw new Exception($"No model for {type}");

        if (!_presenterFactory.TryGetValue(type, out var presenterCreator))
            throw new Exception($"No presenter for {type}");

        var model = modelCreator();
        var staff = presenterCreator(model, view);

        staff.Initialize();

        _currentCasinoEntityStaff.SetStaff(staff);

        OnAddStaff?.Invoke(staff);

        _staffList.Add(staff);
    }

    public void ClearAll()
    {
        foreach (var staff in _staffList)
            staff.Dispose();
        _staffList.Clear();
    }

    #endregion

    #region Output

    public event Action<StaffType> OnSpawnPrefab;

    public event Action<IStaff> OnAddStaff;

    #endregion
}
