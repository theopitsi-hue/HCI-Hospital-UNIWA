using UnityEditor;

[InitializeOnLoad]
public static class BBAutoGenerateOnPlay
{
    static BBAutoGenerateOnPlay()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                BBGenerator.Generate();
            }
        };
    }
}