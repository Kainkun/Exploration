using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class PlayerHUD : MonoBehaviour
{
    public Image crossHair;
    private float dim;
    private float bright = 255;

    public RectTransform fuelBar;
    public RectTransform currentFuelBar;
    public CanvasGroup fuelBarCanvasGroup;
    public GameObject fuelBarBigLine;
    public GameObject fuelBarSmallLine;
    private const float FuelBoostCostWidth = 100;
    private float fuelBarAlpha;
    private float fuelBarFadeWait = 1f;
    private float fuelBarFadeTime = 0.25f;

    PlayerMovement pm;

    private void Start()
    {
        pm = PlayerMovement.Get();
        pm.onCurrentFuelChange += OnJetpackCurrentFuelChange;
        pm.onMaxFuelChange += OnJetpackMaxFuelChange;
        OnJetpackMaxFuelChange();

        dim = crossHair.color.a;
    }

    private void LateUpdate()
    {
        if (PlayerInteractor.hoveringInteractable || TrashCollectorModule.hoveringTrashBin)
            CrossHairBright();
        else
            CrossHairDim();

        if (pm.CurrentJetpackFuel < pm.MaxJetpackFuel)
        {
            fuelBarAlpha = fuelBarFadeWait / fuelBarFadeTime;
            fuelBarCanvasGroup.alpha = fuelBarAlpha;
        }
        else
        {
            fuelBarAlpha -= Time.deltaTime / fuelBarFadeTime;
            fuelBarCanvasGroup.alpha = fuelBarAlpha;
        }
    }

    public void CrossHairBright()
    {
        Color color = crossHair.color;
        color.a = bright;
        crossHair.color = color;
    }

    public void CrossHairDim()
    {
        Color color = crossHair.color;
        color.a = dim;
        crossHair.color = color;
    }

    public void OnJetpackMaxFuelChange()
    {
        if (pm.MaxJetpackFuel <= 0)
        {
            fuelBar.gameObject.SetActive(false);
            currentFuelBar.gameObject.SetActive(false);
            return;
        }

        fuelBar.gameObject.SetActive(true);
        currentFuelBar.gameObject.SetActive(true);

        fuelBar.sizeDelta = new Vector2(FuelBoostCostWidth * pm.MaxJetpackFuel, fuelBar.sizeDelta.y);

        foreach (Transform child in currentFuelBar.transform)
            Destroy(child.gameObject);


        int maxBoosts = Mathf.FloorToInt(pm.MaxJetpackFuel / pm.boostFuelCost);
        if (maxBoosts > 0)
        {
            RectTransform bigLineLeft =
                Instantiate(fuelBarBigLine, currentFuelBar).GetComponent<RectTransform>();
            RectTransform bigLineRight =
                Instantiate(fuelBarBigLine, currentFuelBar).GetComponent<RectTransform>();

            bigLineLeft.localPosition = new Vector2(-FuelBoostCostWidth / 2, 0);
            bigLineRight.localPosition = new Vector2(FuelBoostCostWidth / 2, 0);

            for (int i = 2; i <= maxBoosts; i++)
            {
                RectTransform smallLineLeft =
                    Instantiate(fuelBarSmallLine, currentFuelBar).GetComponent<RectTransform>();
                RectTransform smallLineRight =
                    Instantiate(fuelBarSmallLine, currentFuelBar).GetComponent<RectTransform>();

                smallLineLeft.anchoredPosition = new Vector2(-FuelBoostCostWidth * i / 2, 0);
                smallLineRight.anchoredPosition = new Vector2(FuelBoostCostWidth * i / 2, 0);
            }
        }

        OnJetpackCurrentFuelChange();
    }

    public void OnJetpackCurrentFuelChange()
    {
        float fuelPercent;
        if (pm.MaxJetpackFuel > 0)
            fuelPercent = pm.CurrentJetpackFuel / pm.MaxJetpackFuel;
        else
            fuelPercent = 0;

        currentFuelBar.sizeDelta =
            new Vector2(fuelPercent * (FuelBoostCostWidth * pm.MaxJetpackFuel), currentFuelBar.sizeDelta.y);
    }
}