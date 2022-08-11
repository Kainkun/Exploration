using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image crossHair;
    private float dim;
    private float bright = 255;

    public GameObject inventoryItem;



    public CanvasGroup inventoryGroup;
    public Transform stackingInventoryList;

    public Transform uniqueInventoryList;

    // public TextMeshProUGUI trashCount;
    // public TextMeshProUGUI essenceCount;
    // public TextMeshProUGUI jobTokenCount;
    private bool showInventory;
    private float inventoryAlpha;
    private float inventoryFadeTime = 0.25f;

    PlayerMovement pm;
    public RectTransform fuelBar;
    public RectTransform currentFuelBar;
    public CanvasGroup fuelBarGroup;
    public GameObject fuelBarBigLine;
    public GameObject fuelBarSmallLine;
    private const float FuelBoostCostWidth = 100;
    private float fuelBarAlpha;
    private float fuelBarFadeWait = 1f;
    private float fuelBarFadeTime = 0.25f;

    private void Start()
    {
        var a = (1, 2);
        pm = PlayerMovement.Singleton;
        pm.onCurrentFuelChange += OnJetpackCurrentFuelChange;
        pm.onMaxFuelChange += OnJetpackMaxFuelChange;
        OnJetpackMaxFuelChange();

        YarnAccess.onSetFloat += CollectStackable;
        YarnAccess.onSetBool += CollectUnique;

        InputManager.Singleton.showInventory += (f) => { showInventory = f > 0 ? true : false; };

        dim = crossHair.color.a;
    }

    void CollectStackable(string s, float f)
    {
        var dict = YarnAccess.Singleton.stackingInventoryDict;
        
        if (dict.ContainsKey(s))
        {
            if (dict[s].textMeshPro == null)
                dict[s].CreateGui(inventoryItem, stackingInventoryList);
            dict[s].SetGuiText(f);
        }
    }

    void CollectUnique(string s, bool b)
    {
        var dict = YarnAccess.Singleton.uniqueInventoryDict;

        if (dict.ContainsKey(s))
        {
            if (b == true && dict[s].textMeshPro == null)
                dict[s].CreateGui(inventoryItem, uniqueInventoryList);
            else if (b == false && dict[s].textMeshPro != null)
                dict[s].RemoveGui();
        }
    }

    // switch (s)
    // {
    //     case "trashCount":
    //         trashCount.gameObject.SetActive(true);
    //         trashCount.text = "Trash: " + f;
    //         break;
    //     case "essenceCount":
    //         essenceCount.gameObject.SetActive(true);
    //         essenceCount.text = "Essence: " + f;
    //         break;
    //
    //     case "hasJetpackKey":
    //
    //         break;
    //     case "libraryMachineFixer":
    //
    //         break;
    //     case "hasJohnRoomKey":
    //
    //         break;
    // }

    // TrashCollectable.onCollect += f =>
    // {
    //     trashCount.gameObject.SetActive(true);
    //     trashCount.text = "Trash: " + f;
    // };
    // EssenceCollectable.onCollect += f =>
    // {
    //     essenceCount.gameObject.SetActive(true);
    //     essenceCount.text = "Essence: " + f;
    // };
    // YarnSingletonCommands.onEarnJobToken += f =>
    // {
    //     jobTokenCount.gameObject.SetActive(true);
    //     jobTokenCount.text = "Work Tokens: " + f;
    // };

    private void LateUpdate()
    {
        if (PlayerInteractor.hoveringInteractable || TrashCollectorModule.hoveringTrashBin)
            CrossHairBright();
        else
            CrossHairDim();

        if (showInventory)
            inventoryAlpha += Time.deltaTime / inventoryFadeTime;
        else
            inventoryAlpha -= Time.deltaTime / inventoryFadeTime;
        inventoryAlpha = Mathf.Clamp01(inventoryAlpha);
        inventoryGroup.alpha = inventoryAlpha;


        if (pm.CurrentJetpackFuel < pm.MaxJetpackFuel)
        {
            fuelBarAlpha = fuelBarFadeWait / fuelBarFadeTime;
            fuelBarGroup.alpha = fuelBarAlpha;
        }
        else
        {
            fuelBarAlpha -= Time.deltaTime / fuelBarFadeTime;
            fuelBarGroup.alpha = fuelBarAlpha;
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