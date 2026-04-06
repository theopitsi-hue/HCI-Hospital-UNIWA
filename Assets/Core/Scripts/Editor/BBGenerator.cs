using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

public static class BBGenerator
{
    private const string OUTPUT_PATH = "Assets/Generated/Scripts/BB.cs";

    [UnityEditor.MenuItem("Tools/Blackboard/Generate BB Class")]
    public static void Generate()
    {
        // Load all keys from Resources instead of AssetDatabase
        var keys = Resources.LoadAll<BlackboardKey>("Core/ScriptableObjects/Keys");

        var names = keys
            .Where(k => k != null)
            .Select(k => k.name)
            .Distinct()
            .OrderBy(n => n)
            .ToList();

        var sb = new StringBuilder();

        sb.AppendLine("public static class BB");
        sb.AppendLine("{");

        foreach (var name in names)
        {
            string safeName = Sanitize(name);
            sb.AppendLine($"    public static readonly string {safeName} = \"{safeName}\";");
        }

        sb.AppendLine("}");

        EnsureFolderExists();

        // Skip rewrite if unchanged
        if (File.Exists(OUTPUT_PATH))
        {
            string existing = File.ReadAllText(OUTPUT_PATH);
            if (existing == sb.ToString())
            {
                UnityEngine.Debug.Log("BB.cs already up to date.");
                return;
            }
        }

        File.WriteAllText(OUTPUT_PATH, sb.ToString());
        UnityEditor.AssetDatabase.Refresh();

        UnityEngine.Debug.Log("BB.cs generated.");
    }

    private static string Sanitize(string input)
    {
        var valid = new string(input
            .Where(c => char.IsLetterOrDigit(c) || c == '_')
            .ToArray());

        if (!string.IsNullOrEmpty(valid) && char.IsDigit(valid[0]))
            valid = "_" + valid;

        return valid;
    }

    private static void EnsureFolderExists()
    {
        string fullPath = "Assets/Generated/Scripts";

        if (Directory.Exists(fullPath))
            return;

        Directory.CreateDirectory(fullPath);
    }
}