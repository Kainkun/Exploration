using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFromGameObject : MonoBehaviour
{
    [EasyButtons.Button]
    public void PlayFromHere()
    {
        PlayFrom.PlayFromTransform(transform);
    }
}
