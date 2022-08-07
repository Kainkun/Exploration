using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnCollectableInteractable : YarnCollectable, IInteractable
{
    public void PrimaryInteract()
    {
        Collect();
    }
}