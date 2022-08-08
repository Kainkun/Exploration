using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Utility
{
    public struct CheckBoxData
    {
        public Vector3 triggerGlobalPosition;
        public Vector3 triggerHalfExtent;
        public Quaternion triggerRotation;
    }

    public static CheckBoxData GetCheckBoxData(BoxCollider boxCollider)
    {
        CheckBoxData checkBoxData;
        GetCheckBoxData(
            boxCollider,
            out checkBoxData.triggerGlobalPosition,
            out checkBoxData.triggerHalfExtent,
            out checkBoxData.triggerRotation);
        return checkBoxData;
    }

    public static void GetCheckBoxData(BoxCollider boxCollider, out Vector3 globalPosition, out Vector3 halfExtent,
        out Quaternion rotation)
    {
        var boxColliderTransform = boxCollider.transform;
        Vector3 boxColliderCenter = boxCollider.center;

        globalPosition = boxColliderTransform.position;
        globalPosition += boxColliderTransform.right * boxColliderCenter.x;
        globalPosition += boxColliderTransform.up * boxColliderCenter.y;
        globalPosition += boxColliderTransform.forward * boxColliderCenter.z;

        halfExtent = boxCollider.size / 2;

        rotation = boxColliderTransform.rotation;
    }

    public static void YAxisLookTowardsSmoothDamp(
        Transform selfTransform,
        Vector3 positionToLookAt,
        ref float angleVelocity,
        float smoothTime,
        float maxSpeed,
        float deltaTime)
    {
        Vector3 directionToPlayerFlat = positionToLookAt - selfTransform.position;
        directionToPlayerFlat.y = 0;
        float targetAngle = Vector3.SignedAngle(Vector3.forward, directionToPlayerFlat, Vector3.up);

        Vector3 localEulerAngles = selfTransform.localEulerAngles;
        float angle = Mathf.SmoothDampAngle(localEulerAngles.y, targetAngle, ref angleVelocity,
            smoothTime, maxSpeed, deltaTime);

        selfTransform.localRotation = Quaternion.Euler(localEulerAngles.x, angle, localEulerAngles.z);
    }
}