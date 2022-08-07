using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeCollectable : MonoBehaviour, IInteractable
{
    public PlayerManager.PlayerUpgradeType playerUpgradeType;

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

        Destroy(gameObject);
    }
}