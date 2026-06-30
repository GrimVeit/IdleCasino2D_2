using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthenticationView : View
{
    [SerializeField] private TextMeshProUGUI textDescription;

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    public void SetDescription(string description)
    {
        textDescription.text = description;
    }
}
