using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class SaveLoadTransforms : EditorWindow
{
    private static Transform[] transforms = new Transform[] { };
    private static Vector3[] positions;
    private static Quaternion[] rotations;
    private static Vector3[] scales;

    [MenuItem("Tools/Save Selected Transforms")]
    public static void StopAndSave()
    {
        transforms = Selection.transforms;

        positions = new Vector3[transforms.Length];
        rotations = new Quaternion[transforms.Length];
        scales = new Vector3[transforms.Length];

        for (int i = 0; i < transforms.Length; i++)
        {
            positions[i] = transforms[i].position;
            rotations[i] = transforms[i].rotation;
            scales[i] = transforms[i].localScale;
        }
    }

    [MenuItem("Tools/Save Selected Transforms", true)]
    static bool ValidateStopAndSave()
    {
        return Selection.transforms.Length > 0;
    }

    [MenuItem("Tools/Select Saved Transforms")]
    public static void SelectSaved()
    {
        Object[] objects = new Object[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            objects[i] = transforms[i].gameObject;
        }

        Selection.objects = objects;
    }

    [MenuItem("Tools/Select Saved Transforms", true)]
    static bool ValidateSelectSaved()
    {
        return transforms.Length > 0;
    }

    [MenuItem("Tools/Load Selected Transforms")]
    public static void Load()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            if (!Selection.transforms.Contains(transforms[i]))
                continue;

            transforms[i].position = positions[i];
            transforms[i].rotation = rotations[i];
            transforms[i].localScale = scales[i];
        }
    }

    [MenuItem("Tools/Load Selected Transforms", true)]
    static bool ValidateLoad()
    {
        bool selectionValid = false;
        foreach (var t in transforms)
        {
            if (!Selection.transforms.Contains(t)) continue;
            selectionValid = true;
            break;
        }

        return transforms.Length > 0 && selectionValid;
    }
}