using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private static float requiredTrashForJobToken = 10;

    private Hologram myHologram;
    private static readonly List<Hologram> Holograms = new List<Hologram>();


    private void Awake()
    {
        myHologram = GetComponentInChildren<Hologram>();
        myHologram.OnTurnOn += GetDisplayText;
        Holograms.Add(myHologram);
    }
    
    private void OnDestroy()
    {
        myHologram.OnTurnOn -= GetDisplayText;
    }

    public static string GetDisplayText()
    {
        YarnAccess.TryGetValue("depositedTrashCount", out float currentDepositedTrashCount);
        return currentDepositedTrashCount + " total trash deposited\n";
    }

    public static void Deposit(float trashCount)
    {
        if (trashCount <= 0)
            return;

        YarnAccess.TryGetValue("depositedTrashCount", out float currentDepositedTrashCount);
        float newDepositedTrashCount = currentDepositedTrashCount + trashCount;

        int currentTokensFromTrash = Mathf.FloorToInt(currentDepositedTrashCount / requiredTrashForJobToken);
        int newTokensFromTrash =
            Mathf.FloorToInt(newDepositedTrashCount / requiredTrashForJobToken) - currentTokensFromTrash;
        YarnAccess.AddValue("jobTokenCount", newTokensFromTrash);
        YarnAccess.SetValue("depositedTrashCount", newDepositedTrashCount);

        foreach (Hologram hologram in Holograms)
        {
            hologram.SetText("Deposited " + trashCount + " trash\n" +
                             newDepositedTrashCount + " total deposited\n" +
                             newTokensFromTrash + " jobTokens added\n");
        }
    }
}