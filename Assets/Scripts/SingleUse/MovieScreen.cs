using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class MovieScreen : MonoBehaviour
{
    public BoxCollider playerTrigger;
    private bool movieStarted;
    public UnityEvent explosion;
    public UnityEvent movieEnd;

    private Utility.CheckBoxData triggerCheckBoxData;

    private void Start()
    {
        triggerCheckBoxData = Utility.GetCheckBoxData(playerTrigger);
    }

    void Update()
    {
        if (movieStarted)
            return;

        if (Physics.CheckBox(
                triggerCheckBoxData.triggerGlobalPosition,
                triggerCheckBoxData.triggerHalfExtent,
                triggerCheckBoxData.triggerRotation,
                LayerMask.GetMask("Player"),
                QueryTriggerInteraction.Ignore))
        {
            movieStarted = true;
            StartCoroutine(PlayMovie());
        }
    }

    IEnumerator PlayMovie()
    {
        float movieTime = 10;
        float explosionTime = 5;

        yield return new WaitForSeconds(explosionTime);

        print("BOOM!");
        explosion.Invoke();

        yield return new WaitForSeconds(movieTime - explosionTime);
        movieEnd.Invoke();
    }
}