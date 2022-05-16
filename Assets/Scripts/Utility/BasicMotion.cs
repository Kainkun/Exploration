using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMotion : MonoBehaviour
{
    [Header("Position Sin")]
    public Vector3 posAmplitude = Vector3.zero;
    public Vector3 posPeriod = Vector3.one;
    public Vector3 posPhase = Vector3.zero;
    
    [Header("Position Noise")]
    public Vector3 posNoiseAmplitude = Vector3.zero;
    public Vector3 posNoiseSpeed = Vector3.one;
    public Vector3 posNoiseOffset = Vector3.zero;

    [Header("Rotation Sin")]
    public Vector3 rotAmplitude = Vector3.zero;
    public Vector3 rotPeriod = Vector3.one;
    public Vector3 rotPhase = Vector3.zero;
    
    [Header("Position Noise")]
    public Vector3 rotNoiseAmplitude = Vector3.zero;
    public Vector3 rotNoiseSpeed = Vector3.one;
    public Vector3 rotNoiseOffset = Vector3.zero;

    [Header("Rotation Continuous")]
    public Vector3 rotate;

    private Vector3 startPosition;
    private Vector3 startRotation;

    private void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localEulerAngles;
    }

    void Update()
    {
        Transform t = transform;
        
        Vector3 translation = Vector3.zero;
        translation.x += posAmplitude.x * Mathf.Sin((2 * Mathf.PI * (Time.time + posPhase.x)) / posPeriod.x);
        translation.y += posAmplitude.y * Mathf.Sin((2 * Mathf.PI * (Time.time + posPhase.y)) / posPeriod.y);
        translation.z += posAmplitude.z * Mathf.Sin((2 * Mathf.PI * (Time.time + posPhase.z)) / posPeriod.z);
        translation.x += posNoiseAmplitude.x * Mathf.PerlinNoise((Time.time + posNoiseOffset.x) * posNoiseSpeed.x, 100);
        translation.y += posNoiseAmplitude.y * Mathf.PerlinNoise((Time.time + posNoiseOffset.y) * posNoiseSpeed.y, 100);
        translation.z += posNoiseAmplitude.z * Mathf.PerlinNoise((Time.time + posNoiseOffset.z) * posNoiseSpeed.z, 100);
        Vector3 position = startPosition;
        position += (translation.x * t.right);
        position += (translation.y * t.up);
        position += (translation.z * t.forward);
        t.localPosition = position;

        Vector3 newRot = startRotation;
        newRot.x += (rotAmplitude.x / 2) * Mathf.Sin((2 * Mathf.PI * (Time.time + rotPhase.x)) / rotPeriod.x);
        newRot.y += (rotAmplitude.y / 2) * Mathf.Sin((2 * Mathf.PI * (Time.time + rotPhase.y)) / rotPeriod.y);
        newRot.z += (rotAmplitude.z / 2) * Mathf.Sin((2 * Mathf.PI * (Time.time + rotPhase.z)) / rotPeriod.z);
        newRot.x += rotNoiseAmplitude.x * Mathf.PerlinNoise(100, (Time.time + rotNoiseOffset.x) * rotNoiseSpeed.x);
        newRot.y += rotNoiseAmplitude.y * Mathf.PerlinNoise(100, (Time.time + rotNoiseOffset.y) * rotNoiseSpeed.y);
        newRot.z += rotNoiseAmplitude.z * Mathf.PerlinNoise(100, (Time.time + rotNoiseOffset.z) * rotNoiseSpeed.z);
        newRot.x += rotate.x * Time.time;
        newRot.y += rotate.y * Time.time;
        newRot.z += rotate.z * Time.time;
        transform.localEulerAngles = newRot;
    }
}