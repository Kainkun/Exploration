using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : SystemSingleton<GameManager>
{
    public static GameObject overlayCredits;
    public static GameObject overlayPause;
    public static GameObject overlaySettings;
    public static GameObject eventSystem;

    public static bool applicationIsQuitting = false;

    [RuntimeInitializeOnLoadMethod]
    public static void Spawn()
    {
        DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("InitializeOnLoad/Game Manager")));
        DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("InitializeOnLoad/Input Manager")));
        DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("InitializeOnLoad/Dialogue System")));

        eventSystem = Instantiate(Resources.Load<GameObject>("InitializeOnLoad/Event System"));
        overlayCredits = Instantiate(Resources.Load<GameObject>("InitializeOnLoad/Overlay Credits"));
        overlayPause = Instantiate(Resources.Load<GameObject>("InitializeOnLoad/Overlay Pause"));
        overlaySettings = Instantiate(Resources.Load<GameObject>("InitializeOnLoad/Overlay Settings"));
        DontDestroyOnLoad(eventSystem);
        DontDestroyOnLoad(overlayCredits);
        DontDestroyOnLoad(overlayPause);
        DontDestroyOnLoad(overlaySettings);
        overlayCredits.SetActive(false);
        overlayPause.SetActive(false);
        overlaySettings.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();

        if (isImposter)
            return;


        Application.quitting += () => applicationIsQuitting = true;
    }

    public static void ToggleCreditsUI() => overlayCredits.SetActive(!overlayCredits.activeSelf);
    public static void ToggleCreditsUI(bool active) => overlayCredits.SetActive(active);
    public static void TogglePauseUI() => overlayPause.SetActive(!overlayPause.activeSelf);


    private static bool _optionsListViewWasInteractableBeforePause;
    private static GameObject _lastSelectedDialogueOptionBeforePause;

    public static void TogglePauseUI(bool active)
    {
        overlayPause.SetActive(active);
        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            InputManager.inputSystemUIInputModule.enabled = true;
            if (YarnAccess.CurrentOptionView)
                _lastSelectedDialogueOptionBeforePause = YarnAccess.CurrentOptionView.gameObject;
            _optionsListViewWasInteractableBeforePause =
                YarnAccess.optionsListView.GetComponent<CanvasGroup>().interactable;
            YarnAccess.optionsListView.GetComponent<CanvasGroup>().interactable = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            InputManager.inputSystemUIInputModule.enabled = false;
            YarnAccess.optionsListView.GetComponent<CanvasGroup>().interactable =
                _optionsListViewWasInteractableBeforePause;
            if (_lastSelectedDialogueOptionBeforePause)
                EventSystem.current.SetSelectedGameObject(_lastSelectedDialogueOptionBeforePause);
        }
    }

    public static void UnpauseForMainMenu()
    {
        overlayPause.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        InputManager.inputSystemUIInputModule.enabled = true;
        Time.timeScale = 1;
    }

    public static void ToggleSettingsUI() => overlaySettings.SetActive(!overlaySettings.activeSelf);
    public static void ToggleSettingsUI(bool active) => overlaySettings.SetActive(active);


    public static bool paused;
    public static Action<bool> OnPauseChange;

    public static void TogglePause()
    {
        TogglePause(!paused);
    }

    public static void TogglePause(bool pause)
    {
        paused = pause;
        Time.timeScale = pause ? 0 : 1;
        OnPauseChange?.Invoke(pause);
    }


    public static void LoadScene(int index) => SceneManager.LoadScene(index);

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif !UNITY_WEBGL
         Application.Quit();
#endif
    }
}