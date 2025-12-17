using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TileManagerConfig", menuName = "Scriptable Objects/TileManagerConfig")]
    public class TileManagerConfig : ScriptableObject
    {
        [Header("Tile Settings")]
        public GameObject tileSolid;
        public GameObject tileHole;
        public int height;
        public int width;

        [Header("Wall Settings")]
        public GameObject wallPrefab;
        public float wallHeight = 2f;
        public float wallThickness = 0.2f;

        [Header("Hole Settings")]
        public int holeCount = 5;
        public float holeRadius = 0.5f;
        public float minHoleDistance = 1.2f;
    }
}
