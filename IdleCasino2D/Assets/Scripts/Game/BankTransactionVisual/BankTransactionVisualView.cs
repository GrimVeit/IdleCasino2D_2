using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankTransactionVisualView : View
{
    [Header("Main")]
    [SerializeField] private BankTransactionVisual bankTransactionVisualPrefab;
    [SerializeField] private Transform transformParent;

    [Header("Positions")]
    [SerializeField] private Transform transformStart;
    [SerializeField] private Transform transformEnd;

    [Header("Color")]
    [SerializeField] private Color colorIncrease;
    [SerializeField] private Color colorDecrease;

    private readonly List<BankTransactionVisual> visuals = new List<BankTransactionVisual>();
    private BankTransactionVisual _currentVisual;
    private Coroutine timerCoroutine;

    public void SetTransaction(int value)
    {
        if (_currentVisual != null)
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            _currentVisual.Deactivate(transformStart, 0.5f);
        }

        BankTransactionVisual newVisual = Instantiate(bankTransactionVisualPrefab, transformParent);
        newVisual.transform.localPosition = transformStart.localPosition;
        newVisual.SetText(value >= 0 ? $"+{value}$" : $"{value}$");
        newVisual.SetColor(value >= 0 ? colorIncrease : colorDecrease);

        newVisual.OnDeactivate += OnVisualDeactivate;
        visuals.Add(newVisual);
        _currentVisual = newVisual;

        newVisual.Activate(transformEnd, 0.5f);

        timerCoroutine = StartCoroutine(AutoDeactivateAfterDelay(newVisual, 4f));
    }

    private IEnumerator AutoDeactivateAfterDelay(BankTransactionVisual visual, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (visual != null)
        {
            visual.Deactivate(transformStart, 0.5f);
        }

        if (_currentVisual == visual)
            _currentVisual = null;
    }

    private void OnVisualDeactivate(BankTransactionVisual visual)
    {
        visual.OnDeactivate -= OnVisualDeactivate;
        visuals.Remove(visual);
        visual.Destroy();
    }
}
