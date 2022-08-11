using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnCollectable : MonoBehaviour
{
    public enum Type
    {
        Unique,
        Stacking,
    }

    public Type type;
    public bool dontDestroy;
    public string collectableIdentifier;
    public float amount;
    public string dialogueTitle;
    private bool collected;

    private void OnValidate()
    {
        collectableIdentifier = collectableIdentifier.Replace(" ", "_");
    }


    public virtual void Collect()
    {
        if (dialogueTitle != "")
            if (YarnAccess.dialogueRunner.IsDialogueRunning)
                return;
            else
                YarnAccess.dialogueRunner.StartDialogue(dialogueTitle);

        if (collected)
            return;

        switch (type)
        {
            case Type.Unique:
                YarnAccess.SetValue(collectableIdentifier, true);
                break;

            case Type.Stacking:
                bool variableExists = YarnAccess.TryGetValue(collectableIdentifier, out float newAmount);
                if (variableExists)
                    newAmount += amount;
                else
                    newAmount = amount;
                YarnAccess.SetValue(collectableIdentifier, newAmount);
                break;
        }

        collected = true;

        Hologram.RefreshHolograms();

        if (!dontDestroy)
            Destroy(gameObject);
    }
}