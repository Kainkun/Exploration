using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrashCollectable : YarnCollectable
{
    public static bool isDisplayingBounds;
    public DisplayBounds displayBounds;
    private const float boundMinDistance = 20;
    public const float propogationSpeed = 0.6f;
    private static float currentBoundDistance;

    private void Start()
    {
        if( PlayerMovement.Singleton == null)
            Destroy(gameObject);
    }

    private void Awake()
    {
        //displayBounds = GetComponentInChildren<DisplayBounds>();
        isDisplayingBounds = false;
        currentBoundDistance = 0;
    }

    public override void Collect()
    {
        base.Collect();
    }

    private void Update()
    {
        if (isDisplayingBounds)
        {
            if (currentBoundDistance < boundMinDistance)
                currentBoundDistance += Time.deltaTime * propogationSpeed;
            else
                currentBoundDistance = boundMinDistance;
        }
        else
        {
            if (currentBoundDistance > 0)
                currentBoundDistance -= Time.deltaTime * propogationSpeed;
            else
                currentBoundDistance = 0;
        }


        displayBounds.isOn = Vector3.Distance(PlayerMovement.Singleton.transform.position, transform.position) <
                             currentBoundDistance;
    }



    IEnumerator FlickerOn()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.05f);
            isDisplayingBounds = false;
            yield return new WaitForSeconds(0.05f);
            isDisplayingBounds = true;
        }
    }



    IEnumerator FlickerOff()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.05f);
            isDisplayingBounds = true;
            yield return new WaitForSeconds(0.05f);
            isDisplayingBounds = false;
        }
    }

}