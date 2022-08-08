using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    private DoorState doorState = DoorState.Close;

    public Transform rightDoor;
    public Transform leftDoor;

    public float slideDistance;

    public float timeToOpen;
    public float timeToClose;

    public AnimationCurve animationCurve;

    public string[] frontSideRequiredUniqueCollectables;
    public YarnUtils.CollectableAmountPair[] frontSideRequiredStackingCollectables;

    public string[] backSideRequiredUniqueCollectables;
    public YarnUtils.CollectableAmountPair[] backSideRequiredStackingCollectables;

    public BoxCollider frontSideTrigger;
    public BoxCollider backSideTrigger;

    private float t;
    private Vector3 rightStartPosition;
    private Vector3 leftStartPosition;

    private Utility.CheckBoxData frontTriggerCheckBoxData;
    private Utility.CheckBoxData backTriggerCheckBoxData;


    public enum DoorState
    {
        Close,
        Open
    };

    bool isFrontEntryAllowed =>
        YarnUtils.HasRequiredCollectables(frontSideRequiredStackingCollectables, frontSideRequiredUniqueCollectables);

    bool isBackEntryAllowed =>
        YarnUtils.HasRequiredCollectables(backSideRequiredStackingCollectables, backSideRequiredUniqueCollectables);

    bool isPlayerInFrontTrigger => frontSideTrigger && Physics.CheckBox(
        frontTriggerCheckBoxData.triggerGlobalPosition,
        frontTriggerCheckBoxData.triggerHalfExtent,
        frontTriggerCheckBoxData.triggerRotation,
        LayerMask.GetMask("Player"),
        QueryTriggerInteraction.Ignore);

    bool isPlayerInBackTrigger => backSideTrigger && Physics.CheckBox(
        backTriggerCheckBoxData.triggerGlobalPosition,
        backTriggerCheckBoxData.triggerHalfExtent,
        backTriggerCheckBoxData.triggerRotation,
        LayerMask.GetMask("Player"),
        QueryTriggerInteraction.Ignore);

    private void Start()
    {
        frontTriggerCheckBoxData = Utility.GetCheckBoxData(frontSideTrigger);
        backTriggerCheckBoxData = Utility.GetCheckBoxData(backSideTrigger);

        if (rightDoor)
            rightStartPosition = rightDoor.position;
        if (leftDoor)
            leftStartPosition = leftDoor.position;
    }

    private void Update()
    {
        CheckDoorState();
        AnimateDoor();
    }

    void CheckDoorState()
    {
        if ((isPlayerInFrontTrigger && isFrontEntryAllowed) ||
            (isPlayerInBackTrigger && isBackEntryAllowed))
            doorState = DoorState.Open;
        else
            doorState = DoorState.Close;
    }

    protected virtual void AnimateDoor()
    {
        if (doorState == DoorState.Open)
        {
            if (t < 1)
                t += Time.deltaTime / timeToOpen;
            else
                t = 1;
        }
        else
        {
            if (t > 0)
                t -= Time.deltaTime / timeToClose;
            else
                t = 0;
        }

        if (rightDoor)
            rightDoor.position = rightStartPosition + (transform.right * (slideDistance * animationCurve.Evaluate(t)));
        if (leftDoor)
            leftDoor.position = leftStartPosition + (-transform.right * (slideDistance * animationCurve.Evaluate(t)));
    }
}