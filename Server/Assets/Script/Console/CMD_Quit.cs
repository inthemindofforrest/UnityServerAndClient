using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CMD_Quit : ConsoleCommand
    {
        public override string Name { get; protected set ; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public CMD_Quit()
        {
            Name = "Quit";
            Command = "quit";
            Description = "Quits the application";
            Help = "Does Something";

            AddCommandToConsole();
        }

        public override void RunCommand()
        {
            Application.Quit();
        }

        public static CMD_Quit CreateCommand()
        {
            return new CMD_Quit();
        }
    }
}