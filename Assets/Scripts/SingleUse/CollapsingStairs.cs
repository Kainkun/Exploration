using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingStairs : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float timeToCollapse;
    public float timeToRaise;
    public float position;
    public float targetPosition;

    public float TargetPosition
    {
        get => targetPosition;
        set => targetPosition = value;
    }

    private Utility.CheckBoxData triggerCheckBoxData;

    private void Start()
    {
        transform.localScale = new Vector3(1, animationCurve.Evaluate(position), 1);
    }

    private void Update()
    {
        if (Math.Abs(position - TargetPosition) > 0.01f)
        {
            if (position > TargetPosition)
                position -= Time.deltaTime / timeToCollapse;

            if (position < TargetPosition)
                position += Time.deltaTime / timeToRaise;
        }
        else
        {
            position = TargetPosition;
        }


        transform.localScale = new Vector3(1, animationCurve.Evaluate(position), 1);
    }
}