using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideArray : MonoBehaviour
{
    public Transform[] sliders;

    public enum MovementType
    {
        PerPart,
        Total
    };

    public enum StaggerType
    {
        StaggerStart,
        StaggerEnd
    };

    public MovementType movementType;
    public StaggerType staggerType;
    public Vector3 movement;
    public float timeOn = 1;
    public float timeOff = 1;

    private Vector3[] startPositions;
    private float currentT;
    public float targetT;
    public AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve positionCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private Vector3 lastSliderMovement;

    private void Start()
    {
        startPositions = new Vector3[sliders.Length];
        for (int i = 0; i < sliders.Length; i++)
            startPositions[i] = sliders[i].localPosition;

        if (movementType == MovementType.PerPart)
            lastSliderMovement = movement * (startPositions.Length + 1);
        else
            lastSliderMovement = movement;
    }

    public void Update()
    {
        if (currentT < targetT)
            currentT += (1 / timeOn) * Time.deltaTime;
        else if (currentT > targetT)
            currentT -= (1 / timeOff) * Time.deltaTime;

        currentT = Mathf.Clamp01(currentT);

        for (int i = 0; i < sliders.Length; i++)
        {
            if (staggerType == StaggerType.StaggerStart)
            {
                float t = positionCurve.Evaluate(currentT) * 2;
                t = Mathf.Clamp(t - ((1f / sliders.Length) * i), 0, 1);
                sliders[i].localPosition = startPositions[i] + lastSliderMovement * animationCurve.Evaluate(t);
            }
            else if (staggerType == StaggerType.StaggerEnd)
            {
                float t = animationCurve.Evaluate(currentT);
                t = Mathf.Clamp(t, 0, (1f / sliders.Length) * (i + 1));
                sliders[i].localPosition = startPositions[i] + lastSliderMovement * positionCurve.Evaluate(t);
            }
        }
    }

    public void SetTargetT(float t) => targetT = t;
}