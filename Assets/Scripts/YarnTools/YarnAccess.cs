using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.EventSystems;
using Yarn;

public class YarnAccess : SystemSingleton<YarnAccess>
{
    public static DialogueRunner dialogueRunner;
    public static InMemoryVariableStorage inMemoryVariableStorage;
    public static LineView lineView;
    public static OptionsListView optionsListView;
    
    public Dictionary<string, UniqueYarnData> uniqueInventoryDict = new Dictionary<string, UniqueYarnData>();
    public Dictionary<string, YarnStackableData> stackingInventoryDict = new Dictionary<string, YarnStackableData>();

    public static Action<string, float> onSetFloat;
    public static Action<string, bool> onSetBool;

    public static OptionView CurrentOptionView
    {
        get
        {
            GameObject gameObject = EventSystem.current.currentSelectedGameObject;
            if (gameObject)
            {
                OptionView optionView = gameObject.GetComponent<OptionView>();
                if (optionView)
                    return optionView;
            }

            return null;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        dialogueRunner = GetComponent<DialogueRunner>();
        inMemoryVariableStorage = GetComponent<InMemoryVariableStorage>();
        lineView = GetComponentInChildren<LineView>();
        optionsListView = GetComponentInChildren<OptionsListView>();
        
        stackingInventoryDict.Add("jobTokenCount", new YarnStackableData("Work Token", "Work Tokens"));
        stackingInventoryDict.Add("essenceCount", new YarnStackableData("Essence", "Essence"));
        stackingInventoryDict.Add("trashCount", new YarnStackableData("Piece of Trash", "Pieces of Trash"));

        uniqueInventoryDict.Add("hasMultiTool", new UniqueYarnData("MultiTool"));
        uniqueInventoryDict.Add("hasTrashCollector", new UniqueYarnData("Trash Collector"));
        uniqueInventoryDict.Add("hasEssenceCollector", new UniqueYarnData("Essence Collector"));
        uniqueInventoryDict.Add("hasJetpackKey", new UniqueYarnData("Jetpack Key"));
        uniqueInventoryDict.Add("hasJetpackGlide", new UniqueYarnData("Jetpack"));
        uniqueInventoryDict.Add("hasJetpackBoost", new UniqueYarnData("Jetpack Boost Upgrade"));
        uniqueInventoryDict.Add("libraryMachineFixer", new UniqueYarnData("Library Machine Fixer"));
        uniqueInventoryDict.Add("hasJohnRoomKey", new UniqueYarnData("John's Room Key"));
        uniqueInventoryDict.Add("hasStandardKeycard", new UniqueYarnData("Standard Access KeyCard"));
    }

    private void OnApplicationQuit()
    {
        onSetFloat = null;
        onSetBool = null;
    }
    
    [YarnCommand("SetString")]
    public static void SetValue(string variableName, string stringValue)
    {
        inMemoryVariableStorage.SetValue("$" + variableName, stringValue);
    }

    [YarnCommand("SetFloat")]
    public static void SetValue(string variableName, float floatValue)
    {
        inMemoryVariableStorage.SetValue("$" + variableName, floatValue);
        onSetFloat?.Invoke(variableName, floatValue);
    }

    [YarnCommand("AddFloat")]
    public static void AddValue(string variableName, float floatToAdd)
    {
        TryGetValue(variableName, out float currentValue);
        SetValue(variableName, currentValue + floatToAdd);
    }

    [YarnCommand("SetBool")]
    public static void SetValue(string variableName, bool boolValue)
    {
        inMemoryVariableStorage.SetValue("$" + variableName, boolValue);
        onSetBool?.Invoke(variableName, boolValue);
    }

    public static bool TryGetValue<T>(string variableName, out T result)
    {
        bool variableExists = inMemoryVariableStorage.TryGetValue("$" + variableName, out T r);
        result = r;
        return variableExists;
    }
    
    
    
    public class UniqueYarnData
    {
        public string displayText;
        public TextMeshProUGUI textMeshPro;

        public UniqueYarnData(string displayText)
        {
            this.displayText = displayText;
            textMeshPro = null;
        }

        public void CreateGui(GameObject guiItem, Transform parent)
        {
            textMeshPro = Instantiate(guiItem, parent).GetComponent<TextMeshProUGUI>();
            textMeshPro.text = displayText;
        }

        public void RemoveGui()
        {
            Destroy(textMeshPro.gameObject);
            textMeshPro = null;
        }
    }

    public class YarnStackableData
    {
        public string displayTextSingular;
        public string displayTextPlural;
        public TextMeshProUGUI textMeshPro;

        public YarnStackableData(string displayTextSingular, string displayTextPlural)
        {
            this.displayTextSingular = displayTextSingular;
            this.displayTextPlural = displayTextPlural;
            textMeshPro = null;
        }

        public void CreateGui(GameObject guiItem, Transform parent) =>
            textMeshPro = Instantiate(guiItem, parent).GetComponent<TextMeshProUGUI>();

        public void SetGuiText(float f)
        {
            if (Math.Abs(f - 1) < 0.01f)
                textMeshPro.text = displayTextSingular + ": " + f;
            else
                textMeshPro.text = displayTextPlural + ": " + f;
        }
    }


}