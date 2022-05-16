using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableLocked
{
    private bool toggled;
    Vector3 startPosition;
    Vector3 startRotation;
    public Vector3 toggledPosition;
    public Vector3 toggledRotation;

    private void Start()
    {
        var t = transform;
        startPosition = t.localPosition;
        startRotation = t.localEulerAngles;
    }

    public override void PrimaryInteractSuccess()
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

    public override void PrimaryInteractFail()
    {
        print("le epic fail XD");
    }
}
