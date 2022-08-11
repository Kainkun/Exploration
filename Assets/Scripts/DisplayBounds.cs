using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBounds : MonoBehaviour
{
    public bool isOn;
    private Renderer renderer;
    private MeshFilter meshFilter;
    private Camera camera;
    private RectTransform rectUiTransform;
    const float NearestDistance = 0.6f;

    private Image image;
    private TextMeshProUGUI text;

    void Start()
    {
        camera = Camera.main;
        renderer = GetComponent<Renderer>();
        meshFilter = GetComponent<MeshFilter>();

        GameObject g = Instantiate(Resources.Load<GameObject>("TrashBoundsUI"));
        rectUiTransform = g.GetComponent<RectTransform>();
        rectUiTransform.SetParent(GameObject.FindObjectOfType<PlayerHUD>().transform);

        image = g.GetComponentInChildren<Image>();
        text = g.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        if (rectUiTransform)
            Destroy(rectUiTransform.gameObject);
    }

    void FixedUpdate()
    {
        if (isOn)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds)
                && Physics.Raycast(
                    transform.position,
                    PlayerManager.playerCamera.transform
                        .position - transform.position,
                    out RaycastHit hit,
                    Single.PositiveInfinity,
                    ~LayerMask.GetMask(new string[] { "Ignore Raycast", "Collectable", "Transparent" }),
                    QueryTriggerInteraction.Ignore)
                && hit.collider.CompareTag("Player"))
            {
                rectUiTransform.gameObject.SetActive(true);
                Rect rect = Utility.TightRect(camera, transform, meshFilter, NearestDistance);
                rect.size += Vector2.one * 0.2f;

                rectUiTransform.position = rect.center;
                rectUiTransform.sizeDelta = rect.size;

                return;
            }
        }

        rectUiTransform.gameObject.SetActive(false);
    }

    public void Highlight()
    {
        image.color = Color.green;
        text.color = Color.green;
    }
    
    public void Unhighlight()
    {
        image.color = Color.white;
        text.color = Color.white;
    }
}