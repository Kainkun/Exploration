using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTester : MonoBehaviour
{
    private Vector3 enterPosition;
    private Vector3 exitPosition;

    private void OnCollisionEnter(Collision collision)
    {
        enterPosition = collision.contacts[0].point;
    }

    private void OnCollisionStay(Collision other)
    {
        exitPosition = other.contacts[0].point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(enterPosition, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(exitPosition, 0.1f);
    }
}