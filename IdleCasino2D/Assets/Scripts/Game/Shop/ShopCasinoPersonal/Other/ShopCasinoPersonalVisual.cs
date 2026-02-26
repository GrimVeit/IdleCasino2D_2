using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCasinoPersonalVisual : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int SkinId => _skinId;

    [SerializeField] private Image imagePersonal;

    [SerializeField] private Image imageChoose;
    [SerializeField] private Sprite spriteActive;
    [SerializeField] private Sprite spriteInactive;

    private int _skinId;

    public void SetData(ShopCasinoPersonalData data)
    {
        imagePersonal.sprite = data.SpriteSkin;
        _skinId = data.SkinId;
    }

    public void Choose()
    {
        imageChoose.sprite = spriteActive;
    }

    public void Unchoose()
    {
        imageChoose.sprite = spriteInactive;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnChoosePersonal?.Invoke(_skinId);
    }

    #region Output

    public event Action<int> OnChoosePersonal;

    #endregion
}
