using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEffectCombination : MonoBehaviour
{
    [SerializeField] private List<ScaleEffect> scaleEffects = new List<ScaleEffect>();
    [SerializeField] private float timeScale;

    private IEnumerator scaleEffectCombimation_Coroutine;

    public void Initialize()
    {
        scaleEffects.ForEach(data => data.Initialize());
    }

    public void Dispose()
    {
        scaleEffects.ForEach(data => data.Dispose());
    }

    public void ActivateEffect()
    {
        if (scaleEffectCombimation_Coroutine != null)
            Coroutines.Stop(scaleEffectCombimation_Coroutine);

        scaleEffectCombimation_Coroutine = ActivateScaleEffect_Coroutine();
        Coroutines.Start(scaleEffectCombimation_Coroutine);
    }

    public void DeactivateEffect()
    {
        if (scaleEffectCombimation_Coroutine != null)
            Coroutines.Stop(scaleEffectCombimation_Coroutine);

        scaleEffectCombimation_Coroutine = DeactivateScaleEffect_Coroutine();
        Coroutines.Start(scaleEffectCombimation_Coroutine);
    }

    private IEnumerator ActivateScaleEffect_Coroutine()
    {
        for (int i = 0; i < scaleEffects.Count; i++)
        {
            scaleEffects[i].ActivateEffect();

            yield return new WaitForSeconds(timeScale);
        }
    }

    private IEnumerator DeactivateScaleEffect_Coroutine()
    {
        for (int i = 0; i < scaleEffects.Count; i++)
        {
            scaleEffects[i].DeactivateEffect();

            yield return new WaitForSeconds(timeScale);
        }
    }
}
