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

    public Hologram frontHologram;
    public Hologram backHologram;


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

    private void Awake()
    {
        frontTriggerCheckBoxData = Utility.GetCheckBoxData(frontSideTrigger);
        backTriggerCheckBoxData = Utility.GetCheckBoxData(backSideTrigger);

        if (rightDoor)
            rightStartPosition = rightDoor.position;
        if (leftDoor)
            leftStartPosition = leftDoor.position;


        if (frontHologram)
            frontHologram.OnTurnOn += GetFrontDisplayText;
        if (backHologram)
            backHologram.OnTurnOn += GetBackDisplayText;
    }

    private void OnDestroy()
    {
        if (frontHologram)
            frontHologram.OnTurnOn -= GetFrontDisplayText;
        if (backHologram)
            backHologram.OnTurnOn -= GetBackDisplayText;
    }

    private void Update()
    {
        CheckDoorState();
        AnimateDoor();
    }


    public string GetFrontDisplayText() =>
        GetDisplayText(frontSideRequiredUniqueCollectables, frontSideRequiredStackingCollectables);

    public string GetBackDisplayText() =>
        GetDisplayText(backSideRequiredUniqueCollectables, backSideRequiredStackingCollectables);

    private string GetDisplayText(string[] requiredUniqueCollectables,
        YarnUtils.CollectableAmountPair[] requiredStackingCollectables
    )
    {
        string s = "";
        if (requiredUniqueCollectables.Length > 0)
            for (int i = 0; i < requiredUniqueCollectables.Length; i++)
            {
                string name = requiredUniqueCollectables[i];
                if (YarnUtils.variableNameToStringDict.ContainsKey(name))
                    name = YarnUtils.variableNameToStringDict[name];
                s += name + " required\n";
            }

        if (requiredStackingCollectables.Length > 0)
        {
            for (int i = 0; i < requiredStackingCollectables.Length; i++)
            {
                string name = requiredStackingCollectables[i].collectableName;
                if (YarnUtils.variableNameToStringDict.ContainsKey(name))
                    name = YarnUtils.variableNameToStringDict[name];

                float amount = requiredStackingCollectables[i].collectableAmount;
                s += amount + " " + name + (amount > 1 ? "s" : "") +
                     " required\n";
            }
        }

        if (requiredUniqueCollectables.Length > 0 || requiredStackingCollectables.Length > 0)
            s = s.Substring(0, s.Length - 1);

        //YarnAccess.TryGetValue("depositedTrashCount", out float currentDepositedTrashCount);
        return s;
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