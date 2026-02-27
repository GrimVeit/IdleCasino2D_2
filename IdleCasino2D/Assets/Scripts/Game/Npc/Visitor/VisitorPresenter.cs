using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorPresenter : IVisitor
{
    private readonly VisitorModel _model;
    private readonly VisitorView _view;

    public VisitorPresenter(VisitorModel model, VisitorView view)
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
        _view.OnPathCompleted += EndDestination;

        _model.OnStartMove += _view.MoveTo;
    }

    private void DeactivateEvents()
    {
        _view.OnPathCompleted -= EndDestination;

        _model.OnStartMove -= _view.MoveTo;
    }


    #region Output

    public event Action<INpc, Node> OnEndDestination;

    private void EndDestination(Node node)
    {
        OnEndDestination?.Invoke(this, node);
    }

    #endregion

    #region Input

    public Node CurrentNode => _view.CurrentNode;

    public void MoveTo(Node target, bool isAbsolute) => _model.MoveTo(target, isAbsolute);
    public void SetMove(Node node) => _view.SetMove(node);
    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum) => _view.ActivateNpcRotation(npcRotationEnum);

    public void Destroy() => _view.Exit();

    #region TARGET

    public CasinoEntityType CurrentTarget => _model.CurrentTarget;
    public bool MoveNextStep() => _model.MoveNextStep();


    #endregion

    #region EMOTIONS

    public void ActivateWin() => _view.ActivateAnimation(VisitorAnimationEnum.Win);

    public void ActivateLose() => _view.ActivateAnimation(VisitorAnimationEnum.Lose);

    public void ActivatePlay() => _view.ActivateAnimation(VisitorAnimationEnum.Play);
    public void ActivateIdle() => _view.ActivateAnimation(VisitorAnimationEnum.Idle);

    #endregion

    #region ORDER

    public Vector3 Position => _view.Position;
    public void SetOrder(int order) => _view.SetOrder(order);

    #endregion

    #region ACTIVATOR

    public void Show() => _view.Show();
    public void HideDestroy() => _view.HideDestroy();

    #endregion

    #endregion
}
