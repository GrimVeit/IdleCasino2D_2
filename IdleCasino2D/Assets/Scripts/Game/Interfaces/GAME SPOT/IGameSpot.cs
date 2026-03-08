using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameSpot
{
    //CLICK-INTERACTIVE
    public event Action OnClick;

    //ANIMATIONS
    public void ActivateAnimation(string name);

    public void ActivateHightlight();
    public void DeactivateHighlight();
}
