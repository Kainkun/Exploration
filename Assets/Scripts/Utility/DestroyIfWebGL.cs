using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfWebGL : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_WEBGL
        Destroy(gameObject);
#else
        Destroy(this);
#endif
    }
}