using System;
using System.Collections.Generic;
using UnityEngine;

public class StaffSpawnerModel
{
    private readonly List<IStaff> _staffList = new();
    private readonly List<ICasinoEntityStaff> _casinoEntityStaffList = new();

    private readonly Dictionary<StaffType, Func<IStaffModel>> _modelFactory = new()
    {
        { StaffType.Croupier, () => new DealerModel() },
        { StaffType.Bartender, () => new BartenderModel() },
        { StaffType.Songstress, () => new SongstressModel() },
        { StaffType.Hostess, () => new HostessModel() },
        { StaffType.Manager, () => new ManagerModel() },
    };
    private readonly Dictionary<StaffType, Func<IStaffModel, IStaffView, IStaff>> _presenterFactory = new()
    {
        { StaffType.Croupier, (model, view) => new DealerPresenter((DealerModel)model, (DealerView)view) },
        { StaffType.Bartender, (model, view) => new BartenderPresenter((BartenderModel)model, (BartenderView)view) },
        { StaffType.Songstress, (model, view) => new SongstressPresenter((SongstressModel)model, (SongstressView)view) },
        { StaffType.Hostess, (model, view) => new HostessPresenter((HostessModel) model, (HostessView)view) },
        { StaffType.Manager, (model, view) => new ManagerPresenter((ManagerModel)model, (ManagerView)view) },
    };

    private readonly ISoundProvider _soundProvider;

    private const string PlayerPrefsKey = "CasinoStaff";
    private readonly List<StaffSaveData> _savedStaff = new();
    private int _currentSkinId;
    private int _currentEntityIndex;

    private ICasinoEntityStaff _currentCasinoEntityStaff;

    public StaffSpawnerModel(List<ICasinoEntityInfo> casinoEntities, ISoundProvider soundProvider)
    {
        foreach (var entity in casinoEntities)
        {
            if (entity is ICasinoEntityStaff casinoEntityStaff)
            {
                _casinoEntityStaffList.Add(casinoEntityStaff);
            }
        }

        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey))
            return;

        string json = PlayerPrefs.GetString(PlayerPrefsKey);

        var container = JsonUtility.FromJson<StaffSaveContainer>(json);

        List<StaffSaveData> saved = new();
        saved.AddRange(container.Items);

        foreach (var data in saved)
        {
            var entity = _casinoEntityStaffList[data.EntityIndex];

            SpawnStaff(entity, data.StaffType, data.SkinId);
        }
    }

    public void Dispose()
    {
        var container = new StaffSaveContainer
        {
            Items = _savedStaff
        };

        string json = JsonUtility.ToJson(container);

        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    #region Public Methods

    public void SpawnStaff(ICasinoEntityStaff casinoEntityStaff, StaffType type, int skinId)
    {
        Debug.Log($"SPAWN STAFF 1 - {type} and {skinId}");

        _currentCasinoEntityStaff = casinoEntityStaff;

        _currentEntityIndex = _casinoEntityStaffList.IndexOf(_currentCasinoEntityStaff);
        _currentSkinId = skinId;

        Debug.Log("SPAWN STAFF 2");

        var data = new StaffSaveData
        {
            EntityIndex = _currentEntityIndex,
            StaffType = type,
            SkinId = skinId
        };

        Debug.Log("SPAWN STAFF 3");

        _savedStaff.Add(data);

        Debug.Log("SPAWN STAFF 4");

        OnSpawnPrefab?.Invoke(type, skinId);
    }

    public void CreateStaff(StaffType type, IStaffView view)
    {
        if (_currentCasinoEntityStaff == null) return;

        if (!_modelFactory.TryGetValue(type, out var modelCreator))
            throw new Exception($"No model for {type}");

        if (!_presenterFactory.TryGetValue(type, out var presenterCreator))
            throw new Exception($"No presenter for {type}");

        var model = modelCreator();
        model.SetSoundProvider(_soundProvider);
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

    public event Action<StaffType, int> OnSpawnPrefab;

    public event Action<IStaff> OnAddStaff;

    #endregion
}

[Serializable]
public class StaffSaveData
{
    public int EntityIndex;
    public StaffType StaffType;
    public int SkinId;
}

[Serializable]
public class StaffSaveContainer
{
    public List<StaffSaveData> Items = new();
}
