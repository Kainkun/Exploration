using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : SystemSingleton<InputManager>
{
    private static PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform.gameObject);
    }


    // public InputActionMap[] maps;
    // public InputActionAsset actions;
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        SceneManager.sceneLoaded += HandleLoadScene;

        // maps = playerInput.actions.actionMaps.ToArray();
        // print(playerInput.defaultActionMap);
        // print(playerInput.actions);
        // foreach (InputActionMap inputActionMap in playerInput.actions.actionMaps)
        // {
        //     print(inputActionMap.name);
        // }


        // inputslist.IndexOf("Player");
        //
        //
        // inputStack.Add(inputslist.IndexOf("Player"));
        //
        // _inputPriority.Add(new Object(), "test4");
        // _inputPriority.Add(new Object(), "test3");
        // _inputPriority.Add(new Object(), "test2");
        // _inputPriority.Add(new Object(), "test222");
        // _inputPriority.Add(new Object(), "test5");
        // _inputPriority.Add(new Object(), "test1");
        // for (int i = 0; i < _inputPriority.Count; i++)
        // {
        //     print(_inputPriority.Values[i]);
        // }
        // print(_inputPriority.Remove(new Object()));
    }

    // public bool AddInput(string actionMapName, int priority)
    // {
    //     if (priority < _inputPriority.Keys[_inputPriority.Count])
    //         return false;
    //     
    //     _inputPriority.Add(priority, actionMapName);
    //     playerInput.SwitchCurrentActionMap(_inputPriority.Values[_inputPriority.Count]);
    //
    //     return true;
    // }

    // public void RemoveInput(string actionMapName)
    // {
    //     _inputPriority.IndexOfValue("test4");
    //     playerInput.SwitchCurrentActionMap(_inputPriority.Values[_inputPriority.Count]);
    // }

    public static void HandleLoadScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 0)
            playerInput.SwitchCurrentActionMap("UI");
        else
            playerInput.SwitchCurrentActionMap("Player");
    }

    public void OnPlayerPause()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            playerInput.SwitchCurrentActionMap("UI");
            GameManager.TogglePauseUI(true);
            GameManager.TogglePause(true);
        }
    }

    public void OnUnpause()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            playerInput.SwitchCurrentActionMap("Player");
            GameManager.TogglePauseUI(false);
            GameManager.TogglePause(false);
        }
    }

    public void OnBack()
    {
        if (GameManager.overlaySettings.activeSelf)
        {
            GameManager.ToggleSettingsUI(false);
        }
        else if(GameManager.overlayPause.activeSelf)
        {
            OnUnpause();
        }
        else if (GameManager.overlayCredits.activeSelf)
        {
            GameManager.ToggleCreditsUI(false);
        }
    }

    public Action<float> Jump;
    public void OnJump(InputValue value)
    {
        Jump?.Invoke(value.Get<float>());
    }

    public Action<float> Crouch;
    public void OnCrouch(InputValue value)
    {
        Crouch?.Invoke(value.Get<float>());
    }

    public Action<Vector2> Move;
    public void OnMove(InputValue value)
    {
        Move?.Invoke(value.Get<Vector2>());
    }

    public Action<Vector2> Look;
    public void OnLook(InputValue value)
    {
        Look?.Invoke(value.Get<Vector2>());
    }
    
    public Action Primary;
    public void OnPrimary() => Primary?.Invoke();
    
    public Action Secondary;
    public void OnSecondary() => Secondary?.Invoke();
    
    public Action Tertiary;
    public void OnTertiary() => Tertiary?.Invoke();
}
