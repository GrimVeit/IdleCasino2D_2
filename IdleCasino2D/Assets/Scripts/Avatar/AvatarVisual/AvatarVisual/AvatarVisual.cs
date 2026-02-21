using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AvatarVisual : MonoBehaviour
{
    public int Id => _id;

    [SerializeField] private Image imageAvatar;
    [SerializeField] private Button buttonAvatar;
    [SerializeField] private Transform transformLine;

    private int _id;
    private Tween tweenScale;

    public void Initialize()
    {
        buttonAvatar.onClick.AddListener(ChooseAvatar);
    }

    public void Dispose()
    {
        buttonAvatar.onClick.RemoveListener(ChooseAvatar);
    }

    public void SetData(int id, Sprite sprite)
    {
        imageAvatar.sprite = sprite;

        _id = id;
    }

    public void Select()
    {
        tweenScale?.Kill();

        tweenScale = transformLine.DOScale(1, 0.3f);
    }

    public void Deselect()
    {
        tweenScale?.Kill();

        tweenScale = transformLine.DOScale(0, 0.15f);
    }

    #region Output

    public event Action<int> OnChooseAvatar;

    private void ChooseAvatar()
    {
        OnChooseAvatar?.Invoke(_id);
    }

    #endregion
}
