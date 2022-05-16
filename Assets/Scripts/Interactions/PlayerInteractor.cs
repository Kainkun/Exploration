using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
            {
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                interactable?.PrimaryInteract();
            }
        }
    }
}