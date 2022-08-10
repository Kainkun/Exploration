using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : SystemSingleton<PlayerHUD>
{
    public RectTransform fuelBar;
    public RectTransform currentFuelBar;
    public GameObject fuelBarBigLine;
    public GameObject fuelBarSmallLine;
    private const float fuelUnitWidth = 100;

    // public Image fuelRing;
    // public Image boostDot1;
    // public Image boostDot2;
    // public Image boostDot3;
    // public Image imageCenterMini;

    private void Start()
    {
        OnJetpackMaxFuelChange();
    }

    public void OnJetpackMaxFuelChange()
    {
        PlayerMovement pm = PlayerMovement.Get();
        
        if (pm.MaxJetpackFuel <= 0)
        {
            fuelBar.gameObject.SetActive(false);
            currentFuelBar.gameObject.SetActive(false);
            return;
        }

        fuelBar.gameObject.SetActive(true);
        currentFuelBar.gameObject.SetActive(true);

        fuelBar.sizeDelta = new Vector2(fuelUnitWidth * pm.MaxJetpackFuel, fuelBar.sizeDelta.y);

        foreach (Transform child in currentFuelBar.transform)
            Destroy(child.gameObject);


        int maxBoosts = Mathf.FloorToInt(pm.MaxJetpackFuel / pm.boostFuelCost);
        if (maxBoosts > 0)
        {
            RectTransform bigLineLeft =
                Instantiate(fuelBarBigLine, currentFuelBar).GetComponent<RectTransform>();
            RectTransform bigLineRight =
                Instantiate(fuelBarBigLine, currentFuelBar).GetComponent<RectTransform>();

            bigLineLeft.localPosition = new Vector2(-fuelUnitWidth / 2, 0);
            bigLineRight.localPosition = new Vector2(fuelUnitWidth / 2, 0);

            for (int i = 2; i <= maxBoosts; i++)
            {
                RectTransform smallLineLeft =
                    Instantiate(fuelBarSmallLine, currentFuelBar).GetComponent<RectTransform>();
                RectTransform smallLineRight =
                    Instantiate(fuelBarSmallLine, currentFuelBar).GetComponent<RectTransform>();
            
                smallLineLeft.anchoredPosition = new Vector2(-fuelUnitWidth * i / 2, 0);
                smallLineRight.anchoredPosition = new Vector2(fuelUnitWidth * i / 2, 0);
            }
        }

        OnJetpackCurrentFuelChange();
    }

    public void OnJetpackCurrentFuelChange()
    {
        PlayerMovement pm = PlayerMovement.Get();

        float fuelPercent;
        if (pm.MaxJetpackFuel > 0)
            fuelPercent = pm.CurrentJetpackFuel / pm.MaxJetpackFuel;
        else
            fuelPercent = 0;

        currentFuelBar.sizeDelta =
            new Vector2(fuelPercent * (fuelUnitWidth * pm.MaxJetpackFuel), currentFuelBar.sizeDelta.y);

        // if (currentJetpackFuel == 0)
        //     fuelRing.fillAmount = 0;
        // else if (currentJetpackFuel % boostFuelCost == 0)
        //     fuelRing.fillAmount = 1;
        // else
        //     fuelRing.fillAmount = (currentJetpackFuel % boostFuelCost) / boostFuelCost;
        //
        // if (Mathf.CeilToInt(currentJetpackFuel / boostFuelCost) >= 1)
        //     boostDot1.enabled = true;
        // else
        //     boostDot1.enabled = false;
        //
        // if (Mathf.CeilToInt(currentJetpackFuel / boostFuelCost) >= 2)
        //     boostDot2.enabled = true;
        // else
        //     boostDot2.enabled = false;
        //
        // if (Mathf.CeilToInt(currentJetpackFuel / boostFuelCost) >= 3)
        //     boostDot3.enabled = true;
        // else
        //     boostDot3.enabled = false;
        //
        // imageCenterMini.fillAmount = currentJetpackFuel / maxJetpackFuel;
    }
}