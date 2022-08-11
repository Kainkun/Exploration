/*
namespace ConsoleUtility
{
    [AutoRegisterConsoleCommand]
    public class MyConsoleCommand : IConsoleCommand
    {
        public void Execute(string[] args)
        {
            //use args array to parse command. args[] do not include the base command so in
            // command 'mycommand foo bar' args[0] is foo and args[1] is bar
        }

        public string name => "mycommand"; 		// the actual command key
        public string summary => "Does soemthing";	// summary displayed when typing 'help'
        public string help => "usage: mycommand"; 	// help displayed when typing 'help mycommand'

        public IEnumerable<Console.Alias> aliases
        {
            get
            {
                yield return Console.Alias.Get("myalias", "mycommand foo bar");
                // yield return any console alias you need, for ease of use purposes
            }
        }
    }
    

    [AutoRegisterConsoleCommand]
    public class MyConsoleCommand : IConsoleCommand
    {
        public void Execute(string[] args)
        {
        
        }

        public string name => "mycommand";
        public string summary => "Does soemthing";
        public string help => "usage: mycommand";

        public IEnumerable<Console.Alias> aliases { get; }
    }

}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConsoleUtility
{
    [AutoRegisterConsoleCommand]
    public class Unlock : IConsoleCommand
    {
        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                PlayerMultiTool.Singleton.ActivateMultiTool();
                PlayerMultiTool.Singleton.UnlockModule(PlayerMultiTool.ModuleType.Trash);
                PlayerMultiTool.Singleton.UnlockModule(PlayerMultiTool.ModuleType.Essence);
                PlayerMovement.Singleton.UnlockGlide();
                PlayerMovement.Singleton.UnlockBoost();
                return;
            }

            switch (args[0])
            {
                case "mt":
                    PlayerMultiTool.Singleton.ActivateMultiTool();
                    break;
                case "tc":
                    PlayerMultiTool.Singleton.UnlockModule(PlayerMultiTool.ModuleType.Trash);
                    break;
                case "ec":
                    PlayerMultiTool.Singleton.UnlockModule(PlayerMultiTool.ModuleType.Essence);
                    break;
                case "gl":
                    PlayerMovement.Singleton.UnlockGlide();
                    break;
                case "bo":
                    PlayerMovement.Singleton.UnlockBoost();
                    break;
            }
        }

        public string name => "ul";
        public string summary => "Unlock";
        public string help => "usage: ul, ul [mt, tc, ec, gl, bo]";

        public IEnumerable<Console.Alias> aliases { get; }
    }

    [AutoRegisterConsoleCommand]
    public class Collect : IConsoleCommand
    {
        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                YarnAccess.AddValue("trashCount", 1000);
                YarnAccess.AddValue("essenceCount", 1000);
                YarnAccess.AddValue("jobTokenCount", 1000);
                return;
            }

            switch (args[0])
            {
                case "tr":
                    YarnAccess.AddValue("trashCount", float.Parse(args[1]));
                    break;
                case "es":
                    YarnAccess.AddValue("essenceCount", float.Parse(args[1]));
                    break;
                case "jt":
                    YarnAccess.AddValue("jobTokenCount", float.Parse(args[1]));
                    break;
                default:
                    if (args.Length == 1)
                        YarnAccess.SetValue(args[0], true);
                    else
                        YarnAccess.AddValue(args[0], float.Parse(args[1]));
                    break;
            }
        }

        public string name => "cl";
        public string summary => "Collect";
        public string help => "usage: cl, cl [tr, es, jt] amount, cl name";

        public IEnumerable<Console.Alias> aliases { get; }
    }

    [AutoRegisterConsoleCommand]
    public class Teleport : IConsoleCommand
    {
        public void Execute(string[] args)
        {
            Transform t = GameObject.Find(args[0]).transform;

            var transform = PlayerMovement.Singleton.transform;
            transform.position = t.position;
            transform.eulerAngles = new Vector3(0, t.eulerAngles.y, 0);
        }

        public string name => "tp";
        public string summary => "Teleport";
        public string help => "usage: tp [FirstFloor, SecondFloor, etc...]";

        public IEnumerable<Console.Alias> aliases
        {
            get
            {
                GameObject g = GameObject.Find("DebugPositions");
                if(g)
                {
                    Transform[] children = g.GetComponentsInChildren<Transform>();
                    foreach (Transform child in children)
                        yield return Console.Alias.Get("tp" + child.name, "tp " + child.name);
                }
            }
        }
    }

    [AutoRegisterConsoleCommand]
    public class Noclip : IConsoleCommand
    {
        public void Execute(string[] args)
        {
            PlayerMovement.Singleton.ToggleNoclip();
        }

        public string name => "noclip";
        public string summary => "nc, noclip";
        public string help => "usage: nc, noclip";

        public IEnumerable<Console.Alias> aliases
        {
            get { yield return Console.Alias.Get("nc", "noclip"); }
        }
    }
}