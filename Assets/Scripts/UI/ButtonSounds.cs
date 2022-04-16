using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerDownHandler
{
    public AudioClip onPointerEnter;
    public AudioClip onPointerSelect;
    public AudioClip onPointerDown;
    public AudioClip onPointerUp;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(onPointerEnter)
            AudioManager.Get().PlaySound(onPointerEnter);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(onPointerSelect)
            AudioManager.Get().PlaySound(onPointerSelect);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(onPointerDown)
            AudioManager.Get().PlaySound(onPointerDown);
    }

    private void PlayClickedSound()
    {
        if(onPointerUp)
            AudioManager.Get().PlaySound(onPointerUp);
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickedSound);
    }
}
