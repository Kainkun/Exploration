using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Yarn.Unity;

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

    [EasyButtons.Button]
    public static void UnlockMultiTool() => playerMultiTool.Activate();

    [EasyButtons.Button]
    public static void UnlockTrashCollector() => playerMultiTool.UnlockTrashCollector();

    [EasyButtons.Button]
    public static void UnlockEssenceCollector() => playerMultiTool.UnlockEssenceCollector();

    [EasyButtons.Button]
    public static void UnlockJetpackGlide() => playerMovement.canGlide = true;

    [EasyButtons.Button]
    public static void UnlockJetpackBoost() => playerMovement.canBoost = true;

    [EasyButtons.Button]
    public static void PickUp5Trash()
    {
        YarnAccess.TryGetValue("trashCount", out float result);
        result += 5;
        YarnAccess.SetValue("trashCount", result);
    }
}