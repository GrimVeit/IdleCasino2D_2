using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoEntityClickInteractionModel
{
    private List<CasinoEntityClickInteractionAdapter> _casinoEntityClickInteractionAdapters = new();

    public CasinoEntityClickInteractionModel(List<ICasinoEntityInfo> casinoEntities)
    {
        foreach (var entity in casinoEntities)
        {
            if (entity is ICasinoEntitySpotClickListener spotClickListener && 
                entity is ICasinoEntityActivator activator &&
                entity is ICasinoEntityManual manual)
            {
                var dto = new CasinoEntityClickInteractionAdapter(entity, spotClickListener, activator, manual);
                dto.OnCasinoSpotClicked += CasinoSpotClick;

                _casinoEntityClickInteractionAdapters.Add(dto);
            }
        }
    }

    public void Initialize()
    {
        for (int i = 0; i < _casinoEntityClickInteractionAdapters.Count; i++)
        {
            _casinoEntityClickInteractionAdapters[i].Initialize();
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < _casinoEntityClickInteractionAdapters.Count; i++)
        {
            _casinoEntityClickInteractionAdapters[i].Dispose();
        }
    }

    public void CasinoSpotClick(CasinoEntityClickInteractionAdapter casinoEntityClickInteractionAdapter)
    {
        if (casinoEntityClickInteractionAdapter.IsOpen)
        {
            OnClickToOpenCasinoEntity?.Invoke(casinoEntityClickInteractionAdapter);
        }
        else
        {
            OnClickToCloseCasinoEntity?.Invoke(casinoEntityClickInteractionAdapter);
        }
    }

    #region Output

    public event Action<CasinoEntityClickInteractionAdapter> OnClickToOpenCasinoEntity;
    public event Action<CasinoEntityClickInteractionAdapter> OnClickToCloseCasinoEntity;

    #endregion
}

public class CasinoEntityClickInteractionAdapter
{
    public ICasinoEntityInfo CasinoEntityInfo { get; }
    public ICasinoEntitySpotClickListener SpotClickListener { get; }
    public ICasinoEntityActivator CasinoEntityActivator { get; }
    public ICasinoEntityManual CasinoEntityManual { get; }

    public CasinoEntityType CasinoEntityType => CasinoEntityInfo.CasinoEntityType;
    public bool IsOpen => CasinoEntityInfo.IsOpen;

    public event Action<CasinoEntityClickInteractionAdapter> OnCasinoSpotClicked;

    public CasinoEntityClickInteractionAdapter(
        ICasinoEntityInfo casinoEntityInfo,
        ICasinoEntitySpotClickListener casinoEntitySpotClick,
        ICasinoEntityActivator casinoEntityActivator,
        ICasinoEntityManual casinoEntityManual)
    {
        CasinoEntityInfo = casinoEntityInfo;
        SpotClickListener = casinoEntitySpotClick;
        CasinoEntityActivator = casinoEntityActivator;
        CasinoEntityManual = casinoEntityManual;
    }

    private void CasinoSpotClicked()
    {
        OnCasinoSpotClicked?.Invoke(this);
    }

    public void Initialize()
    {
        SpotClickListener.OnSpotClick += CasinoSpotClicked;
    }

    public void Dispose()
    {
        SpotClickListener.OnSpotClick -= CasinoSpotClicked;
    }
}
