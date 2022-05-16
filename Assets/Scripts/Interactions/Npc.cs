using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Npc : MonoBehaviour, IInteractable
{
    public string startNode;

    public void PrimaryInteract()
    {
        if(!YarnSingleton.Get().dialogueRunner.IsDialogueRunning)
            YarnSingleton.Get().dialogueRunner.StartDialogue(startNode);
    }
}
