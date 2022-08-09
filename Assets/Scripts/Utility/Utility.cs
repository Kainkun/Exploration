using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float RealModulo(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

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

    private static Rect BoundsToScreenSpace(Camera camera, Bounds bounds)
    {
        Vector3[] corners = new Vector3[8];
        Vector2[] screenSpaceCorners = new Vector2[8];

        corners[0] = new Vector3(
            bounds.center.x + bounds.extents.x,
            bounds.center.y + bounds.extents.y,
            bounds.center.z + bounds.extents.z);
        corners[1] = new Vector3(
            bounds.center.x + bounds.extents.x,
            bounds.center.y + bounds.extents.y,
            bounds.center.z - bounds.extents.z);
        corners[2] = new Vector3(
            bounds.center.x + bounds.extents.x,
            bounds.center.y - bounds.extents.y,
            bounds.center.z + bounds.extents.z);
        corners[3] = new Vector3(
            bounds.center.x + bounds.extents.x,
            bounds.center.y - bounds.extents.y,
            bounds.center.z - bounds.extents.z);
        corners[4] = new Vector3(
            bounds.center.x - bounds.extents.x,
            bounds.center.y + bounds.extents.y,
            bounds.center.z + bounds.extents.z);
        corners[5] = new Vector3(
            bounds.center.x - bounds.extents.x,
            bounds.center.y + bounds.extents.y,
            bounds.center.z - bounds.extents.z);
        corners[6] =
            new Vector3(
                bounds.center.x - bounds.extents.x,
                bounds.center.y - bounds.extents.y,
                bounds.center.z + bounds.extents.z);
        corners[7] = new Vector3(
            bounds.center.x - bounds.extents.x,
            bounds.center.y - bounds.extents.y,
            bounds.center.z - bounds.extents.z);


        for (int i = 0; i < corners.Length; i++)
        {
            screenSpaceCorners[i] = camera.WorldToScreenPoint(corners[i]);
            Debug.DrawRay(corners[i], Vector3.up / 5f, Color.red);
        }

        // Now find the min/max X & Y of these screen space corners.
        float min_x = screenSpaceCorners[0].x;
        float min_y = screenSpaceCorners[0].y;
        float max_x = screenSpaceCorners[0].x;
        float max_y = screenSpaceCorners[0].y;

        for (int i = 1; i < 8; i++)
        {
            if (screenSpaceCorners[i].x < min_x)
            {
                min_x = screenSpaceCorners[i].x;
            }

            if (screenSpaceCorners[i].y < min_y)
            {
                min_y = screenSpaceCorners[i].y;
            }

            if (screenSpaceCorners[i].x > max_x)
            {
                max_x = screenSpaceCorners[i].x;
            }

            if (screenSpaceCorners[i].y > max_y)
            {
                max_y = screenSpaceCorners[i].y;
            }
        }

        return Rect.MinMaxRect(min_x, min_y, max_x, max_y);
    }

    public static Vector3 PointToScreenSpaceBiased(Camera camera, Vector3 point, float nearestDistance)
    {
        Vector3 cameraLocal = camera.transform.InverseTransformPoint(point);
        cameraLocal.z = Mathf.Max(nearestDistance, cameraLocal.z);
        point = camera.transform.TransformPoint(cameraLocal);
        return camera.WorldToScreenPoint(point);
    }

    public static Rect TightRect(Camera camera, Transform transform, MeshFilter meshFilter, float nearestDistance)
    {
        Mesh mesh = meshFilter.mesh;

        var vertices = mesh.vertices;


        Vector3 p = transform.TransformPoint(vertices[0]);
        Vector2 min = PointToScreenSpaceBiased(camera, p, nearestDistance);
        Vector2 max = min;

        // Iterate through all vertices
        // except first one
        for (var i = 1; i < vertices.Length; i++)
        {
            p = transform.TransformPoint(vertices[i]);
            Vector2 v = PointToScreenSpaceBiased(camera, p, nearestDistance);

            // Go through X,Y of the Vector2
            for (var n = 0; n < 2; n++)
            {
                max[n] = Mathf.Max(v[n], max[n]);
                min[n] = Mathf.Min(v[n], min[n]);
            }
        }

        Rect rect = new Rect();
        rect.xMin = min.x;
        rect.yMin = min.y;
        rect.xMax = max.x;
        rect.yMax = max.y;
        return rect;
    }
}