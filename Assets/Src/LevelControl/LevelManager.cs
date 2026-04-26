using Environment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelControl
{
    public class LevelManager : Utilities.Singleton<LevelManager>
    {
        public int CurrentLevel { get; set; }

        public string GetCurrentLevelName()
        {
            return "Level" + CurrentLevel.ToString();
        }

        public string GetLevelName(int level)
        {
            return "Level" + level.ToString();
        }

        public void RestartLevelEnvironment()
        {
            foreach (GameObject objectToRestart in GameObject.FindGameObjectsWithTag("Reseteable"))
            {
                IReseteable reseteable = objectToRestart.GetComponent<IReseteable>();
                if (reseteable == null) continue;
                
                reseteable.ResetState();
            }
        }
        
        public void DestroyMassBalls()
        {
            foreach (GameObject massBall in GameObject.FindGameObjectsWithTag("MassBall"))
            {
                Destroy(massBall);
            }
        }
    }
}