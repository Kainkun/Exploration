using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DisplayBounds : MonoBehaviour
{
    private Renderer renderer;
    private MeshFilter meshFilter;
    private Camera camera;

    public GameObject rectUi;
    private RectTransform rectUiTransform;
    const float NearestDistance = 0.6f;

    void Start()
    {
        camera = Camera.main;
        renderer = GetComponent<Renderer>();
        meshFilter = GetComponent<MeshFilter>();

        GameObject g = Instantiate(rectUi);
        rectUiTransform = g.GetComponent<RectTransform>();
        rectUiTransform.SetParent(GameObject.Find("Gameplay UI Canvas").transform);
    }

    void FixedUpdate()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
        {
            rectUiTransform.gameObject.SetActive(true);
            Rect rect = Utility.TightRect(camera, transform, meshFilter, NearestDistance);
            rect.size += Vector2.one * 0.2f;


            rectUiTransform.position = rect.center;
            rectUiTransform.sizeDelta = rect.size;
        }
        else
        {
            rectUiTransform.gameObject.SetActive(false);
        }
    }
}