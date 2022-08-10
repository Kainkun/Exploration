using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUpgradeCollectable : MonoBehaviour, IInteractable
{
    public PlayerManager.PlayerUpgradeType playerUpgradeType;
    public UnityEvent unityEvent;

    public void PrimaryInteract()
    {
        switch (playerUpgradeType)
        {
            case PlayerManager.PlayerUpgradeType.UnlockMultiTool:
                PlayerManager.UnlockMultiTool();
                break;

            case PlayerManager.PlayerUpgradeType.UnlockTrashCollector:
                PlayerManager.UnlockTrashCollector();
                break;

            case PlayerManager.PlayerUpgradeType.UnlockEssenceCollector:
                PlayerManager.UnlockEssenceCollector();
                break;

            case PlayerManager.PlayerUpgradeType.UnlockJetpack:
                PlayerManager.UnlockJetpackGlide();
                break;

            case PlayerManager.PlayerUpgradeType.UnlockJetpackBoost:
                PlayerManager.UnlockJetpackBoost();
                break;
        }
        
        unityEvent?.Invoke();

        Destroy(gameObject);
    }
}