using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    public string collectableIdentifier;

    private void OnValidate()
    {
        collectableIdentifier = collectableIdentifier.Replace(" ", "_");
    }

    public void PrimaryInteract()
    {
        YarnSingleton.Get().inMemoryVariableStorage.SetValue("$" + collectableIdentifier, true);
        Destroy(gameObject);
    }
}