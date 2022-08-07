using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsingStairs : MonoBehaviour
{
    public BoxCollider playerTrigger;

    private void Start()
    {
        playerTrigger.transform.parent = null;
    }

    private void Update()
    {
        if (playerTrigger && Physics.CheckBox(
                playerTrigger.transform.position + playerTrigger.center,
                playerTrigger.size / 2,
                playerTrigger.transform.rotation,
                LayerMask.GetMask("Player"),
                QueryTriggerInteraction.Ignore))
        {
            if (transform.localScale.y > 0.1f)
                transform.localScale -= new Vector3(0, (1 / 0.25f) * Time.deltaTime, 0);
            else
            {
                transform.localScale = new Vector3(1, 0.1f, 1);
                Destroy(playerTrigger.gameObject);
                Destroy(this);
            }
        }
    }
}