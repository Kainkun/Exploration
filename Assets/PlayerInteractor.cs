using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
            {
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                interactable?.PrimaryInteract();
            }
        }
    }
}