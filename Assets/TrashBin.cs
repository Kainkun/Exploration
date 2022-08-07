using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private float requiredTrashForJobToken = 10;

    public void Deposit(float trashCount)
    {
        if (trashCount <= 0)
            return;

        YarnAccess.TryGetValue("depositedTrashCount", out float currentDepositedTrashCount);
        float newDepositedTrashCount = currentDepositedTrashCount + trashCount;

        int currentTokensFromTrash = Mathf.FloorToInt(currentDepositedTrashCount / requiredTrashForJobToken);
        int newTokensFromTrash =
            Mathf.FloorToInt(newDepositedTrashCount / requiredTrashForJobToken) - currentTokensFromTrash;
        YarnAccess.AddValue("jobTokenCount", newTokensFromTrash);
        YarnAccess.SetValue("depositedTrashCount", newDepositedTrashCount);

        print("Deposited " + trashCount + " trash. " + newDepositedTrashCount + " total deposited. " +
              newTokensFromTrash + " jobTokens added.");
    }
}