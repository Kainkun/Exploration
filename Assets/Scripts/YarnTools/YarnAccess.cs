using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.EventSystems;
using Yarn;

public class YarnAccess : MonoBehaviour
{
    [HideInInspector] public static DialogueRunner dialogueRunner;
    [HideInInspector] public static InMemoryVariableStorage inMemoryVariableStorage;
    [HideInInspector] public static LineView lineView;
    public static OptionView OptionView => EventSystem.current.currentSelectedGameObject.GetComponent<OptionView>();

    private void Start()
    {
        dialogueRunner = GetComponent<DialogueRunner>();
        inMemoryVariableStorage = GetComponent<InMemoryVariableStorage>();
        lineView = GetComponentInChildren<LineView>();
    }


    public static void SetValue(string variableName, string stringValue)
    {
        inMemoryVariableStorage.SetValue("$" + variableName, stringValue);
    }

    public static void SetValue(string variableName, float floatValue)
    {
        inMemoryVariableStorage.SetValue("$" + variableName, floatValue);
    }

    public static void AddValue(string variableName, float floatToAdd)
    {
        TryGetValue(variableName, out float currentValue);
        inMemoryVariableStorage.SetValue("$" + variableName, currentValue + floatToAdd);
    }

    public static void SetValue(string variableName, bool boolValue)
    {
        inMemoryVariableStorage.SetValue("$" + variableName, boolValue);
    }

    public static bool TryGetValue<T>(string variableName, out T result)
    {
        bool variableExists = inMemoryVariableStorage.TryGetValue("$" + variableName, out T r);
        result = r;
        return variableExists;
    }
}