using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Npc : MonoBehaviour, IInteractable
{
    public string startNode;
    DialogueRunner dialogueRunner;

    public void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
    
    public void PrimaryInteract()
    {
        dialogueRunner.StartDialogue(startNode);
    }
}
