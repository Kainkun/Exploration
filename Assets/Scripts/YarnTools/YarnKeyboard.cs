using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

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

            //optionview is EventSystem.current.currentSelectedGameObject.GetComponent<OptionView>();
            var a = EventSystem.current.currentSelectedGameObject;
            if (a)
            {
                var b = a.GetComponent<OptionView>();
                if (b)
                    YarnAccess.OptionView.InvokeOptionSelected();
            }
        }
    }
}