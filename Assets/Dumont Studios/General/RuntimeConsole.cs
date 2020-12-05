using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable

namespace DumontStudios.General
{
    public enum LogType
    {
        Normal,
        Input,
        Warning,
        Error
    }

    public enum LogLevel
    {
        Level0,
        Level1,
        Level2,
        Level3
    }

    public struct LogEntry
    {
        public string text;
        public LogType type;
        public LogLevel level;
    }

    public class Command
    {
        public string name;
        public Action<string[]> method;
        public int numberOfArguments;
        public string[] argumentNames;
        public string description;

        public Command(string name, string description, Action<string[]> method, string[] argumentNames)
        {
            this.name = name;
            this.method = method;
            this.numberOfArguments = argumentNames.Length;
            this.argumentNames = argumentNames;
        }
        
        public void execute(string[] arguments)
        {
            method(arguments);
        }
    }

    [DefaultExecutionOrder(-10000)]
    public class RuntimeConsole : MonoBehaviour
    {
        public static bool isOpen;

        public static RuntimeConsole instance;

        private static List<LogEntry> entries = new List<LogEntry>();

        //Data
        //public float windowHeight = 400;
        public KeyCode key = KeyCode.F1;
        public int maxEntries = 150;
        public LogLevel logLevel = LogLevel.Level0;
        public bool logOnUnity;

        //Internal
        private Vector2 scrollPos;
        private static string input = "";
        private static string lastInput = "";

        private static int inputHistoryIndex = 0;
        private static List<string> inputHistory = new List<string>();

        private static Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private static List<Command> matchedCommands = new List<Command>();

        //Flags
        private static bool focusInput = false;
        private static bool newEntry = false;

        public void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);

