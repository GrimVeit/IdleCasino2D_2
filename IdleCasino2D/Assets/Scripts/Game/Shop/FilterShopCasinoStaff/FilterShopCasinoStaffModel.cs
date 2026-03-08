using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FilterShopCasinoStaffModel
{
    private readonly List<CasinoEntityStaffAdapter> _casinoEntities = new();
    private ShopCasinoStaffData _currentStaffData;
    private readonly IMoneyProvider _moneyProvider;
    private readonly ISpawnerStaffProvider _spawnerStaffProvider;
    private readonly IShopCasinoPersonalListener _shopCasinoPersonalListener;

    private bool isActiveClicked = false;

    public FilterShopCasinoStaffModel(
        List<ICasinoEntityInfo> casinoEntities, IMoneyProvider moneyProvider, ISpawnerStaffProvider spawnerStaffProvider, IShopCasinoPersonalListener shopCasinoPersonalListener)
    {
        foreach (var entity in casinoEntities)
        {
            if (entity is ICasinoEntityStaff staff)
            {
                ICasinoEntitySpotClickListener clickListener = entity as ICasinoEntitySpotClickListener;
                ICasinoEntityInteractiveProvider interactiveProvider = entity as ICasinoEntityInteractiveProvider;
                ICasinoEntityHighlightProvider highlight = entity as ICasinoEntityHighlightProvider;

                var dto = new CasinoEntityStaffAdapter(entity, staff, clickListener, interactiveProvider, highlight);
                dto.OnCasinoSpotClicked += OnSpotClick;
                dto.Initialize();

                _casinoEntities.Add(dto);
            }
        }

        _moneyProvider = moneyProvider;
        _spawnerStaffProvider = spawnerStaffProvider;
        _shopCasinoPersonalListener = shopCasinoPersonalListener;
    }

    public void Activate()
    {
        isActiveClicked = true;
    }

    public void Deactivate()
    {
        isActiveClicked = false;
    }

    public void Initialize()
    {
        _shopCasinoPersonalListener.OnChooseStaffData += SetStaffData;
    }

    public void Dispose()
    {
        _shopCasinoPersonalListener.OnChooseStaffData -= SetStaffData;
    }

    public void CancelSelection()
    {
        _currentStaffData = null;

        ActivateAll();

        foreach (var d in _casinoEntities)
        {
            d.CasinoEntityHighlightProvider?.DeactivateHighlight();
        }
    }

    private void SetStaffData(ShopCasinoStaffData staffData)
    {
        Debug.Log(staffData.StaffType);

        _currentStaffData = staffData;

        if (!_moneyProvider.CanAfford(_currentStaffData.Price))
        {
            OnPurchaseFailed?.Invoke("Not enough money for this staff");
            return;
        }

        for (int i = 0; i < _casinoEntities.Count; i++)
        {
            _casinoEntities[i].CasinoEntityInteractiveProvider?.DeactivateEntityInteractive();
        }

        switch (_currentStaffData.StaffType)
        {
            case StaffType.Manager:
            case StaffType.Hostess:
            case StaffType.Bartender:
            case StaffType.Songstress:
                HandleInstantPurchase();
                break;
            case StaffType.Croupier:
                PrepareClickSelection();
                break;
        }
    }

    private void HandleInstantPurchase()
    {
        var availableEntity = _casinoEntities.FirstOrDefault(e =>
            e.IsOpen == true &&
            e.CasinoEntityStaff.PersonalType == _currentStaffData.StaffType &&
            e.CasinoEntityStaff.CountStaff < e.CasinoEntityStaff.CountStaffNeed);

        if (availableEntity == null)
        {
            OnPurchaseFailed?.Invoke("No available slot for this staff");
            CancelSelection();
            return;
        }

        _moneyProvider.SendMoney(-_currentStaffData.Price);
        _spawnerStaffProvider.SetStaff(availableEntity.CasinoEntityStaff, _currentStaffData.StaffType, _currentStaffData.SkinId);
        OnStaffPurchased?.Invoke();

        CancelSelection();
    }

    private void PrepareClickSelection()
    {
        bool hasAvailable = false;

        DeactivateAll();

        foreach (var dto in _casinoEntities)
        {
            if (dto.CasinoEntityStaff.PersonalType != StaffType.Croupier) continue;

            bool isAvailable = dto.IsOpen && dto.CasinoEntityStaff.CountStaff < dto.CasinoEntityStaff.CountStaffNeed;

            if (dto.CasinoEntityInteractiveProvider != null)
            {
                if (isAvailable)
                {
                    dto.CasinoEntityHighlightProvider?.ActivateHighlight();
                    dto.CasinoEntityInteractiveProvider.ActivateEntityInteractive();
                }
                else
                {
                    dto.CasinoEntityInteractiveProvider.DeactivateEntityInteractive();
                }
            }

            if (isAvailable) hasAvailable = true;
        }

        if (!hasAvailable)
        {
            OnPurchaseFailed?.Invoke("No available tables for this staff");
            CancelSelection();
            return;
        }

        OnStaffOpenChoose?.Invoke(_currentStaffData);
    }

    private void OnSpotClick(CasinoEntityStaffAdapter dto)
    {
        if (!isActiveClicked) return;

        if (_currentStaffData == null) return;

        if (dto.CasinoEntityStaff.PersonalType != _currentStaffData.StaffType) return;

        _moneyProvider.SendMoney(-_currentStaffData.Price);
        _spawnerStaffProvider.SetStaff(dto.CasinoEntityStaff, _currentStaffData.StaffType, _currentStaffData.SkinId);
        OnStaffPurchased?.Invoke();

        CancelSelection();
    }

    private void ActivateAll()
    {
        foreach (var d in _casinoEntities)
        {
            d.CasinoEntityInteractiveProvider?.ActivateEntityInteractive();
        }
    }

    private void DeactivateAll()
    {
        foreach (var d in _casinoEntities)
        {
            d.CasinoEntityHighlightProvider?.DeactivateHighlight();
            d.CasinoEntityInteractiveProvider?.DeactivateEntityInteractive();
        }
    }

    #region Output

    public event Action<string> OnPurchaseFailed;

    public event Action<ShopCasinoStaffData> OnStaffOpenChoose;
    public event Action OnStaffPurchased;

    #endregion
}

