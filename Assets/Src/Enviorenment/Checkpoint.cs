using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IActivable // Checkpoint is IActivable because it could activate other go
{
    [SerializeField] private Vector2 _spawnOffset = new Vector2(0, 3);
    private bool _isActivated;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isActivated)
        {
            Activate();
        }
    }

    private void SetCheckpoint()
    {
        Debug.Log("SetCheckpoint");
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + _spawnOffset.y);
        CheckpointManager.Instance.SetCheckpoint(pos);
        
        _isActivated = true;
    }

    public void Activate()
    {
        SetCheckpoint();
    }

    public void DeActivate()
    {
        throw new NotImplementedException();
    }

    public void SwapState()
    {
        throw new NotImplementedException();
    }
}
