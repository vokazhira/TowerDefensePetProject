using System.Collections.Generic;
using UnityEngine;
using Waves.ScriptableObjects;

namespace Levels
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 54)]
    public class LevelData : ScriptableObject
    {
        public string LevelName;
        public List<WaveData> Waves = new List<WaveData>();
    }
}