using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BallShooterConfig", menuName = "Scriptable Objects/BallShooterConfig")]
    public class BallShooterConfig : ScriptableObject
    {
        [Header("Trajectory Settings")]
        public int trajectoryPoints = 30;
        public float timeStep = 0.1f;
        public float powerCycleSpeed = 10f;

        [Header("Shot Settings")]
        public float powerMin = 5f;
        public float powerMax = 20f;
        public float reloadDelay = 0.5f;

        [Header("Ball Settings")]
        public float ballLifeSpan = 10f;
        
        [Header("Prefabs")]
        public GameObject ball;
        public GameObject tileSolid;
        
    }
}
