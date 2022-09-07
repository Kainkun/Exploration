using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.InputSystem.Interactions;
using System.Text.RegularExpressions;

public class YarnConverter : EditorWindow
{
    const string noColor = @"(?!\<\/color\>)";
    const string word = "your";
    const string replace = @"<color=\#ffffff>$&</color>";


    [MenuItem("Yarn Tools/Convert")]
    private static void ConvertFiles()
    {
        string path = Application.dataPath;
        path += "/Yarn";
        string[] files = Directory.GetFiles(path, "*.yarn", SearchOption.AllDirectories);
        foreach (string file in files)
            ConvertFile(file);
    }

    public static void ConvertFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

         // for (int i = 0; i < lines.Length; i++)
         // {
         //     Regex regex = new Regex(word + noColor);
         //     lines[i] = regex.Replace(lines[i], replace);
         // }

        File.WriteAllLines(filePath, lines);
    }
}