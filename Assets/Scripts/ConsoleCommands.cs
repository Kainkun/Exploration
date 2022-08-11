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
                PlayerManager.UnlockMultiTool();
                PlayerManager.UnlockTrashCollector();
                PlayerManager.UnlockEssenceCollector();
                PlayerManager.UnlockJetpackGlide();
                PlayerManager.UnlockJetpackBoost();
                return;
            }

            switch (args[0])
            {
                case "mt":
                    PlayerManager.UnlockMultiTool();
                    break;
                case "tc":
                    PlayerManager.UnlockTrashCollector();
                    break;
                case "ec":
                    PlayerManager.UnlockEssenceCollector();
                    break;
                case "gl":
                    PlayerManager.UnlockJetpackGlide();
                    break;
                case "bo":
                    PlayerManager.UnlockJetpackBoost();
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
                PlayerManager.PickUpTrash(1000);
                PlayerManager.PickUpEssence(1000);
                PlayerManager.GetJobToken(1000);
                return;
            }

            switch (args[0])
            {
                case "tr":
                    PlayerManager.PickUpTrash(int.Parse(args[1]));
                    break;
                case "es":
                    PlayerManager.PickUpEssence(int.Parse(args[1]));
                    break;
                case "jt":
                    PlayerManager.GetJobToken(int.Parse(args[1]));
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

            var transform = PlayerManager.playerMovement.transform;
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
                Transform[] children = GameObject.Find("DebugPositions").GetComponentsInChildren<Transform>();
                foreach (Transform child in children)
                    yield return Console.Alias.Get("tp" + child.name, "tp " + child.name);
            }
        }
    }

    [AutoRegisterConsoleCommand]
    public class Noclip : IConsoleCommand
    {
        public void Execute(string[] args)
        {
            PlayerManager.ToggleNoclip();
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