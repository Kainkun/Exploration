using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public LayerMask layerMask;

    private void Start()
    {
        InputManager.Get().Use += Interact;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5, layerMask,
                QueryTriggerInteraction.Collide))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable != null)
            {
                
            }
        }
    }

    void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5, layerMask,
                QueryTriggerInteraction.Collide))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            interactable?.PrimaryInteract();
        }
    }
}