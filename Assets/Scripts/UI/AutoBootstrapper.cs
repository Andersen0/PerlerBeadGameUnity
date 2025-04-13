using UnityEngine;

public class AutoBootstrapper : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("AutoBootstrapper running...");

        // Create a GameObject called "Bootstrap"
        GameObject bootstrap = new GameObject("Bootstrap");

        // Add the PerlerColorChanger script to it
        bootstrap.AddComponent<PerlerColorChanger>();
    }
}
