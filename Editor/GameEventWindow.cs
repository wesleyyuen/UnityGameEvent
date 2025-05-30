using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class GameEventEditorWindow : EditorWindow
{
    private string _type;
    private string _typeCapitalized;
    private string _path = "Assets/Scripts/CustomEventTypes/";
    
    [MenuItem("Window/Game Events/Create New Game Event Scripts")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GameEventEditorWindow));
    }
    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Enter a valid C# type with no spaces");
        string input = EditorGUILayout.TextField("Type", _type);
        if (input != null)
        {
            _type = input.Trim();
            _typeCapitalized = input[0].ToString().ToUpper() + input.Substring(1);
        }
        
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Enter path to event scripts or use default");
        EditorGUILayout.BeginHorizontal();
        string path = EditorGUILayout.TextField("Path", _path);
        if (path != null)
        {
            _path = path.Trim();
            string lastChar = _path.Substring(_path.Length - 1, 1);
            if (!lastChar.Equals("/")) _path += "/";
        }
        if (GUILayout.Button("Use Currently Opened Folder"))
        {
            UpdatePathToCurrentOpenedFolder();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        if (GUILayout.Button("Generate Scripts"))
        {
            CreateDirectoryIfNotFound(_path);
            CreateEvent();
            CreateCaller();
            CreateListener();
            AssetDatabase.Refresh();
        }
    }

    private void UpdatePathToCurrentOpenedFolder()
    {
        // Use reflection to force get currently opened folder in project window
        Type projectWindowUtilType = typeof(ProjectWindowUtil);
        MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        object obj = getActiveFolderPath.Invoke(null, new object[0]);
        _path = obj.ToString();
    }

    private void CreateEvent()
    {
        string path = $"{_path}{_typeCapitalized}GameEvent.cs";
        Debug.Log($"Creating Event: {path}");
        using (StreamWriter outfile = new StreamWriter(path))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("");
            outfile.WriteLine("namespace GameEvents");
            outfile.WriteLine("{");
            outfile.WriteLine($"    [CreateAssetMenu(menuName = \"Game Events/Custom/{_typeCapitalized} Game Event\")]");
            outfile.WriteLine($"    public class {_typeCapitalized}GameEvent : GenericGameEvent<{_type}> {{}}");
            outfile.WriteLine("}");
        }
    }

    private void CreateCaller()
    {
        string path = $"{_path}{_typeCapitalized}EventCaller.cs";
        Debug.Log($"Creating Caller: {path}");
        using (StreamWriter outfile = new StreamWriter(path))
        {
            outfile.WriteLine("namespace GameEvents");
            outfile.WriteLine("{");
            outfile.WriteLine($"    public class {_typeCapitalized}EventCaller : GenericGameEventCaller<{_type}> {{}}");
            outfile.WriteLine("}");
        }
    }

    private void CreateListener()
    {
        string path = $"{_path}{_typeCapitalized}EventListener.cs";
        Debug.Log($"Creating Listener: {path}");
        using (StreamWriter outfile = new StreamWriter(path))
        {
            outfile.WriteLine("namespace GameEvents");
            outfile.WriteLine("{");
            outfile.WriteLine($"    public class {_typeCapitalized}EventListener : GenericGameEventListener<{_type}> {{}}");
            outfile.WriteLine("}");
        }
    }

    private void CreateDirectoryIfNotFound(string path)
    {
        bool directoryExists = Directory.Exists(path);
        if (!directoryExists) Directory.CreateDirectory(path);
    }
}
