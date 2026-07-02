using System;
using System.Collections;
using UnityEngine;

public class TimerModel
{
    public event Action OnActivateTimer;
    public event Action OnDeactivateTimer;
    public event Action OnStartTimer;
    public event Action OnStopTimer;

    public event Action<int> OnTimeChanged;

    private bool isActive;
    private IEnumerator timerCoroutine;

    public void ActivateTimer(int seconds, TimerDirection direction)
    {
        isActive = true;

        StopCoroutine();

        timerCoroutine = TimerCoroutine(seconds, direction);
        Coroutines.Start(timerCoroutine);

        OnActivateTimer?.Invoke();
    }

    public void DeactivateTimer()
    {
        isActive = false;

        StopCoroutine();

        OnDeactivateTimer?.Invoke();
    }

    public void ResetTimer()
    {
        isActive = false;

        StopCoroutine();

        OnTimeChanged?.Invoke(0);
    }

    private void StopCoroutine()
    {
        if (timerCoroutine != null)
        {
            Coroutines.Stop(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator TimerCoroutine(int seconds, TimerDirection direction)
    {
        OnStartTimer?.Invoke();

        int value = direction == TimerDirection.Backward ? seconds : 0;

        while (true)
        {
            OnTimeChanged?.Invoke(value);

            if (direction == TimerDirection.Backward)
            {
                if (value <= 0) break;
                value--;
            }
            else
            {
                if (value >= seconds) break;
                value++;
            }

            yield return new WaitForSeconds(1);
        }

        if (isActive)
            OnStopTimer?.Invoke();
    }
}

public enum TimerDirection
{
    Forward,
    Backward
}
