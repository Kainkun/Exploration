using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUpgradeCollectable : MonoBehaviour, IInteractable
{
    public PlayerUpgradeType playerUpgradeType;
    public UnityEvent unityEvent;
    
    public enum PlayerUpgradeType
    {
        UnlockMultiTool,
        UnlockTrashCollector,
        UnlockEssenceCollector,
        UnlockJetpack,
        UnlockJetpackBoost
    }

    public void PrimaryInteract()
    {
        switch (playerUpgradeType)
        {
            case PlayerUpgradeType.UnlockMultiTool:
                PlayerMultiTool.Singleton.ActivateMultiTool();
                break;

            case PlayerUpgradeType.UnlockTrashCollector:
                PlayerMultiTool.Singleton.UnlockModule(PlayerMultiTool.ModuleType.Trash);
                break;

            case PlayerUpgradeType.UnlockEssenceCollector:
                PlayerMultiTool.Singleton.UnlockModule(PlayerMultiTool.ModuleType.Essence);
                break;

            case PlayerUpgradeType.UnlockJetpack:
                PlayerMovement.Singleton.UnlockGlide();
                break;

            case PlayerUpgradeType.UnlockJetpackBoost:
                PlayerMovement.Singleton.UnlockBoost();
                break;
        }
        
        unityEvent?.Invoke();

        Destroy(gameObject);
    }
}