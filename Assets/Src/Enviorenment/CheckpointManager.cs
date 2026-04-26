using UnityEngine;

public class CheckpointManager : Utilities.Singleton<CheckpointManager>
{
    public struct CheckpointData
    {
        static public CheckpointData Invalid = new CheckpointData(0, Vector2.zero, 0f);
        public int checkpointIndex;
        public Vector2 position;
        public float mass;

        public CheckpointData(int checkpointIndex, Vector2 position, float mass)
        {
            this.checkpointIndex = checkpointIndex;
            this.position = position;
            this.mass = mass;
        }

        public bool IsValid()
        {
            return Invalid != this;
        }

        public static bool operator==(CheckpointData a, CheckpointData b)
        {
            return a.checkpointIndex == b.checkpointIndex && a.position == b.position && a.mass == b.mass;
        }

        public static bool operator!=(CheckpointData a, CheckpointData b)
        {
            return !(a == b);
        }
    }

    private CheckpointData _currentCheckpointData = CheckpointData.Invalid;
    

    public void SetCheckpoint(CheckpointData data)
    {
        _currentCheckpointData = data;
    }

    public CheckpointData GetCurrentCheckpoint()
    {
        return _currentCheckpointData;
    }
}
