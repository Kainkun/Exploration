using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMultiTool : MonoBehaviour
{
    public GameObject baseMesh;
    public Transform muzzle;
    public LayerMask raycastLayerMask;

    private bool isMultiToolActive;
    private int currentModuleCount = 0;
    private ModuleType currentModule = ModuleType.None;
    private Dictionary<ModuleType, MultiToolModule> moduleDict;

    private void Start()
    {
        moduleDict = new Dictionary<ModuleType, MultiToolModule>()
        {
            { ModuleType.None, new NoneModule(this) },
            { ModuleType.Trash, new TrashCollectorModule(this) },
            { ModuleType.Essence, new EssenceCollectorModule(this) }
        };
        
        InputManager.Get().Primary += () => moduleDict[currentModule].UsePrimary();
    }

    public enum ModuleType
    {
        None,
        Trash,
        Essence
    };

    public void ActivateMultiTool()
    {
        if (isMultiToolActive)
            return;

        YarnAccess.SetValue("hasMultiTool", true);
        baseMesh.SetActive(true);
        isMultiToolActive = true;

        moduleDict[currentModule].Intro();
    }

    void Update()
    {
        if (!isMultiToolActive)
            return;

        if (currentModuleCount >= 1)
        {
            if (Mouse.current.scroll.ReadValue().y > 0)
                SwitchMultiToolMode((ModuleType)Utility.RealModulo((int)currentModule + 1, currentModuleCount + 1));
            else if (Input.mouseScrollDelta.y < 0)
                SwitchMultiToolMode((ModuleType)Utility.RealModulo((int)currentModule - 1, currentModuleCount + 1));
        }

        moduleDict[currentModule].Update();
    }

    public void UnlockModule(ModuleType moduleType)
    {
        if (moduleDict[moduleType].Unlock())
        {
            currentModuleCount++;
            if (isMultiToolActive)
                SwitchMultiToolMode(moduleType);
        }
    }

    public void SwitchMultiToolMode(ModuleType moduleType)
    {
        moduleDict[currentModule].Outro();
        currentModule = moduleType;
        moduleDict[currentModule].Intro();
    }
}