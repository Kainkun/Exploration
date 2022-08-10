using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjectUtility", menuName = "TEMPLATE-K/ScriptableObjectUtility", order = 1)]
public class ScriptableObjectUtility : ScriptableObject
{
    public void ToggleCreditsUI() => GameManager.ToggleCreditsUI();
    public void ToggleCreditsUI(bool active) => GameManager.ToggleCreditsUI(active);
    
    public void TogglePauseUI() => GameManager.TogglePauseUI();
    public void TogglePauseUI(bool active) => GameManager.TogglePauseUI(active);
    
    public void ToggleSettingsUI() => GameManager.ToggleSettingsUI();
    public void ToggleSettingsUI(bool active) => GameManager.ToggleSettingsUI(active);

    public void Unpause() => InputManager.Get().OnUnpause();
    public void UnpauseForMainMenu() => GameManager.UnpauseForMainMenu();
    
    
    public void LoadScene(int index) => GameManager.LoadScene(index);

    public void PlaySound(AudioClip audioClip) => AudioManager.Get().PlaySound(audioClip);
    public void PlaySound(AudioClip audioClip, float volume) => AudioManager.Get().PlaySound(audioClip, volume);

    public void QuitGame() => GameManager.QuitGame();
    
    public void StartDialogue(string s) => YarnAccess.dialogueRunner.StartDialogue(s);
}