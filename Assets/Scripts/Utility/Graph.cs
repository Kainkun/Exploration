using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    //GameObject.Find("GraphVelocityY").GetComponent<Graph>().SetValue(transform.position.y);
    
    public Color color = Color.red;
    public float width = 0.01f;
    public float minVertDistance = 0.01f;
    private TrailRenderer t;

    void Start()
    {
        t = gameObject.GetComponent<TrailRenderer>();
        t.startColor = color;
        t.endColor = color;
        t.startWidth = width;
        t.endWidth = width;
        t.time = Single.PositiveInfinity;
        t.minVertexDistance = minVertDistance;
        t.enabled = false;
        transform.position = Vector3.zero;
        t.enabled = true;
    }

    private void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
    }

    public void SetValue(float value)
    {
        var t = transform;
        Vector3 p = t.position;
        p.y = value;
        t.position = p;
    }
}