using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class InputManager : SystemSingleton<InputManager>
{
    public static PlayerInput playerInput;
    public static InputSystemUIInputModule inputSystemUIInputModule;

    protected override void Awake()
    {
        base.Awake();
        
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        inputSystemUIInputModule = GameObject.FindObjectOfType<InputSystemUIInputModule>();
        SceneManager.sceneLoaded += HandleLoadScene;
        
        InputManager.inputSystemUIInputModule.enabled = false;
    }


    public static void HandleLoadScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void OnPlayerPause()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            playerInput.SwitchCurrentActionMap("UI");
            GameManager.TogglePauseUI(true);
            GameManager.TogglePause(true);
        }
    }

    public void OnUnpause()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
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
        else if (GameManager.overlayPause.activeSelf)
        {
            OnUnpause();
        }
        else if (GameManager.overlayCredits.activeSelf)
        {
            GameManager.ToggleCreditsUI(false);
        }
    }

    public static Action<float> jump;
    public void OnJump(InputValue value) => jump?.Invoke(value.Get<float>());

    public static Action<float> crouch;
    public void OnCrouch(InputValue value) => crouch?.Invoke(value.Get<float>());

    public static Action<Vector2> move;
    public void OnMove(InputValue value) => move?.Invoke(value.Get<Vector2>());

    public static Action<Vector2> look;
    public void OnLook(InputValue value) => look?.Invoke(value.Get<Vector2>());

    public static Action<float> sprint;
    public void OnSprint(InputValue value) => sprint?.Invoke(value.Get<float>());

    public static Action primary;
    public void OnPrimary() => primary?.Invoke();

    public static Action secondary;
    public void OnSecondary() => secondary?.Invoke();

    public static Action tertiary;
    public void OnTertiary() => tertiary?.Invoke();
    
    public static Action use;
    public void OnUse() => use?.Invoke();

    public static Action<float> showInventory;
    public void OnShowInventory(InputValue value) => showInventory?.Invoke(value.Get<float>());

    public void OnChangeDialogueOption(InputValue value)
    {
        float direction = value.Get<float>();
        
        OptionView optionView = YarnAccess.CurrentOptionView;
        if (optionView && optionView.IsInteractable())
        {
            AxisEventData axisEventData = new AxisEventData(EventSystem.current);

            axisEventData.Reset();
            axisEventData.moveVector = new Vector2(0, direction);
            axisEventData.moveDir = direction > 0 ? MoveDirection.Up : MoveDirection.Down;

            ExecuteEvents.Execute(optionView.gameObject, axisEventData, ExecuteEvents.moveHandler);
        }
    }
    
    public void OnChooseDialogueOption()
    {
        OptionView optionView = YarnAccess.CurrentOptionView;
        if (optionView && optionView.IsInteractable())
            YarnAccess.CurrentOptionView.InvokeOptionSelected();
        else
            YarnAccess.lineView.UserRequestedViewAdvancement();
    }


}