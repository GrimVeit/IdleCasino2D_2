using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostessPresenter : IHostess
{
    public StaffType StaffType => StaffType.Hostess;

    private readonly HostessModel _model;
    private readonly HostessView _view;

    public HostessPresenter(HostessModel model, HostessView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnSetAnimation += _view.ActivateAnimation;
    }

    private void DeactivateEvents()
    {
        _model.OnSetAnimation -= _view.ActivateAnimation;
    }

    #region DEALER
    public void ActivateAnimation(HostessAnimationEnum animationEnum) => _model.SetAnimation(animationEnum);

    #endregion

    #region ACTIVATOR

    public void Show() => _view.Show();

    public void HideDestroy() => _view.HideDestroy();

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum) => _view.ActivateNpcRotation(npcRotationEnum);

    public void SetOrder(int order) => _view.SetOrder(order);

    public Node CurrentNode => _view.CurrentNode;

    public Vector3 Position => _view.Position;

    #endregion

    #region MOVE

    public event Action<INpc, Node> OnEndDestination;
    public void SetMove(Node node) => _view.SetMove(node);
    public void MoveTo(Node target, bool IsAbsolute) => _view.MoveTo(target, IsAbsolute);

    #endregion
}
