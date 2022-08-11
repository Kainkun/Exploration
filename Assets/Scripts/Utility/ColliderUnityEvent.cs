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
    private bool triggeredOnce;

    public LayerMask layerMask;

    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;

    private void OnTriggerEnter(Collider other)
    {
        if (oneShot && triggeredOnce)
        {
            Destroy(gameObject);
            return;
        }

        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            onTriggerEnter?.Invoke();

        triggeredOnce = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (oneShot && triggeredOnce)
        {
            Destroy(gameObject);
            return;
        }

        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            onTriggerExit?.Invoke();

        triggeredOnce = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (oneShot && triggeredOnce)
        {
            Destroy(gameObject);
            return;
        }

        if (layerMask == (layerMask | (1 << collision.gameObject.layer)))
            onCollisionEnter.Invoke();

        triggeredOnce = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (oneShot && triggeredOnce)
        {
            Destroy(gameObject);
            return;
        }

        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            onCollisionExit.Invoke();

        triggeredOnce = true;
    }
}