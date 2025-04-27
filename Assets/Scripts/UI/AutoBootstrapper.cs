using UnityEngine;

public static class AutoBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Debug.Log("AutoBootstrapper running...");

        // Create a GameObject called "Bootstrap"
        GameObject bootstrap = new GameObject("Bootstrap");

        // Add the PerlerColorChanger script to it
        bootstrap.AddComponent<PerlerColorChanger>();
    }
}
