using UnityEngine;
using UnityEngine.EventSystems;

public class YarnKeyboard : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (YarnAccess.dialogueRunner.IsDialogueRunning)
                YarnAccess.lineView.OnContinueClicked();

            // if (YarnAccess.dialogueRunner.IsDialogueRunning)
            //     YarnAccess.dialogueRunner.OnViewRequestedInterrupt();

            if (EventSystem.current.currentSelectedGameObject)
            {
                //optionview is EventSystem.current.currentSelectedGameObject.GetComponent<OptionView>();
                YarnAccess.OptionView.InvokeOptionSelected();
            }
        }
    }
}