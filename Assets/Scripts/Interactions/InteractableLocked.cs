using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableLocked : MonoBehaviour, IInteractable
{
    public string[] requiredCollectables;

    public void PrimaryInteract()
    {
        if (HasRequiredCollectables())
            PrimaryInteractSuccess();
        else
            PrimaryInteractFail();
    }

    public abstract void PrimaryInteractSuccess();

    public abstract void PrimaryInteractFail();

    public bool HasRequiredCollectables()
    {
        for (int i = 0; i < requiredCollectables.Length; i++)
        {
            string requiredCollectable = requiredCollectables[i];
            bool variableExists = YarnSingleton.Get().inMemoryVariableStorage.TryGetValue("$" + requiredCollectable, out bool variableIsTrue);

            if (!variableExists || !variableIsTrue)
                return false;
        }

        return true;
    }
}