public class CasinoEntityStaffAdapter
{
    public ICasinoEntityInfo CasinoEntityInfo { get; }
    public ICasinoEntityStaff CasinoEntityStaff { get; }

    public ICasinoEntitySpotClickListener SpotClickListener { get; }
    public ICasinoEntityInteractiveProvider CasinoEntityInteractiveProvider { get; }
    public ICasinoEntityHighlightProvider CasinoEntityHighlightProvider { get; }


    public CasinoEntityType CasinoEntityType => CasinoEntityInfo.CasinoEntityType;
    public bool IsOpen => CasinoEntityInfo.IsOpen;

    public event Action<CasinoEntityStaffAdapter> OnCasinoSpotClicked;

    public CasinoEntityStaffAdapter(
        ICasinoEntityInfo casinoEntityInfo,
        ICasinoEntityStaff casinoEntityStaff,

        ICasinoEntitySpotClickListener casinoEntitySpotClick = null,
        ICasinoEntityInteractiveProvider casinoEntityInteractiveProvider = null,
        ICasinoEntityHighlightProvider casinoEntityHighlightProvider = null)
    {
        CasinoEntityInfo = casinoEntityInfo;
        CasinoEntityStaff = casinoEntityStaff;

        SpotClickListener = casinoEntitySpotClick;
        CasinoEntityInteractiveProvider = casinoEntityInteractiveProvider;
        CasinoEntityHighlightProvider = casinoEntityHighlightProvider;
    }

    private void CasinoSpotClicked()
    {
        OnCasinoSpotClicked?.Invoke(this);
    }

    public void Initialize()
    {
        if(SpotClickListener != null)
           SpotClickListener.OnSpotClick += CasinoSpotClicked;
    }

    public void Dispose()
    {
        if(SpotClickListener != null)
           SpotClickListener.OnSpotClick -= CasinoSpotClicked;
    }
}
