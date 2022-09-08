using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public class YarnConverter : EditorWindow
{
    private static readonly (string[], string)[] pairs = new (string[], string)[]
    {
        (new string[] { @"(\\\[(.*?\S+.*?)\\\])" }, "9e9e9e"), //Computer Variable
        (new string[] { "error" }, "ff0000"), //Computer Error
        (new string[] { "Vulcan", "Vulcan Terminal" }, "ff0000"), //Vulcan
        (new string[] { "Multi-Tool", "Standard Lab Access Keycard", "Job-Token", "Trash", "trash" }, "fcba03") //Items
    };

    [MenuItem("Yarn Tools/Convert")]
    private static void ConvertFiles()
    {
        string[] files =
            Directory.GetFiles(Application.dataPath + "/Yarn/Original", "*.yarn", SearchOption.AllDirectories);
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
                    Regex regex = new Regex(@"(?<!(^\s*(title:|position:|<<|---|===).*)|\$\S*)" + s + @"(?!\<\/color\>)");
                    lines[i] = regex.Replace(lines[i], @"<color=\#" + pair.Item2 + @">$&</color>");
                }
            }

            var match = Regex.Match(lines[i], @"(?<=\\\\GLITCH)[0-9]+(?=\\\\)");
            if (match.Success)
            {
                int n = Int32.Parse(match.Value);
                string glitch = GetGlitchText(n);
                Regex regex2 = new Regex(@"\\\\GLITCH[0-9]+\\\\");
                lines[i] = regex2.Replace(lines[i], glitch);
            }
        }

        string processedFilePath = originalFilePath.Replace(
            Application.dataPath + "/Yarn/Original",
            Application.dataPath + "/Yarn/Processed");

        string dir = Path.GetDirectoryName(processedFilePath);
        Directory.CreateDirectory(dir);

        string ext = Path.GetExtension(processedFilePath);
        string name = Path.GetFileNameWithoutExtension(processedFilePath);
        processedFilePath = Path.Combine(dir, name + "_processed" + ext);

        File.WriteAllLines(processedFilePath, lines);
    }

    private static string GetGlitchText(int n)
    {
        string s = "";
        for (int i = 0; i < n; i++)
        {
            s += @"<color=\#" + GetRandomColor() + ">" + GetRandomChar() + "</color>";
        }

        return s;
    }

    private static string GetRandomColor()
    {
        Color color = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);
        return ColorUtility.ToHtmlStringRGB(color);
    }

    private static char GetRandomChar()
    {
        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@%^&*+-=?~";
        return chars[Random.Range(0, chars.Length)];
    }
}