                return;
            }

            instance = this;

            RegisterCommand("?", Question, "command");
            RegisterCommand("help", Help);
            RegisterCommand("log_level", SetLogLevel, "level");
            RegisterCommand("unload_resources", UnloadResources);
            RegisterCommand("path", Path);
        }

        public void OnDestroy()
        {
            inputHistory.Clear();
            commands.Clear();
            matchedCommands.Clear();
        }

        public void OnGUI()
        {
            if(Event.current.isKey && Event.current.keyCode == key && Event.current.type == EventType.KeyDown)
            {
                Event.current.Use();
                Toggle();
            }

            if(!isOpen)
            {
                return;
            }

            GUILayout.BeginVertical();
            scrollPos = GUILayout.BeginScrollView(scrollPos, "Box", GUILayout.Width(Screen.width), GUILayout.Height(Screen.height/2));

            int maxIterations = Mathf.Min(entries.Count - 1, maxEntries);

            if(newEntry)
            {
                scrollPos.y = 9999999;
                newEntry = false;
            }

            for(int i = 0; i <= maxIterations; i++)
            {
                LogEntry entry = entries[entries.Count - (maxIterations - i) - 1];

                if(entry.level > logLevel)
                {
                    continue;
                }

                Color originalColor = GUI.color;

                if(entry.type == LogType.Input)
                {
                    GUI.color = Color.green;
                }
                else if(entry.type == LogType.Warning)
                {
                    GUI.color = Color.yellow;
                }
                else if(entry.type == LogType.Error)
                {
                    GUI.color = Color.red;
                }

                GUILayout.Label(entry.text);

                GUI.color = originalColor;
            }

            GUILayout.EndScrollView();

            bool movedUpOrDown = false;

            if(inputHistory.Count > 0)
            {
                if(Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow)
                {
                    inputHistoryIndex = Mathf.Max(inputHistoryIndex - 1, 0);
                    input = inputHistory[inputHistoryIndex];
                    movedUpOrDown = true;
                }

                if(Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow)
                {
                    inputHistoryIndex = Mathf.Min(inputHistoryIndex + 1, inputHistory.Count - 1);
                    input = inputHistory[inputHistoryIndex];
                    movedUpOrDown = true;
                }
            }

            GUI.SetNextControlName("Command Input Field");
            input = GUILayout.TextField(input);
            TextEditor textField = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

            if(input != lastInput)
            {
                lastInput = input;

                matchedCommands = commands.Values.Where(h => h.name.Contains(input)).ToList();
            }

            if(!string.IsNullOrEmpty(input))
            {
                for(int i = 0; i < matchedCommands.Count; i++)
                {
                    GUILayout.BeginHorizontal("Box");
                    GUILayout.Label(matchedCommands[i].name, GUILayout.Width(200));

                    for(int j = 0; j < matchedCommands[i].numberOfArguments; j++)
                    {
                        GUILayout.Label(matchedCommands[i].argumentNames[j], GUILayout.Width(100));
                        GUILayout.Space(30);
                    }
                    GUILayout.EndHorizontal();
                }
            }

            if(movedUpOrDown)
            {
                textField.SelectNone();
                textField.MoveTextEnd();
            }

            if(focusInput)
            {
                focusInput = false;
                GUI.FocusControl("Command Input Field");
            }

            if(Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "Command Input Field")
            {
                HandleInput(input);
                input = "";
            }

            GUILayout.EndVertical();
        }

        public static void Log(FastString entry, LogType logType = LogType.Normal, LogLevel logLevel = LogLevel.Level0, bool alsoLogOnUnity = false)
        {
            //if(!Application.isPlaying || instance == null)
            //{
            //    logOnUnity(entry, logType);

            //    return;
            //}

            if(alsoLogOnUnity || !Application.isPlaying || !instance || instance.logOnUnity)
            {
                if(logType == LogType.Normal)
                {
                    Debug.Log(entry);
                }
                else if(logType == LogType.Warning)
                {
                    Debug.LogWarning(entry);
                }
                else if(logType == LogType.Error)
                {
                    Debug.LogError(entry);
                }
            }

            entries.Add(new LogEntry() { text = entry.ToString(), type = logType, level = logLevel });
            newEntry = true;
        }

        public static void LogWarning(FastString entry, LogLevel logLevel = LogLevel.Level0, bool alsoLogOnUnity = false)
        {
            Log(entry, LogType.Warning, logLevel, alsoLogOnUnity);
        }

        public static void LogError(FastString entry, LogLevel logLevel = LogLevel.Level0, bool alsoLogOnUnity = false)
        {
            Log(entry, LogType.Error, logLevel, alsoLogOnUnity);
        }

        //Utils
        public static void RegisterCommand(string command, Action<string[]> method, params string[] argumentNames)
        {
            if(!commands.ContainsKey(command))
            {
                commands.Add(command, new Command(command, "", method, argumentNames));
            }
        }

        public static void RegisterCommand(string command, string description, Action<string[]> method, params string[] argumentNames)
        {
            if(!commands.ContainsKey(command))
            {
                commands.Add(command, new Command(command, description, method, argumentNames));
            }
        }

        private static void HandleInput(string str)
        {
            if(!string.IsNullOrEmpty(str))
            {
                inputHistory.Add(str);
                inputHistoryIndex = inputHistory.Count;
            }

            Log(str, LogType.Input);

            Parse(str);
        }

        private void Toggle()
        {
            isOpen = !isOpen;
            focusInput = true;
        }

        private static void Parse(string str)
        {
            string[] words = str.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if(words.Length > 0)
            {
                if(commands.ContainsKey(words[0]))
                {
                    Command command = commands[words[0]];

                    if(words.Length > command.numberOfArguments)
                    {
                        string[] arguments = new string[words.Length - 1];
                        Array.Copy(words, 1, arguments, 0, arguments.Length);

                        command.execute(arguments);
                    }
                    else
                    {
                        Log("Incorrect number of arguments.", LogType.Error);
                    }
                }
                else
                {
                    Log("Invalid command.", LogType.Error);
                }
            }
        }

        //Basic Commands
        private static void Question(string[] arguments)
        {
            try
            {
                string commandName = arguments[0];

                if(!commands.ContainsKey(commandName))
                {
                    Log("Invalid command.");

                    return;
                }

                Command command = commands[commandName];
                string str = commandName + " ";

                foreach(string argumentName in command.argumentNames)
                {
                    str += argumentName + " ";
                }

                Log(str + (string.IsNullOrEmpty(command.description) ? "" : "> " + command.description));
            }
            catch(Exception e)
            {
                Log("Invalid argument.");
            }
        }

        private static void Help(string[] arguments)
        {
            foreach(KeyValuePair<string, Command> commandPair in commands)
            {
                string str = commandPair.Key + " ";

                foreach(string argumentName in commandPair.Value.argumentNames)
                {
                    str += argumentName + " ";
                }

                Log(str + (string.IsNullOrEmpty(commandPair.Value.description) ? "" : "> " + commandPair.Value.description));
            }
        }

        private static void UnloadResources(string[] arguments)
        {
            Resources.UnloadUnusedAssets();

            Log("Unloaded Unused Resources.");
        }

        private static void Path(string[] arguments)
        {
            Log(Application.dataPath);
        }

        private static void SetLogLevel(string[] arguments)
        {
            try
            {
                LogLevel logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), arguments[0]);
                instance.logLevel = logLevel;

                Log("Log Level is now set to: " + logLevel.ToString());
            }
            catch(Exception e)
            {
                Log("Invalid argument.");
            }
        }

        private static void SetLogOnUnity(string[] arguments)
        {
            try
            {
                instance.logOnUnity = bool.Parse(arguments[0]);
            }
            catch(Exception e)
            {
                Log("Invalid argument.");
            }
        }
    }
}

#pragma warning restore