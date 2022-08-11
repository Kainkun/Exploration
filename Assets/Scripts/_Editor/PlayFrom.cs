using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayFrom : EditorWindow
{
    private static bool _startedWithMenuItem;
    private const float CameraHeight = 1.5f;

    [InitializeOnLoadMethod]
    public static void Startup()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.EnteredPlayMode && _startedWithMenuItem)
            {
                MovePlayerToSceneView();
                _startedWithMenuItem = false;
            }
        };
    }

    [MenuItem("Play/Play from scene view")]
    public static void PlayFromSceneView()
    {
        if(EditorApplication.isPlaying)
            return;
        
        _startedWithMenuItem = true;
        EditorApplication.EnterPlaymode();
    }

    public static void MovePlayerToSceneView()
    {
        var cameraTransform = SceneView.lastActiveSceneView.camera.transform;
        Vector3 position = cameraTransform.position;
        position.y -= CameraHeight;
        float yRotation = cameraTransform.eulerAngles.y;

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