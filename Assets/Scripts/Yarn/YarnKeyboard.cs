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
            if (YarnSingleton.Get().lineView)
                YarnSingleton.Get().lineView.UserRequestedViewAdvancement();
            
            if(EventSystem.current.currentSelectedGameObject)
            {
                YarnSingleton.Get().OptionView.InvokeOptionSelected();
            }
        }
    }
}
