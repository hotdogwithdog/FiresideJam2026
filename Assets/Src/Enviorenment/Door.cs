using UnityEngine;

public class Door : MonoBehaviour, IActivable
{
    public void Activate()
    {
        Debug.Log("Door activated");
    }

    public void DeActivate()
    {
        Debug.Log("Door deactivated");
    }

    public void SwapState()
    {
        throw new System.NotImplementedException();
    }
}
