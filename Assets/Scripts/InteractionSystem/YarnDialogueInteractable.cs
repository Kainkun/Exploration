using System;
using Antlr4.Runtime.Atn;
using UnityEngine;

public class YarnDialogueInteractable : MonoBehaviour, IInteractable
{
    public string startNode;
    public BoxCollider playerInteractionBound;

    private Utility.CheckBoxData triggerCheckBoxData;

    private void Start()
    {
        if (playerInteractionBound)
            triggerCheckBoxData = Utility.GetCheckBoxData(playerInteractionBound);
    }

    public virtual void PrimaryInteract()
    {
        if (playerInteractionBound && !Physics.CheckBox(
                triggerCheckBoxData.triggerGlobalPosition,
                triggerCheckBoxData.triggerHalfExtent,
                triggerCheckBoxData.triggerRotation,
                LayerMask.GetMask("Player"),
                QueryTriggerInteraction.Ignore))
            return;

        if (!YarnAccess.dialogueRunner.IsDialogueRunning)
        {
            YarnAccess.dialogueRunner.StartDialogue(startNode);
            
            // Cursor.lockState = CursorLockMode.None;
            
            YarnAccess.dialogueRunner.onDialogueComplete.AddListener(EndDialogue);
        }
    }

    private void EndDialogue()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        
        YarnAccess.dialogueRunner.onDialogueComplete.RemoveListener(EndDialogue);
    }
}