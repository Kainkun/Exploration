using UnityEngine;

public class YarnDialogueInteractable : MonoBehaviour, IInteractable
{
    public string startNode;
    public BoxCollider playerInteractionBound;

    public virtual void PrimaryInteract()
    {
        if (playerInteractionBound)
            if (!Physics.CheckBox(
                    playerInteractionBound.transform.position + playerInteractionBound.center,
                    playerInteractionBound.size / 2,
                    playerInteractionBound.transform.rotation,
                    LayerMask.GetMask("Player")))
                return;

        if (!YarnAccess.dialogueRunner.IsDialogueRunning)
            YarnAccess.dialogueRunner.StartDialogue(startNode);
    }
}