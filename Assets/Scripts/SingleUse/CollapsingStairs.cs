using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingStairs : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public BoxCollider playerTrigger;
    public bool startCollapsed;
    float t;
    public float timeToCollapse;
    public float timeToRaise;
    public bool collapse;

    private Utility.CheckBoxData triggerCheckBoxData;

    private void Start()
    {
        t = startCollapsed ? 0 : 1;

        if (playerTrigger)
            triggerCheckBoxData = Utility.GetCheckBoxData(playerTrigger);
    }

    private void Update()
    {
        if (playerTrigger && Physics.CheckBox(
                triggerCheckBoxData.triggerGlobalPosition,
                triggerCheckBoxData.triggerHalfExtent,
                triggerCheckBoxData.triggerRotation,
                LayerMask.GetMask("Player"),
                QueryTriggerInteraction.Ignore))
        {
            collapse = true;
        }

        if (collapse)
        {
            if (t > 0)
                t -= Time.deltaTime / timeToCollapse;
            else
                t = 0;
        }
        else
        {
            if (t < 1)
                t += Time.deltaTime / timeToRaise;
            else
                t = 1;
        }

        transform.localScale = new Vector3(1, animationCurve.Evaluate(t), 1);
    }
}