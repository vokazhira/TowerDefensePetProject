using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace ScriptableObjectData.WaveSO
{
    [Serializable]
    public class WaveContent
    {
        public EnemyType EnemyType;
        public int Count;
        public float DelayBetweenSpawns;
    }
    
    [CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData", order = 52)]
    public class WaveData : ScriptableObject
    {
        public string WaveName;
        public List<WaveContent> Contents = new List<WaveContent>();
        public float DelayBeforeWave;
    }
}