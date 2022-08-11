using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : SystemSingleton<PlayerCamera>
{
    public float mouseSensitivity;
    public float mouseSmoothing;
    private Vector2 mouseInput;

    [Range(0f, 90f)] public float cameraLookUpLimit = 90f;
    [Range(0f, 90f)] public float cameraLookDownLimit = 90f;

    public Transform cameraSystem;
    [HideInInspector] public CinemachineVirtualCamera virtualCamera;
    [HideInInspector] public CinemachineBrain cinemachineBrain;

    private Transform yawTransform;
    private Transform pitchTransform;
    private Transform rollTransform;
    private float currentPitch;

    protected override void Awake()
    {
        base.Awake();
        
        GameObject g = GameObject.FindGameObjectWithTag("MainCamera");
        if (!g)
            g = Instantiate(Resources.Load<GameObject>("Main Camera"));
        cinemachineBrain = g.GetComponent<CinemachineBrain>();

        yawTransform = cameraSystem.GetChild(0);
        pitchTransform = yawTransform.GetChild(0);
        rollTransform = pitchTransform.GetChild(0);
        virtualCamera = rollTransform.GetChild(0).GetComponent<CinemachineVirtualCamera>();

    }

    private void Start()
    {
        InputManager.Singleton.look += v => mouseInput = v;
    }

    private void LateUpdate()
    {
        BodyYaw(mouseInput.x * mouseSensitivity * Time.deltaTime);

        CameraRotate(
            0,
            mouseInput.y * mouseSensitivity * Time.deltaTime,
            0);
    }

    public void CameraRotate(float yaw, float pitch, float roll)
    {
        currentPitch += pitch;
        currentPitch = Mathf.Clamp(currentPitch, -cameraLookDownLimit, cameraLookUpLimit);
        pitchTransform.localEulerAngles = new Vector3(-currentPitch, 0, 0);

        yawTransform.Rotate(Vector3.up, yaw);

        rollTransform.localEulerAngles += new Vector3(0, 0, -roll);
    }

    public void HeadRotate()
    {
    }

    public void BodyYaw(float yaw)
    {
        transform.Rotate(Vector3.up, yaw);
    }
}