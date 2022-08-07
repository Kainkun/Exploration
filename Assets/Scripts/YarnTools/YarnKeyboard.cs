using UnityEngine;
using UnityEngine.EventSystems;

public class YarnKeyboard : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (YarnAccess.lineView)
                YarnAccess.lineView.UserRequestedViewAdvancement();

            if (EventSystem.current.currentSelectedGameObject)
            {
                YarnAccess.OptionView.InvokeOptionSelected();
            }
        }
    }
}