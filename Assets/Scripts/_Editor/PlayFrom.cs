using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayFrom : EditorWindow
{
    private static Transform playFromTransform;

    [InitializeOnLoadMethod]
    private static void Startup()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            Debug.Log(playFromTransform);
            if (state == PlayModeStateChange.EnteredPlayMode && playFromTransform)
            {
                MovePlayerToTransform(playFromTransform);
                playFromTransform = null;
            }
        };
    }


    [MenuItem("Play/Play from scene view")]
    public static void PlayFromSceneView()
    {
        if (EditorApplication.isPlaying)
        {
            MovePlayerToSceneView();
            return;
        }

        playFromTransform = SceneView.lastActiveSceneView.camera.transform;
        EditorApplication.EnterPlaymode();
    }

    public static void PlayFromTransform(Transform transform)
    {
        if (EditorApplication.isPlaying)
        {
            MovePlayerToTransform(transform);
            return;
        }

        playFromTransform = transform;
        EditorApplication.EnterPlaymode();
    }


    public static void MovePlayerToSceneView()
    {
        MovePlayerToTransform(SceneView.lastActiveSceneView.camera.transform);
    }

    public static void MovePlayerToTransform(Transform transform)
    {
        Vector3 position = transform.position;
        float yRotation = transform.eulerAngles.y;

        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        Transform playerTransform;
        if (!playerManager || (playerManager && !playerManager.gameObject.activeSelf))
            playerTransform = Instantiate(Resources.Load<GameObject>("Player")).transform;
        else
            playerTransform = playerManager.transform;

        playerTransform.position = position;
        playerTransform.eulerAngles = new Vector3(0, yRotation, 0);
    }
}