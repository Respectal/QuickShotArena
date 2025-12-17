using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "HoleTileConfig", menuName = "Scriptable Objects/HoleTileConfig")]
    public class HoleTileConfig : ScriptableObject
    {
        [Header("Timing")]
        public float minSwitchTime = 3f;
        public float maxSwitchTime = 7f;
        public float flashDuration = 1.5f;
        public float flashSpeed = 6f;
        
        [Header("Probability")]
        public float goodOnSpawn = 0.5f;
        public float goodOnSwitch = 0.7f;

        [Header("Visuals")]
        public Color goodColor = Color.steelBlue;
        public Color goodFlashColor = Color.royalBlue;
        public Color badColor = Color.crimson;
        public Color badFlashColor = Color.darkRed;
    }
}
