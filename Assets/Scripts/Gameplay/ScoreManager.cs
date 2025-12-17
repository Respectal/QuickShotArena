using Core;
using Core.Enums;
using UnityEngine;

namespace Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int scoreIncrease = 1;
        
        public int CurrentScore => _currentScore;
        public static ScoreManager Instance { get; private set; }
        public static int LastScore { get; set; }

        private int _currentScore;
        
        public void SaveScore()
        {
            LastScore = CurrentScore;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            _currentScore = 0;
        }
        
        private void OnEnable()
        {
            GameEvents.BallScored += OnBallScored;
        }

        private void OnDisable()
        {
            GameEvents.BallScored -= OnBallScored;
        }

        void OnBallScored(GameObject ball, HoleTile holeTile, HoleType holeType, Vector3 holePosition)
        {
            if (holeType is HoleType.Good)
            {
                _currentScore += scoreIncrease;
            }
        }
    }
}