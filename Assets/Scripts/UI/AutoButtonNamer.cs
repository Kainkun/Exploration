using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class AutoButtonNamer : MonoBehaviour
{
    public string buttonName = "Button";

    public void OnValidate()
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText(buttonName);
        name = buttonName + " Button";
    }
}
