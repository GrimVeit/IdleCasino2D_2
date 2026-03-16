using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongstressPresenter : ISongstress
{
    public StaffType StaffType => StaffType.Croupier;

    private readonly SongstressModel _model;
    private readonly SongstressView _view;

    public SongstressPresenter(SongstressModel model, SongstressView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClick += _model.Click;

        _model.OnSetAnimation += _view.ActivateAnimation;
        _model.OnClick += Click;
    }

    private void DeactivateEvents()
    {
        _view.OnClick -= _model.Click;

        _model.OnSetAnimation -= _view.ActivateAnimation;
        _model.OnClick -= Click;
    }

    #region DEALER
    public void ActivateAnimation(SongstressAnimationEnum animationEnum) => _model.SetAnimation(animationEnum);

    #endregion

    #region ACTIVATOR

    public void Show() => _view.Show();

    public void HideDestroy() => _view.HideDestroy();
    public void SetMove(Node node) => _view.SetMove(node);

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum) => _view.ActivateNpcRotation(npcRotationEnum);

    public void SetOrder(int order) { }

    public event Action<INpc, Node> OnEndDestination;

    public Node CurrentNode => _view.CurrentNode;

    public Vector3 Position => _view.Position;

    public void MoveTo(Node target, bool IsAbsolute)
    {

    }

    #endregion

    #region CLICK

    public event Action<ISongstress> OnClick;

    private void Click()
    {
        OnClick?.Invoke(this);
    }

    #endregion

    #region MESSAGE

    public void SetMessage(string message, SpeechTurnEnum turn) => _view.SetMessage(message, turn);

    public void SetMessage(string message)
    {
        SpeechTurnEnum turnEnum = (SpeechTurnEnum)UnityEngine.Random.Range(0, 2);

        _view.SetMessage(message, turnEnum);
    }

    #endregion
}
