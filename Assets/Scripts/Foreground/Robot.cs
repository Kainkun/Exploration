using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Robot : YarnDialogueInteractable
{
    public float smoothTime;
    public float maxSpeed;

    private float angleVelocity;
    private bool lookTowardsPlayer;

    private Transform mesh;

    protected void Start()
    {
        mesh = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        if (lookTowardsPlayer)
        {
            Vector3 playerPosition = PlayerMovement.Singleton.transform.position;
            Vector3 direction = playerPosition - mesh.position;
            Utility.YAxisLookTowardsSmoothDamp(mesh, direction, ref angleVelocity, smoothTime, maxSpeed,
                Time.fixedDeltaTime);
        }
        else
        {
            Vector3 localEulerAngles = mesh.localEulerAngles;
            float angle = Mathf.SmoothDampAngle(localEulerAngles.y, 0, ref angleVelocity,
                smoothTime, maxSpeed, Time.deltaTime);
            localEulerAngles = new Vector3(localEulerAngles.x, angle, localEulerAngles.z);
            mesh.localEulerAngles = localEulerAngles;
        }
    }

    protected override void EndDialogue()
    {
        lookTowardsPlayer = false;
    }

    public override void PrimaryInteract()
    {
        lookTowardsPlayer = true;
        base.PrimaryInteract();
    }
}