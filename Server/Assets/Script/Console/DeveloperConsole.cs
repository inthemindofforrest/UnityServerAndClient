using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Console
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }

        public void AddCommandToConsole()
        {
            string addMessage = " command has been added to the console.";
            DeveloperConsole.AddCommandToConsole(Command, this);
            DeveloperConsole.sLog(Name + addMessage);
        }

        public abstract void RunCommand();
    }

    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance { get; private set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }

        [Header("UI Components")]
        public Canvas ConsoleCanvas;
        public ScrollRect ScrollRect;
        public Text ConsoleText;
        public Text InputText;
        public InputField InputField;

        private void Awake()
        {
            if(Instance != null)
            {
                return;
            }
            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
        }

        private void Start()
        {
            //ConsoleCanvas.gameObject.SetActive(false);
            CreateCommands();
        }

        private void CreateCommands()
        {
            CMD_Quit.CreateCommand();
        }

        public static void AddCommandToConsole(string _Name, ConsoleCommand _Command)
        {
            if (!Commands.ContainsKey(_Name))
            {
                Commands.Add(_Name, _Command);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.BackQuote))
            {
                ConsoleCanvas.gameObject.SetActive(!ConsoleCanvas.gameObject.activeInHierarchy);
            }

            if(ConsoleCanvas.gameObject.activeInHierarchy)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    if(InputText.text != "")
                    {
                        Log(InputText.text);
                        ParseInput(InputText.text);
                        InputText.text = "";
                    }
                }
            }
        }

        public void Log(string _MSG)
        {
            ConsoleText.text += _MSG + "\n";
            ScrollRect.verticalNormalizedPosition = 0f;
        }
        public static void sLog(string _MSG)
        {
            DeveloperConsole.Instance.Log(_MSG);
        }

        private void ParseInput(string _Input)
        {
            string[] input = _Input.Split(null);

            if(input.Length == 0 || input == null)
            {
                Log("Command not recognized...");
                return;
            }

            if(!Commands.ContainsKey(input[0]))
            {
                Log("Command not Found...");
            }
            else
            {
                Commands[input[0]].RunCommand();
            }
        }
    }
}