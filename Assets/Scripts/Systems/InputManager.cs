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


    // public InputActionMap[] maps;
    // public InputActionAsset actions;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        inputSystemUIInputModule = GameObject.FindObjectOfType<InputSystemUIInputModule>();
        SceneManager.sceneLoaded += HandleLoadScene;


        //EventSystem.current.SetSelectedGameObject();
        //GameObject.FindObjectOfType<InputSystemUIInputModule>().move;

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     print(EventSystem.current.currentSelectedGameObject);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Y))
        // {
        //     print(EventSystem.current.currentSelectedGameObject);
        //     //EventSystem.current.currentSelectedGameObject.GetComponent<OptionView>().Select();
        //     EventSystem.current.currentSelectedGameObject.GetComponent<OptionView>().InvokeOptionSelected();;
        // }
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

    public Action<float> Jump;
    public void OnJump(InputValue value) => Jump?.Invoke(value.Get<float>());

    public Action<float> Crouch;
    public void OnCrouch(InputValue value) => Crouch?.Invoke(value.Get<float>());

    public Action<Vector2> Move;
    public void OnMove(InputValue value) => Move?.Invoke(value.Get<Vector2>());

    public Action<Vector2> Look;
    public void OnLook(InputValue value) => Look?.Invoke(value.Get<Vector2>());

    public Action<float> Sprint;
    public void OnSprint(InputValue value) => Sprint?.Invoke(value.Get<float>());

    public Action Primary;
    public void OnPrimary() => Primary?.Invoke();

    public Action Secondary;
    public void OnSecondary() => Secondary?.Invoke();

    public Action Tertiary;
    public void OnTertiary() => Tertiary?.Invoke();
    
    public Action Use;
    public void OnUse() => Use?.Invoke();

    public void OnChangeDialogueOption(InputValue value)
    {
        float direction = value.Get<float>();

        // if(EventSystem.current.currentSelectedGameObject == null)
        // {
        //     GameObject gameObject = GameObject.FindObjectOfType<OptionView>().gameObject;
        //     if(gameObject)
        //         EventSystem.current.SetSelectedGameObject(gameObject);
        // }


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