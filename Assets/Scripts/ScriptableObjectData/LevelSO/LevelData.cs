using System.Collections.Generic;
using ScriptableObjectData.WaveSO;
using UnityEngine;

namespace ScriptableObjectData.LevelSO
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 54)]
    public class LevelData : ScriptableObject
    {
        public string LevelName;
        public List<WaveData> Waves = new List<WaveData>();
        public int CrystalPerWave;
    }
}