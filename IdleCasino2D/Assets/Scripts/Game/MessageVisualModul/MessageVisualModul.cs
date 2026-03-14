using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageVisualModul : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private MessageVisual messageVisualPrefab_Left;
    [SerializeField] private MessageVisual messageVisualPrefab_Right;
    [SerializeField] private Transform transformParent;

    private readonly List<MessageVisual> visuals = new();
    private MessageVisual _currentVisual;
    private Coroutine timerCoroutine;

    public void SetMessage(string message, SpeechTurnEnum turnEnum)
    {
        if (_currentVisual != null)
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            _currentVisual.Deactivate();
        }

        MessageVisual newVisual;

        if(turnEnum == SpeechTurnEnum.Left)
        {
            newVisual = Instantiate(messageVisualPrefab_Left, transformParent);
        }
        else
        {
            newVisual = Instantiate(messageVisualPrefab_Right, transformParent);
        }

        newVisual.transform.localPosition = Vector3.zero;
        newVisual.SetText(message);

        newVisual.OnDeactivate += OnVisualDeactivate;
        visuals.Add(newVisual);
        _currentVisual = newVisual;

        newVisual.Activate();

        timerCoroutine = StartCoroutine(AutoDeactivateAfterDelay(newVisual, 0.7f));
    }

    private IEnumerator AutoDeactivateAfterDelay(MessageVisual visual, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (visual != null)
        {
            visual.Deactivate();
        }

        if (_currentVisual == visual)
            _currentVisual = null;
    }

    private void OnVisualDeactivate(MessageVisual visual)
    {
        visual.OnDeactivate -= OnVisualDeactivate;
        visuals.Remove(visual);
        visual.Destroy();
    }
}
