using Core;
using Core.Enums;
using ScriptableObjects;
using UnityEngine;

namespace Gameplay
{
    public class TimerManager : MonoBehaviour
    {
        [SerializeField] private TimerManagerConfig config;

        public bool IsRunning { get; private set;}
        public float TimeRemaining => _timeRemaining;

        private float _timeRemaining;

        private void Awake()
        {
            _timeRemaining = config.startTime;
            IsRunning = true;
        }

        private void OnEnable()
        {
            GameEvents.BallScored += OnBallScored;
        }

        private void OnDisable()
        {
            GameEvents.BallScored -= OnBallScored;
        }

        private void Update()
        {
            if (!IsRunning)
            {
                return;
            }
            
            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining <= 0f)
            {
                _timeRemaining = 0f;
                IsRunning = false;
                GameOver();
            }
        }

        void OnBallScored(GameObject ball, HoleTile holeTile, HoleType holeType, Vector3 holePosition)
        {
            var delta = holeType switch
            {
                HoleType.Good => config.bonusTime,
                HoleType.Bad => -config.penaltyTime,
                _ => 0f
            };
            
            _timeRemaining += delta;
            
            GameEvents.RaiseTimeChanged(holeTile, holePosition, delta);
        }

        void GameOver()
        {
            IsRunning = false;
            _timeRemaining = 0f;
            
            GameEvents.RaiseGameOver();
            
            // Save last score
            ScoreManager.Instance.SaveScore();

            // Load GameOver scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
        }
    }
}