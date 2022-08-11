using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public LayerMask layerMask;

    private IInteractable currentInteractable;

    public static bool hoveringInteractable;

    private void Start()
    {
        InputManager.Singleton.use += Interact;
    }

    private void Update()
    {
        IInteractable previousInteractable = currentInteractable;
        currentInteractable = CheckForInteractable();
        hoveringInteractable = currentInteractable != null;
    }

    IInteractable CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5, layerMask,
                QueryTriggerInteraction.Collide))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable != null)
            {
                return interactable;
            }
        }

        return null;
    }

    void Interact()
    {
        currentInteractable?.PrimaryInteract();
    }
}