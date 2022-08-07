using System;
using UnityEngine;

public class YarnDialogueInteractable : MonoBehaviour, IInteractable
{
    public string startNode;
    public BoxCollider playerInteractionBound;

    private void Start()
    {
        if (playerInteractionBound)
            playerInteractionBound.transform.parent = null;
    }

    public virtual void PrimaryInteract()
    {
        if (playerInteractionBound && !Physics.CheckBox(
                playerInteractionBound.transform.position + playerInteractionBound.center,
                playerInteractionBound.size / 2,
                playerInteractionBound.transform.rotation,
                LayerMask.GetMask("Player")))
            return;

        if (!YarnAccess.dialogueRunner.IsDialogueRunning)
            YarnAccess.dialogueRunner.StartDialogue(startNode);
    }
}