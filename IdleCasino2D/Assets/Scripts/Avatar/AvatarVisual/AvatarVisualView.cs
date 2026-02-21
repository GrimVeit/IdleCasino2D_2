using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AvatarVisualView : View, IIdentify
{
    public string GetID() => id;

    [SerializeField] private string id;
    [SerializeField] private AvatarVisual avatarVisualPrefab;
    [SerializeField] private List<Sprite> spritesAvatar;

    [SerializeField] private List<Transform> slots;
    [SerializeField] private float moveDuration = 0.5f;
    public float scaleDuration = 0.3f;

    private readonly List<AvatarVisual> avatarVisuals = new();

    public void Initialize()
    {
        for (int i = 0; i < spritesAvatar.Count; i++)
        {
            var visual = Instantiate(avatarVisualPrefab, slots[i]);
            visual.transform.localPosition = Vector3.zero;
            visual.OnChooseAvatar += ChooseAvatar;
            visual.SetData(i, spritesAvatar[i]);
            visual.Initialize();

            avatarVisuals.Add(visual);
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < avatarVisuals.Count; i++)
        {
            avatarVisuals[i].OnChooseAvatar -= ChooseAvatar;
            avatarVisuals[i].Dispose();
        }

        avatarVisuals.Clear();
    }

    public void Select(int avatar)
    {
        avatarVisuals.FirstOrDefault(data => data.Id == avatar).Select();
    }

    public void Deselect(int avatar)
    {
        avatarVisuals.FirstOrDefault(data => data.Id == avatar).Deselect();
    }

    #region Output

    public event Action<int> OnChooseAvatar;

    private void ChooseAvatar(int avatar)
    {
        OnChooseAvatar?.Invoke(avatar);
    }

    #endregion
}
