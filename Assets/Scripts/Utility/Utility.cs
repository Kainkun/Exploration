using System.Collections;
using System.Collections.Generic;
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
}