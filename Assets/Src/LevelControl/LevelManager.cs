using System;
using Unity.VisualScripting;

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
    }
}