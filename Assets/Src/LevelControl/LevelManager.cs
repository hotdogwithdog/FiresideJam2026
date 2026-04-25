using System;
using Unity.VisualScripting;

namespace LevelControl
{
    public class LevelManager : Utilities.Singleton<LevelManager>
    {
        public int CurrentLevel { get; set; }
    }
}