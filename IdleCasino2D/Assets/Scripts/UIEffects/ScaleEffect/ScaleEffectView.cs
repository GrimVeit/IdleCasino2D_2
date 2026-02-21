using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEffectView : View
{
    [SerializeField] private List<ScaleEffectCombination> scaleEffects = new List<ScaleEffectCombination>();

    public void Initialize()
    {
        for (int i = 0; i < scaleEffects.Count; i++)
        {
            scaleEffects[i].Initialize();
        }
    }

    public void Dispose()
    {
        for(int i = 0;  i < scaleEffects.Count; i++)
        {
            scaleEffects[i].Dispose();
        }
    }
}
