using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private protected GameObject panel;
    public virtual void Initialize() { }
    public virtual void ActivatePanel() { }
    public virtual void DeactivatePanel() { }
    public virtual void Dispose() { }
}
