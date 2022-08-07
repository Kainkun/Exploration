using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableUnityEvent : MonoBehaviour, IInteractable
{
    public string[] requiredCollectables;
    public bool oneTimeUse;
    public bool isOn;
    public UnityEvent success;
    public UnityEvent successOn;
    public UnityEvent successOff;
    public UnityEvent fail;

    public void PrimaryInteract()
    {
        if (YarnUtils.HasRequiredUniqueCollectables(requiredCollectables))
        {
            success.Invoke();
            if (isOn)
                successOff.Invoke();
            else
                successOn.Invoke();
            isOn = !isOn;
        }
        else
            fail.Invoke();

        if (oneTimeUse)
            this.enabled = false;
    }
}