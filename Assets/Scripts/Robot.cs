using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : YarnDialogueInteractable
{
    private bool lookTowardsPlayer;
    public float smoothTime;
    public float maxSpeed;

    private Vector3 startLocalRotation;
    private float angleVelocity;

    private void Start()
    {
        startLocalRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        if (lookTowardsPlayer)
        {
            Vector3 playerPosition = PlayerManager.playerMovement.transform.position;
            Vector3 directionToPlayerFlat = playerPosition - transform.position;
            directionToPlayerFlat.y = 0;
            float targetAngle = Vector3.SignedAngle(Vector3.forward, directionToPlayerFlat, Vector3.up);

            float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref angleVelocity,
                smoothTime, maxSpeed);

            transform.localRotation = Quaternion.Euler(startLocalRotation.x, angle, startLocalRotation.z);

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