using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationElementView : View
{
    [SerializeField] private List<AnimationElement> animationElements = new List<AnimationElement>();

    public void Activate(string id, int cycles)
    {
        var element = GetAnimationElement(id);

        if(element == null)
        {
            Debug.LogWarning("Not found animation element with id - " + id);
        }

        element.Activate(cycles);
    }

    public void Deactivate(string id)
    {
        var element = GetAnimationElement(id);

        if (element == null)
        {
            Debug.LogWarning("Not found animation element with id - " + id);
        }

        element.Deactivate();
    }

    private AnimationElement GetAnimationElement(string id)
    {
        return animationElements.FirstOrDefault(data => data.Id == id);
    }
}
