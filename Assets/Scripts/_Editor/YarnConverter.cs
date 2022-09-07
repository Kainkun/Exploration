using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.InputSystem.Interactions;
using System.Text.RegularExpressions;

public class YarnConverter : EditorWindow
{
    private readonly (string[], string)[] pairs = new (string[], string)[]
    {
        (new string[] { "grey" }, "9e9e9e"), //Computer Variable
        (new string[] { "error" }, "ff0000"), //Computer Error
        (new string[] { "vulcan", "Vulcan", "Vulcan Terminal" }, "ff0000"), //Vulcan
        (new string[] { "multiTool", "Standard Lab Access Keycard" }, "fcba03") //Items
    };

    [MenuItem("Yarn Tools/Convert")]
    private static void ConvertFiles()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Yarn/Original", "*.yarn", SearchOption.AllDirectories);
        foreach (string file in files)
            ConvertFile(file);
    }

    public static void ConvertFile(string originalFilePath)
    {
        string[] lines = File.ReadAllLines(originalFilePath);

        for (int i = 0; i < lines.Length; i++)
        {
            foreach (var pair in pairs)
            {
                foreach (string s in pair.Item1)
                {
                    Regex regex = new Regex(s + @"(?!\<\/color\>)");
                    lines[i] = regex.Replace(lines[i], @"<color=\#" + pair.Item2 + @">$&</color>");
                }
            }
        }

        string processedFilePath = originalFilePath.Replace(Application.dataPath + "/Yarn/Original", Application.dataPath + "/Yarn/Processed");
        File.WriteAllLines(originalFilePath, lines);
    }
}