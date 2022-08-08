using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string[] requiredUniqueCollectables;
    public YarnUtils.CollectableAmountPair[] requiredStackingCollectables;
    private bool toggled;
    Vector3 startPosition;
    Vector3 startRotation;
    public Vector3 toggledPosition;
    public Vector3 toggledRotation;
    
    private void Awake()
    {
        var t = transform;
        startPosition = t.localPosition;
        startRotation = t.localEulerAngles;
    }

    public void PrimaryInteract()
    {
        if (YarnUtils.HasRequiredCollectables(requiredUniqueCollectables, requiredStackingCollectables))
            InteractSuccess();
        else
            InteractFail();
    }

    public void InteractSuccess()
    {
        toggled = !toggled;
        var t = transform;
        if (toggled)
        {
            t.localPosition = toggledPosition;
            t.localEulerAngles = toggledRotation;
        }
        else
        {
            t.localPosition = startPosition;
            t.localEulerAngles = startRotation;
        }
    }

    public void InteractFail()
    {
        print("le epic fail XD");
    }
}