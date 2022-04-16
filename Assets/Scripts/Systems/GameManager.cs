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
    
    private static readonly GameData GameData = new GameData();

    private void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        
        Application.quitting += () => applicationIsQuitting = true;
        
        GameData.SetData();

        overlayCredits = Instantiate(Resources.Load<GameObject>("Overlay Credits"));
        overlayPause = Instantiate(Resources.Load<GameObject>("Overlay Pause"));
        overlaySettings = Instantiate(Resources.Load<GameObject>("Overlay Settings"));
        eventSystem = Instantiate(Resources.Load<GameObject>("EventSystem"));
        
         DontDestroyOnLoad(overlayCredits);
         DontDestroyOnLoad(overlayPause);
         DontDestroyOnLoad(overlaySettings);
         DontDestroyOnLoad(eventSystem);
        
         overlayCredits.SetActive(false);
         overlayPause.SetActive(false);
         overlaySettings.SetActive(false);
    }

    public static void ToggleCreditsUI() => overlayCredits.SetActive(!overlayCredits.activeSelf);
    public static void ToggleCreditsUI(bool active) =>overlayCredits.SetActive(active);
    public static void TogglePauseUI() => overlayPause.SetActive(!overlayPause.activeSelf);
    public static void TogglePauseUI(bool active) =>overlayPause.SetActive(active);
    public static void ToggleSettingsUI() => overlaySettings.SetActive(!overlaySettings.activeSelf);
    public static void ToggleSettingsUI(bool active) =>overlaySettings.SetActive(active);


    
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
    
    
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif !UNITY_WEBGL
         Application.Quit();
#endif
    }
}