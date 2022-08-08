using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : YarnDialogueInteractable
{
    public float smoothTime;
    public float maxSpeed;

    private float angleVelocity;
    private bool lookTowardsPlayer;

    private void FixedUpdate()
    {
        if (lookTowardsPlayer)
        {
            Vector3 playerPosition = PlayerManager.playerMovement.transform.position;
            Vector3 directionToPlayerFlat = playerPosition - transform.position;
            directionToPlayerFlat.y = 0;

            Utility.YAxisLookTowardsSmoothDamp(transform, playerPosition, ref angleVelocity, smoothTime, maxSpeed, Time.fixedDeltaTime);

            if (Vector3.Angle(transform.forward, directionToPlayerFlat) < 0.1f)
            {
                transform.forward = directionToPlayerFlat;
                lookTowardsPlayer = false;
            }
        }
    }

    public override void PrimaryInteract()
    {
        lookTowardsPlayer = true;
        base.PrimaryInteract();
    }
}