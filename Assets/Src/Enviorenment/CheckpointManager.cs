using UnityEngine;

public class CheckpointManager : Utilities.Singleton<CheckpointManager>
{
    private Vector2 _currentCheckpoint;

    public void SetCheckpoint(Vector2 position)
    {
        _currentCheckpoint = position;
    }

    public Vector2 GetCurrentCheckpoint()
    {
        return _currentCheckpoint;
    }
}
