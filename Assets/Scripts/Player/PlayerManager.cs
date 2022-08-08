using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static CinemachineVirtualCamera playerCamera;
    public static PlayerMovement playerMovement;
    public static PlayerMultiTool playerMultiTool;

    public enum PlayerUpgradeType
    {
        UnlockMultiTool,
        UnlockTrashCollector,
        UnlockEssenceCollector,
        UnlockJetpack,
        UnlockJetpackBoost
    }

    private void Awake()
    {
        playerCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMultiTool = GetComponentInChildren<PlayerMultiTool>();
    }

    public static void ToggleNoclip() => playerMovement.ToggleNoclip();

    public static void UnlockMultiTool() => playerMultiTool.Activate();

    public static void UnlockTrashCollector() => playerMultiTool.UnlockTrashCollector();

    public static void UnlockEssenceCollector() => playerMultiTool.UnlockEssenceCollector();

    public static void UnlockJetpackGlide() => playerMovement.canGlide = true;

    public static void UnlockJetpackBoost() => playerMovement.canBoost = true;

    public static void PickUpTrash(float amount) => YarnAccess.AddValue("trashCount", amount);

    public static void PickUpEssence(float amount) => YarnAccess.AddValue("essenceCount", amount);
    
    public static void GetJobToken(float amount) => YarnAccess.AddValue("jobTokenCount", amount);

}