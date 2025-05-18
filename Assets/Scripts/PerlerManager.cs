using UnityEngine;

public class PerlerManager : MonoBehaviour
{   
    public void ClearAllPerlers()
    {
        GameObject[] beads = GameObject.FindGameObjectsWithTag("PerlerTag");

        foreach (GameObject bead in beads)
        {
            Destroy(bead);
        }
    }
}
