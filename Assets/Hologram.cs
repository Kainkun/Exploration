using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Hologram : MonoBehaviour
{
    public float distanceToDisplay;

    public bool followDistance;
    public float minDistance;
    public float maxDistance;
    public float distanceOffset;
    public float distanceSmoothTime;
    public float distanceMaxSpeed;

    public bool followDirection;
    public float directionSmoothTime;
    public float directionMaxSpeed;

    [TextArea] public string textOverride;

    private Coroutine coroutine;
    private bool displaying;
    private float distanceVelocity;
    private float angleVelocity;
    private RectTransform text;
    private TextMeshPro textMeshPro;
    private float distance;

    public delegate string TurnOnHandler();

    private static List<Hologram> _holograms = new List<Hologram>();
    public event TurnOnHandler OnTurnOn;

    private void Awake()
    {
        _holograms.Add(this);
        text = transform.GetChild(0).GetComponent<RectTransform>();
        textMeshPro = text.GetComponent<TextMeshPro>();

        Color color = textMeshPro.color;
        color.a = 0;
        textMeshPro.color = color;
    }

    public static void RefreshHolograms()
    {
        foreach (Hologram hologram in _holograms)
            hologram.SetText(hologram.OnTurnOn?.Invoke());
    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(PlayerManager.playerCamera.transform.position, transform.position);
        if (!displaying && distance <= distanceToDisplay)
        {
            displaying = true;
            string s = textOverride;
            if (OnTurnOn != null)
                s = OnTurnOn.Invoke();
            textMeshPro.text = s;
            if (coroutine != null)
                StopCoroutine(coroutine);
            StartCoroutine(TurnOn());
        }
        else if (displaying && distance > distanceToDisplay)
        {
            displaying = false;
            if (coroutine != null)
                StopCoroutine(coroutine);
            StartCoroutine(TurnOff());
        }


        if (followDirection)
        {
            Utility.YAxisLookTowardsSmoothDamp(transform, PlayerManager.playerCamera.transform.position,
                ref angleVelocity, directionSmoothTime, directionMaxSpeed, Time.fixedDeltaTime);
        }

        if (followDistance)
        {
            Vector3 playerFlatPosition = PlayerManager.playerCamera.transform.position;
            playerFlatPosition.y = 0;

            Vector3 flatPosition = transform.position;
            flatPosition.y = 0;

            float flatDistance = Vector3.Distance(flatPosition, playerFlatPosition);
            float clampedFlatDistance = Mathf.Clamp(flatDistance + distanceOffset, minDistance, maxDistance);

            float smoothDistance = Mathf.SmoothDamp(
                text.localPosition.z,
                clampedFlatDistance,
                ref distanceVelocity,
                distanceSmoothTime,
                distanceMaxSpeed);

            text.localPosition = new Vector3(0, 0, smoothDistance);
        }
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }

    IEnumerator TurnOn()
    {
        Color color = textMeshPro.color;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.05f);
            color.a = 0;
            textMeshPro.color = color;
            yield return new WaitForSeconds(0.05f);
            color.a = 255;
            textMeshPro.color = color;
        }
    }

    IEnumerator TurnOff()
    {
        Color color = textMeshPro.color;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.05f);
            color.a = 255;
            textMeshPro.color = color;
            yield return new WaitForSeconds(0.05f);
            color.a = 0;
            textMeshPro.color = color;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToDisplay);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
}