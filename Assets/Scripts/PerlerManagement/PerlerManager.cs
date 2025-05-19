using UnityEngine;

public class PerlerManager : MonoBehaviour
{
    public Transform beadParent; // Assigned in the Inspector as Perler Bead Container

    public void ClearAllPerlers()
    {
        // Destroy all child GameObjects of beadParent
        for (int i = beadParent.childCount - 1; i >= 0; i--)
        {
            Destroy(beadParent.GetChild(i).gameObject);
        }
    }
}