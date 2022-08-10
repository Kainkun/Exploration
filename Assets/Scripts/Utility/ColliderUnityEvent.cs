using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.LowLevel;

public class ColliderUnityEvent : MonoBehaviour
{
    public bool oneShot;

    public LayerMask layerMask;

    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;

    private void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            onTriggerEnter?.Invoke();
        
        if(oneShot)
            Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            onTriggerExit?.Invoke();
        
        if(oneShot)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (layerMask == (layerMask | (1 << collision.gameObject.layer)))
            onCollisionEnter.Invoke();
        
        if(oneShot)
            Destroy(gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            onCollisionExit.Invoke();
        
        if(oneShot)
            Destroy(gameObject);
    }
}