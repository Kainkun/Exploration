using System;
using Antlr4.Runtime.Atn;
using UnityEngine;

public class YarnDialogueInteractable : MonoBehaviour, IInteractable
{
    public bool ignoreInteraction;
    public string startNode;
    public BoxCollider playerInteractionBound;

    private Utility.CheckBoxData triggerCheckBoxData;

    protected virtual void Awake()
    {
        if (playerInteractionBound)
            triggerCheckBoxData = Utility.GetCheckBoxData(playerInteractionBound);
    }

    public virtual void PrimaryInteract()
    {
        if(ignoreInteraction)
            return;
        
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

    protected virtual void EndDialogue()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        
        YarnAccess.dialogueRunner.onDialogueComplete.RemoveListener(EndDialogue);
    }
}