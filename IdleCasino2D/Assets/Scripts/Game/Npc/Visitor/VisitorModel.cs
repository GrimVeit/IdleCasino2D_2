using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorModel
{
    private readonly List<CasinoEntityType> routeTargets;

    private int currentStepTarget = 0;

    private readonly ISoundProvider _soundProvider;

    public VisitorModel(List<CasinoEntityType> routeTargets, ISoundProvider soundProvider)
    {
        this.routeTargets = routeTargets ?? throw new ArgumentNullException(nameof(routeTargets));
        _soundProvider = soundProvider;
    }

    // Текущая цель
    public CasinoEntityType CurrentTarget
    {
        get
        {
            if (!HasCurrentStep())
                throw new InvalidOperationException("Visitor has no current target.");

            return routeTargets[currentStepTarget];
        }
    }

    // Есть ли текущий шаг
    public bool HasCurrentStep()
    {
        return currentStepTarget < routeTargets.Count;
    }

    // Есть ли следующий шаг
    public bool HasNextStep()
    {
        return currentStepTarget + 1 < routeTargets.Count;
    }

    // Переход к следующему шагу
    public void SetNextStep()
    {
        if (!HasNextStep())
            return;

        currentStepTarget += 1;
    }

    // Вторая цель (если нужна для предсмотра)
    public CasinoEntityType? SecondTarget
    {
        get
        {
            if (!HasNextStep())
                return null;

            return routeTargets[currentStepTarget + 1];
        }
    }

    public void MoveTo(Node target, bool isAbsolute)
    {
        OnStartMove?.Invoke(target, isAbsolute);
    }

    public void Click()
    {
        OnClick?.Invoke();
    }

    public void SendMessage(string message, SpeechTurnEnum turn)
    {
        Debug.Log(message);
        _soundProvider.PlayOneShot("Message");

        OnSendMessage?.Invoke(message, turn);
    }

    #region Output

    public event Action<Node, bool> OnStartMove;
    public event Action OnClick;
    public event Action<string, SpeechTurnEnum> OnSendMessage;

    #endregion
}
