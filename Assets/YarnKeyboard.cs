using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.EventSystems;

public class YarnKeyboard : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LineView lineView = GetComponentInChildren<LineView>();
            if (lineView)
                lineView.UserRequestedViewAdvancement();
            
            if(EventSystem.current.currentSelectedGameObject)
            {
                OptionView optionView = EventSystem.current.currentSelectedGameObject.GetComponent<OptionView>();
                optionView.InvokeOptionSelected();
            }
        }
    }
}
