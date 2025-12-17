using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TimerManagerConfig", menuName = "Scriptable Objects/TimerManagerConfig")]
    public class TimerManagerConfig : ScriptableObject
    {
        [Header("Timer Settings")]
        public float startTime = 30f;
        
        [Header("Timer Score Settings")]
        public int bonusTime = 5;
        public int penaltyTime = 10;
    }
}
