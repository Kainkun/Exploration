using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class MovieScreen : MonoBehaviour
{
    private bool movieStarted;
    public UnityEvent explosion;
    public UnityEvent movieEnd;

    public void StartMovieCoroutine()
    {
        if(movieStarted)
            return;

        movieStarted = true;
        StartCoroutine(PlayMovie());
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