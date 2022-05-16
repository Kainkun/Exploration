using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.EventSystems;
using Yarn;

public class YarnSingleton : SystemSingleton<YarnSingleton>
{
    [HideInInspector]
    public DialogueRunner dialogueRunner;
    [HideInInspector]
    public InMemoryVariableStorage inMemoryVariableStorage;
    [HideInInspector]
    public LineView lineView;
    public OptionView OptionView => EventSystem.current.currentSelectedGameObject.GetComponent<OptionView>();
    
    private void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        inMemoryVariableStorage = GetComponent<InMemoryVariableStorage>();
        lineView = GetComponentInChildren<LineView>();
    }

}
