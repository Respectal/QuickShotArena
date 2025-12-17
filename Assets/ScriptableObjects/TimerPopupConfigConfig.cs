using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TimerPopupConfigConfig", menuName = "Scriptable Objects/TimerPopupConfigConfig")]
    public class TimerPopupConfigConfig : ScriptableObject
    {
        [Header("Timing")]
        public float lifetime = 1.2f;
        public float floatSpeed = 0.8f;

        [Header("Visuals")]
        public Color goodColor = Color.steelBlue;
        public Color badColor = Color.crimson;
    }
}
