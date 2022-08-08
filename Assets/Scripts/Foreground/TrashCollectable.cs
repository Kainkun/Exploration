using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollectable : YarnCollectable
{
    private DisplayBounds displayBounds;
    private const float boundMinDistance = 20;

    private void Awake()
    {
        displayBounds = GetComponentInChildren<DisplayBounds>();
    }

    public override void Collect()
    {
        base.Collect();
    }

    private void Update()
    {
        displayBounds.tryToDisplay =
            Vector3.Distance(PlayerManager.playerMovement.transform.position, transform.position) < boundMinDistance;
    }
}